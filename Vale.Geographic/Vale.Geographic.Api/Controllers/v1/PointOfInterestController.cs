using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Vale.Geographic.Api.Controllers
{
    /// <summary>
    /// Controller to PointOfInterest
    /// </summary>
    [Route("api/PointOfInterest")]
    [Authorize]
    public class PointOfInterestController : Controller
    {
        private IPointOfInterestAppService PointOfInterestAppService { get; }
        /// <summary>
        /// Constructor to PointOfInterest Controller 
        /// </summary>
        /// <param name="pointOfInterestAppService">app service</param>
        public PointOfInterestController(IPointOfInterestAppService pointOfInterestAppService)
        {
            this.PointOfInterestAppService = pointOfInterestAppService;
        }


        /// <summary>
        ///     Delete a PointOfInterest by Id
        /// </summary>
        /// <param name="id">PointOfInterest Id</param>
        /// <param name="lastUpdatedBy"></param>
        /// <returns>No content</returns>
        /// <response code="204">PointOfInterest deleted!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
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
                PointOfInterestAppService.Delete(id, lastUpdatedBy);

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
        ///     Filter PointOfInterests
        /// </summary>
        /// <remarks>
        ///    Search PointOfInterests by filters
        /// </remarks>
        /// <param name="active">Retrive all points are active or not</param>
        /// <param name="Id"></param>
        /// <param name="categoryId"></param>
        /// <param name="areaId"></param>
        /// <param name="request">Filter parameters</param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="altitude"></param>
        /// <param name="radiusDistance"></param>
        /// <returns>PointOfInterests list have been solicited</returns>
        /// <response code="200">PointOfInterest list!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PointOfInterestDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] bool? active, Guid? Id, Guid? categoryId, Guid? areaId,  double? longitude, double? latitude, double? altitude, int? radiusDistance, DateTime? lastUpdatedAt, FilterDto request)
        {
            var total = 0;

            var result = PointOfInterestAppService.Get(active, Id, categoryId, areaId, longitude, latitude, altitude, radiusDistance, lastUpdatedAt, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


        /// <summary>
        /// Get all PointOfInterest with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="PointOfInterestDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<PointOfInterestDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = PointOfInterestAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get PointOfInterest by Id
        /// </summary>
        /// <param name="id">PointOfInterest Id</param>
        /// <returns>PointOfInterest that has been solicited</returns>
        /// <response code="200">PointOfInterest!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(PointOfInterestDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            return Ok(PointOfInterestAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new PointOfInterest
        /// </summary>
        /// <param name="value">PointOfInterest data</param>
        /// <returns>PointOfInterest who has been created</returns>
        /// <response code="201">PointOfInterest created!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(PointOfInterestDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] PointOfInterestDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = this.HttpContext.User.Identity.Name;            

            var response = PointOfInterestAppService.Insert(value);
            return Created("", response);
        }

        /// <summary>
        ///     Create a new Point Of Interest  
        /// </summary>
        /// <remarks>
        ///  Create new Point Of Interest using geoJson files
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("LargeScale")]
        [ProducesResponseType(typeof(CollectionPointOfInterestDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult PostLargeScale([FromBody] CollectionPointOfInterestDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            obj.CreatedBy = this.HttpContext.User.Identity.Name;

            var response = PointOfInterestAppService.Insert(obj);
            return Created("", response);
        }

        /// <summary>
        ///     Update a PointOfInterest
        /// </summary>
        /// <param name="id">PointOfInterest Id</param>
        /// <param name="value">PointOfInterest data</param>
        /// <returns>PointOfInterest who has been updated</returns>
        /// <response code="200">PointOfInterest updated!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(PointOfInterestDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] PointOfInterestDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(value.LastUpdatedBy))            
                value.LastUpdatedBy = this.HttpContext.User.Identity.Name;            

            try
            {
                return Ok(PointOfInterestAppService.Update(id, value));
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