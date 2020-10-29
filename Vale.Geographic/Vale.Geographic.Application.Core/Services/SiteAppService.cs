using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System;

namespace Vale.Geographic.Application.Core.Services
{
    public class SiteAppService : AppService, ISiteAppService
    {
        private readonly ISitesRepository sitesRepository;


        public SiteAppService(IUnitOfWork uoW,
                              IMapper mapper,
                              ISitesRepository sitesRepository) : base(uoW, mapper)
        {
            this.sitesRepository = sitesRepository;
        }


        public IEnumerable<SiteAsCountryDto> GetAll(string unitNameFilter)
        {
            var sites = sitesRepository.GetAll().Where(w => w.Status = true);

            IEnumerable<SiteAsCountryDto> countries = MapSitesToSitesTree(sites, unitNameFilter, null);

            return countries;
        }

        public SiteAsCountryDto GetById(Guid id)
        {
            var sites = sitesRepository.GetAll().Where(w => w.Status = true);

            IEnumerable<SiteAsCountryDto> countries = MapSitesToSitesTree(sites, null, id);

            return countries.FirstOrDefault();
        }

        private IEnumerable<SiteAsCountryDto> MapSitesToSitesTree (IEnumerable<Site> sites, string unitNameFilter, Guid? unitIdFilter)
        {
            var sitesAsTree = sites
                .Where(w => w.ParentId == null)
                .Select(country => new SiteAsCountryDto
                {
                    CreatedAt = country.CreatedAt,
                    CreatedBy = country.CreatedBy,
                    Id = country.Id,
                    LastUpdatedAt = country.LastUpdatedAt,
                    LastUpdatedBy = country.LastUpdatedBy,
                    Latitude = country.Latitude,
                    Longitude = country.Longitude,
                    Name = country.Name,
                    Radius = country.Radius,
                    Status = country.Status,
                    Zoom = country.Zoom,
                    States = sites
                        .Where(w => w.ParentId == country.Id)
                        .Select(state => new SiteAsStateDto
                        {
                            CreatedAt = state.CreatedAt,
                            CreatedBy = state.CreatedBy,
                            Id = state.Id,
                            LastUpdatedAt = state.LastUpdatedAt,
                            LastUpdatedBy = state.LastUpdatedBy,
                            Latitude = state.Latitude,
                            Longitude = state.Longitude,
                            Name = state.Name,
                            Radius = state.Radius,
                            Status = state.Status,
                            Zoom = state.Zoom,
                            Units = sites
                                .Where(w => w.ParentId == state.Id && 
                                    (unitNameFilter == null ? true : w.Name.ToUpper().Contains(unitNameFilter.ToUpper())) &&
                                    (unitIdFilter == null ? true : w.Id == unitIdFilter))
                                .Select(site => new SiteDto
                                {
                                    CreatedAt = site.CreatedAt,
                                    CreatedBy = site.CreatedBy,
                                    Id = site.Id,
                                    LastUpdatedAt = site.LastUpdatedAt,
                                    LastUpdatedBy = site.LastUpdatedBy,
                                    Latitude = site.Latitude,
                                    Longitude = site.Longitude,
                                    Name = site.Name,
                                    Radius = site.Radius,
                                    Status = site.Status,
                                    Zoom = site.Zoom,
                                })
                        })
                });

            return sitesAsTree
                .Select(country =>
                {
                    country.States = country.States.Where(w => w.Units.Count() > 0);

                    return country;
                })
                .Where(w => w.States.Count() > 0);
        }
    }
}