using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;
using System;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Application.Core.Services
{
    public class PerimeterAppService : AppService, IPerimeterAppService
    {
        private readonly IAreaRepository areaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISitesPerimetersRepository sitesPerimetersRepository;
        private readonly ISitesRepository sitesRepository;
        private readonly IAreaService areaService;
        private readonly IAuditoryService auditoryService;


        public PerimeterAppService(IUnitOfWork uoW,
                              IMapper mapper,
                              IAreaService areaService,
                              IAreaRepository areaRepository,
                              ICategoryRepository categoryRepository,
                              ISitesPerimetersRepository sitesPerimetersRepository,
                              ISitesRepository sitesRepository,
                              IAuditoryService auditoryService) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this.areaRepository = areaRepository;
            this.categoryRepository = categoryRepository;
            this.sitesPerimetersRepository = sitesPerimetersRepository;
            this.sitesRepository = sitesRepository;
            this.auditoryService = auditoryService;
        }


        public IEnumerable<PerimeterDto> GetAll(IFilterParameters parameters, Guid? filterBySiteId, out int total)
        {
            List<Guid> perimetersAssociatedToTheFilteredSite = new List<Guid>();
            Guid categoryId = GetOficialPerimeterCategoryId();
            IEnumerable<Area> perimeters;

            if (filterBySiteId != null)
            {
                try
                {
                    perimetersAssociatedToTheFilteredSite = sitesPerimetersRepository
                        .GetAll(x => x.SiteId == filterBySiteId && x.Status == true)
                        .Select(s => s.AreaId)
                        .ToList();
                } catch
                {
                    perimetersAssociatedToTheFilteredSite = new List<Guid>();
                }

                perimeters = areaRepository
                   .GetAll(
                       x => (perimetersAssociatedToTheFilteredSite.Count > 0 ? perimetersAssociatedToTheFilteredSite.Contains(x.Id) : false) && x.CategoryId == categoryId && x.Status == true,
                       parameters,
                       new string[] { "CategoryId", "Status" }
                   )
                   .ApplyPagination(parameters, out total);
            } else
            {
                perimeters = areaRepository
                    .GetAll(
                        x => x.CategoryId == categoryId && x.Status == true,
                        parameters,
                        new string[] { "CategoryId", "Status" }
                    )
                    .ApplyPagination(parameters, out total);
            }

            return Mapper.Map<IEnumerable<PerimeterDto>>(perimeters);
        }

        public PerimeterDto GetById(Guid id)
        {
            Guid categoryId = GetOficialPerimeterCategoryId();
            Area perimeter = areaRepository.GetById(id);

            List<Guid> sites;

            try
            {
                sites = sitesPerimetersRepository
                    .GetAll(x => x.AreaId == id && x.Status == true)
                    .Select(s => s.SiteId)
                    .ToList();
            } catch
            {
                sites = new List<Guid>();
            }

            if (perimeter != null && perimeter.CategoryId != categoryId)
                return null;

            PerimeterDto perimeterDto = Mapper.Map<PerimeterDto>(perimeter);
            perimeterDto.Sites = sites;

            return perimeterDto;
        }

        public PerimeterDto Insert(PerimeterDto perimeterToCreate)
        {
            try
            {
                UoW.BeginTransaction();

                Area perimeter = Mapper.Map<Area>(perimeterToCreate);

                perimeter.CategoryId = GetOficialPerimeterCategoryId();
                perimeter.LastUpdatedBy = perimeter.CreatedBy;

                var json = JsonConvert.SerializeObject(perimeterToCreate.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                perimeter.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();

                Area createdPerimeter = areaService.Insert(perimeter);

                perimeterToCreate.Sites.ForEach(site =>
                {
                    sitesPerimetersRepository.Insert(new SitesPerimeter
                    {
                        AreaId = createdPerimeter.Id,
                        SiteId = site,
                        CreatedAt = perimeterToCreate.CreatedAt,
                        CreatedBy = perimeterToCreate.CreatedBy,
                        LastUpdatedAt = perimeterToCreate.LastUpdatedAt,
                        LastUpdatedBy = perimeterToCreate.LastUpdatedBy,
                        Status = true,
                    });
                });

                UoW.Commit();

                return Mapper.Map<PerimeterDto>(createdPerimeter);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public PerimeterDto Update(PerimeterDto perimeterToUpdate)
        {
            try
            {
                Guid categoryId = GetOficialPerimeterCategoryId();
                Area alreadyCreatedPerimeter = areaRepository.GetById(perimeterToUpdate.Id.Value);
                List<SitesPerimeter> sitesAlreadyLinkedToPerimeter;

                try
                {
                    sitesAlreadyLinkedToPerimeter = sitesPerimetersRepository
                        .GetAll(x => x.AreaId == perimeterToUpdate.Id.Value)
                        .ToList();
                    
                    if (sitesAlreadyLinkedToPerimeter == null) sitesAlreadyLinkedToPerimeter = new List<SitesPerimeter>();
                } catch
                {
                    sitesAlreadyLinkedToPerimeter = new List<SitesPerimeter>();
                }
                    
                UoW.Context.Entry(alreadyCreatedPerimeter).State = EntityState.Detached;

                if (alreadyCreatedPerimeter == null || alreadyCreatedPerimeter.CategoryId != categoryId)
                    return null;

                UoW.BeginTransaction();

                var json = JsonConvert.SerializeObject(perimeterToUpdate.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                Area areaToUpdate = new Area
                {
                    Id = perimeterToUpdate.Id.Value,
                    CategoryId = categoryId,
                    CreatedAt = alreadyCreatedPerimeter.CreatedAt,
                    CreatedBy = alreadyCreatedPerimeter.CreatedBy,
                    LastUpdatedAt = perimeterToUpdate.LastUpdatedAt,
                    LastUpdatedBy = perimeterToUpdate.LastUpdatedBy,
                    Name = perimeterToUpdate.Name,
                    Status = perimeterToUpdate.Status,
                    Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse(),
                };

                areaService.Update(areaToUpdate);

                InsertAuditory(areaToUpdate, alreadyCreatedPerimeter);

                UpdateSitesLinkedToPerimeter(areaToUpdate, sitesAlreadyLinkedToPerimeter, perimeterToUpdate.Sites);

                UoW.Commit();

                return Mapper.Map<PerimeterDto>(perimeterToUpdate);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        private Guid GetOficialPerimeterCategoryId()
        {
            var oficialPerimeterCategory = categoryRepository
                .GetAll(x => x.Status == true && x.TypeEntitie == TypeEntitieEnum.OficialPerimeter)
                .FirstOrDefault();

            if (oficialPerimeterCategory != null)
                return oficialPerimeterCategory.Id;
            else
                return new Guid();
        }

        private void InsertAuditory(Area newPerimeter, Area oldPerimeter)
        {
            var audit = new Auditory();
            audit.AreaId = newPerimeter.Id;
            audit.TypeEntitie = TypeEntitieEnum.OficialPerimeter;
            audit.CreatedBy = newPerimeter.LastUpdatedBy;
            audit.LastUpdatedBy = newPerimeter.LastUpdatedBy;
            audit.Status = true;


            if (!newPerimeter.Equals(oldPerimeter))
            {
                var json = GeoJsonSerializer.Create();
                var sw = new System.IO.StringWriter();

                json.Serialize(sw, newPerimeter);
                audit.NewValue = sw.ToString();

                sw = new System.IO.StringWriter();
                json.Serialize(sw, oldPerimeter);

                audit.OldValue = sw.ToString();

                auditoryService.Insert(audit);
            }

        }

        private void UpdateSitesLinkedToPerimeter(Area updatedPerimeter, List<SitesPerimeter> sitesPerimetersAlreadyInDB, List<Guid> sitesIds)
        {
            List<SitesPerimeter> sitesPerimetersToUpdate = new List<SitesPerimeter>();
            List<SitesPerimeter> sitesPerimetersToCreate = new List<SitesPerimeter>();
            List<SitesPerimeter> sitesPerimetersToFile = new List<SitesPerimeter>();

            foreach (var siteId in sitesIds)
            {
                if (sitesPerimetersAlreadyInDB.Any(a => a.SiteId == siteId && a.AreaId == updatedPerimeter.Id))
                {
                    var sitePerimeterAlreadyCreated = sitesPerimetersAlreadyInDB
                        .Where(w => w.SiteId == siteId && w.AreaId == updatedPerimeter.Id)
                        .FirstOrDefault();
                    sitesPerimetersToUpdate.Add(sitePerimeterAlreadyCreated);
                }
                else
                {
                    sitesPerimetersToCreate.Add(new SitesPerimeter
                    {
                        Id = Guid.NewGuid(),
                        AreaId = updatedPerimeter.Id,
                        SiteId = siteId,
                        Status = true,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow,
                        CreatedBy = updatedPerimeter.LastUpdatedBy,
                        LastUpdatedBy = updatedPerimeter.LastUpdatedBy,
                    });
                }
            }

            foreach(var sitePerimeterAlreadyInDB in sitesPerimetersAlreadyInDB)
                if (!sitesPerimetersToUpdate.Any(a => a.Id == sitePerimeterAlreadyInDB.Id))
                    sitesPerimetersToFile.Add(sitePerimeterAlreadyInDB);

            foreach(var sitePerimeterToCreate in sitesPerimetersToCreate)
                sitesPerimetersRepository.Insert(sitePerimeterToCreate);

            foreach (var sitePerimeterToUpdate in sitesPerimetersToUpdate)
            {
                UoW.Context.Entry(sitePerimeterToUpdate).State = EntityState.Detached;

                var updatedSitePerimeter = new SitesPerimeter
                {
                    Id = sitePerimeterToUpdate.Id,
                    AreaId = sitePerimeterToUpdate.AreaId,
                    SiteId = sitePerimeterToUpdate.SiteId,
                    CreatedAt = sitePerimeterToUpdate.CreatedAt,
                    CreatedBy = sitePerimeterToUpdate.CreatedBy,
                    LastUpdatedAt = DateTime.UtcNow,
                    LastUpdatedBy = updatedPerimeter.LastUpdatedBy,
                    Status = true
                };

                sitesPerimetersRepository.Update(updatedSitePerimeter);

                auditoryService.Insert(new Auditory
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    CreatedBy = updatedPerimeter.LastUpdatedBy,
                    LastUpdatedBy = updatedPerimeter.LastUpdatedBy,
                    Status = true,
                    TypeEntitie = TypeEntitieEnum.SiteXOficialPerimeter,
                    OldValue = JsonConvert.SerializeObject(sitePerimeterToUpdate).ToString(),
                    NewValue = JsonConvert.SerializeObject(updatedSitePerimeter).ToString()
                });
            }

            foreach (var sitePerimeterToFile in sitesPerimetersToFile)
            {
                UoW.Context.Entry(sitePerimeterToFile).State = EntityState.Detached;

                var updatedSitePerimeterToFile = new SitesPerimeter
                {
                    Id = sitePerimeterToFile.Id,
                    AreaId = sitePerimeterToFile.AreaId,
                    SiteId = sitePerimeterToFile.SiteId,
                    CreatedAt = sitePerimeterToFile.CreatedAt,
                    CreatedBy = sitePerimeterToFile.CreatedBy,
                    LastUpdatedAt = DateTime.UtcNow,
                    LastUpdatedBy = updatedPerimeter.LastUpdatedBy,
                    Status = false
                };

                sitesPerimetersRepository.Update(updatedSitePerimeterToFile);

                auditoryService.Insert(new Auditory
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    CreatedBy = updatedPerimeter.LastUpdatedBy,
                    LastUpdatedBy = updatedPerimeter.LastUpdatedBy,
                    Status = true,
                    TypeEntitie = TypeEntitieEnum.SiteXOficialPerimeter,
                    OldValue = JsonConvert.SerializeObject(sitePerimeterToFile).ToString(),
                    NewValue = JsonConvert.SerializeObject(updatedSitePerimeterToFile).ToString()
                });
            }
        }
    }
}