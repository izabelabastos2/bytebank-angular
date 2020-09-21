using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Area
    /// </summary>
    [Route("api/Area")]
    [Authorize]
    [ApiVersion("2")]
    public class AreaController : Controller
    {
        private IAreaAppService AreaAppService { get; }
        /// <summary>
        /// Constructor to Area Controller 
        /// </summary>
        /// <param name="areaAppService">app service</param>
        public AreaController(IAreaAppService areaAppService)
        {
            this.AreaAppService = areaAppService;
        }


        /// <summary>
        ///     Delete a Area by Id
        /// </summary>
        /// <param name="id">Area Id</param>
        /// <param name="lastUpdatedBy"></param>
        /// <returns>No content</returns>
        /// <response code="204">Area deleted!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            var lastUpdatedBy = this.HttpContext.User.Identity.Name;

            try
            {
                AreaAppService.Delete(id, lastUpdatedBy);

            }
            catch (ArgumentNullException)
            {

                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        ///     Filter Areas
        /// </summary>
        /// <remarks>
        ///    Search Areas by filters
        /// </remarks>
        /// <param name="active">Retrive all areas are active or not</param>
        /// <param name="id"></param>
        /// <param name="categoryId"></param>
        /// <param name="parentId"></param>
        /// <param name="areaId"></param>
        /// <param name="request">Filter parameters</param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="altitude"></param>
        /// <param name="radiusDistance"></param>
        /// <returns>Areas list have been solicited</returns>
        /// <response code="200">Area list!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AreaDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] bool? active, Guid? id, Guid? categoryId, Guid? parentId, double? longitude, double? latitude, double? altitude, int? radiusDistance, DateTime? lastUpdatedAt, FilterDto request)
        {
            var total = 0;

            var result = AreaAppService.Get(active, id, categoryId, parentId, longitude, latitude, altitude, radiusDistance, lastUpdatedAt, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);

        }
      
        /// <summary>
        /// Get all Area with paging, filtering and sorting.
        /// </summary>
        /// <remarks>Get all Area with paging, filtering and sorting.
        /// </remarks>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="AreaDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<AreaDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {
            var total = 0;

            var result = AreaAppService.GetAll(parameters, out total);
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
            return Ok(AreaAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new Area
        /// </summary>
        /// <param name="value">Area data</param>
        /// <returns>Area who has been created</returns>
        /// <response code="201">Area created!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(AreaDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] AreaDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = this.HttpContext.User.Identity.Name;

            var response = AreaAppService.Insert(value);
            return Created("", response);
        }

        /// <summary>
        ///     Create a new Areas  
        /// </summary>
        /// <remarks>
        ///  Create new areas using geoJson files
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("LargeScale")]
        [ProducesResponseType(typeof(AreaDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult PostLargeScale([FromBody] CollectionAreaDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            obj.CreatedBy = this.HttpContext.User.Identity.Name;

            var response = AreaAppService.Insert(obj);
            return Created("", response);
        }


        /// <summary>
        ///     Update a Area
        /// </summary>
        /// <param name="id">Area Id</param>
        /// <param name="value">Area data</param>
        /// <returns>Area who has been updated</returns>
        /// <response code="200">Area updated!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(AreaDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] AreaDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.LastUpdatedBy = this.HttpContext.User.Identity.Name;

            try
            {
                return Ok(AreaAppService.Update(id, value));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}