using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Vale.Geographic.Api.Models.Perimeters;
using System.Linq;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Vale Oficial Perimeter
    /// </summary>
    [Route("api/Perimeter")]
    [Authorize]
    [ApiVersion("2")]
    public class PerimeterController : Controller
    {
        private IPerimeterAppService PerimeterAppService { get; }
        /// <summary>
        /// Constructor to Vale Oficial Perimeter Controller 
        /// </summary>
        public PerimeterController(IPerimeterAppService perimeterAppService)
        {
            this.PerimeterAppService = perimeterAppService;
        }

        /// <summary>
        /// Get all Vale Oficial Perimeters with paging, filtering and sorting.
        /// </summary>
        /// <remarks>Get all Vale Oficial Perimeter with paging, filtering and sorting.
        /// </remarks>
        /// <param name="defaultModel">
        /// Filter/Sort
        /// </param>
        /// <param name="requestModel">Filter perimeters by site</param>
        /// <returns>List of <see cref="PerimeterDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<PerimeterDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto defaultModel, [FromQuery]GetRequestModel requestModel)
        {
            var result = PerimeterAppService.GetAll(defaultModel, requestModel.site, out int total);
            Response.Headers.Add("X-Total-Count", total.ToString());


            if (requestModel.only_names.HasValue && requestModel.only_names == true)
            {
                return Ok(result
                    .Select(s => new
                    {   
                        s.Id,
                        s.Name
                    })
                    .ToList()
                );
            }

            return Ok(result);
        }

        /// <summary>
        ///     Get Vale Oficial Perimeter by Id
        /// </summary>
        /// <param name="id">Vale Oficial Perimeter Id</param>
        /// <returns>Area that has been solicited</returns>
        /// <response code="200">Area!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(AreaDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            PerimeterDto perimeter = PerimeterAppService.GetById(id);

            if (perimeter == null)
                return NoContent();

            return Ok(perimeter);
        }

        /// <summary>
        ///     Create a new Oficial Vale Perimeter
        /// </summary>
        /// <param name="model">Perimeter data</param>
        /// <returns>Perimeter who has been created</returns>
        /// <response code="201">Perimeter created!</response>
        /// <response code="400">Perimeter has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(PerimeterDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] PostRequestModel model)
        {
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var response = PerimeterAppService.Insert(new PerimeterDto
            {
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                CreatedBy = this.HttpContext.User.Identity.Name,
                LastUpdatedBy = this.HttpContext.User.Identity.Name,
                Geojson = model.Geojson,
                Name = model.Name,
                Status = true,
                Sites = model.Sites
            });
            return Created("", response);
        }

        /// <summary>
        ///     Update an Oficial Vale Perimeter
        /// </summary>
        /// <param name="id"> Oficial Vale Perimeter Id</param>
        /// <param name="model"> Oficial Vale Perimeter data</param>
        /// <returns> Oficial Vale Perimeter who has been updated</returns>
        /// <response code="200"> Oficial Vale Perimeter updated!</response>
        /// <response code="400"> Oficial Vale Perimeter has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(AreaDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] PutRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            PerimeterDto perimeterUpdated = PerimeterAppService.Update(new PerimeterDto
            {
                Id = id,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                CreatedBy = this.HttpContext.User.Identity.Name,
                LastUpdatedBy = this.HttpContext.User.Identity.Name,
                Geojson = model.Geojson,
                Name = model.Name,
                Status = model.Status,
                Sites = model.Sites
            });

            if (perimeterUpdated == null)
                return NotFound("Perimeter " + id);

            return Ok();
        }

        /// <summary>
        ///     File an Oficial Vale Perimeter (delete virtually)
        /// </summary>
        /// <param name="id"> Oficial Vale Perimeter Id</param>
        /// <returns> Oficial Vale Perimeter who has been updated</returns>
        /// <response code="204"> Oficial Vale Perimeter filed!</response>
        /// <response code="405"> Oficial Vale Perimeter cannot be delete right now</response>
        /// <response code="500">Oops! Internal error</response>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 405)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                bool deleted = PerimeterAppService.Delete(id, this.HttpContext.User.Identity.Name);

                if (deleted)
                    return NoContent();
                else
                    return StatusCode(405);
            } catch (ArgumentException e)
            {
                return BadRequest(e);
            } catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}