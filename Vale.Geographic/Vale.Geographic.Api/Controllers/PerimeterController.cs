using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Vale.Geographic.Api.Models.Perimeters;

namespace Vale.Geographic.Api.Controllers
{
    /// <summary>
    /// Controller to Perimeter
    /// </summary>
    [Route("api/Perimeter")]
    [Authorize]
    public class PerimeterController : Controller
    {
        private IPerimeterAppService PerimeterAppService { get; }
        /// <summary>
        /// Constructor to Area Controller 
        /// </summary>
        public PerimeterController(IPerimeterAppService perimeterAppService)
        {
            this.PerimeterAppService = perimeterAppService;
        }

        /// <summary>
        /// Get all Area with paging, filtering and sorting.
        /// </summary>
        /// <remarks>Get all Area with paging, filtering and sorting.
        /// </remarks>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="PerimeterDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<PerimeterDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {
            var result = PerimeterAppService.GetAll(parameters, out int total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get Area by Id
        /// </summary>
        /// <param name="id">Area Id</param>
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
            return Ok();
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
                Status = model.Status,
                Sites = model.Sites
            });
            return Created("", response);
        }
    }
}