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
    /// Controller to Segment
    /// </summary>
    [Route("api/Segment")]
    [Authorize]
    public class SegmentController : Controller
    {
        private ISegmentAppService SegmentAppService { get; }
        /// <summary>
        /// Constructor to Segment Controller 
        /// </summary>
        /// <param name="segmentAppService">app service</param>
        public SegmentController(ISegmentAppService segmentAppService)
        {
            this.SegmentAppService = segmentAppService;
        }


        /// <summary>
        ///     Delete a Segment by Id
        /// </summary>
        /// <param name="id">Segment Id</param>
        /// <param name="lastUpdatedBy"></param>
        /// <returns>No content</returns>
        /// <response code="204">Segment deleted!</response>
        /// <response code="400">Segment has missing/invalid values</response>
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
                SegmentAppService.Delete(id, lastUpdatedBy);
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
        ///     Filter Segments
        /// </summary>
        /// <remarks>
        ///    Search Segments by filters
        /// </remarks>
        /// <param name="active">Retrive all perssons are active or not</param>
        /// <param name="id"></param>
        /// <param name="areaId"></param>
        /// <param name="routeId"></param>
        /// <param name="request">Filter parameters</param>
        /// <returns>Segments list have been solicited</returns>
        /// <response code="200">Segment list!</response>
        /// <response code="400">Segment has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SegmentDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] bool? active, Guid? id, Guid? areaId, Guid? routeId, FilterDto request)
        {
            var total = 0;

            var result = SegmentAppService.Get(active, id, areaId, routeId, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

               
        /// <summary>
        /// Get all Segment with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="SegmentDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<SegmentDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = SegmentAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get Segment by Id
        /// </summary>
        /// <param name="id">Segment Id</param>
        /// <returns>Segment that has been solicited</returns>
        /// <response code="200">Segment!</response>
        /// <response code="400">Segment has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(SegmentDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            return Ok(SegmentAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new Segment
        /// </summary>
        /// <param name="value">Segment data</param>
        /// <returns>Segment who has been created</returns>
        /// <response code="201">Segment created!</response>
        /// <response code="400">Segment has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(SegmentDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] SegmentDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = this.HttpContext.User.Identity.Name;

            var response = SegmentAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        ///     Update a Segment
        /// </summary>
        /// <param name="id">Segment Id</param>
        /// <param name="value">Segment data</param>
        /// <returns>Segment who has been updated</returns>
        /// <response code="200">Segment updated!</response>
        /// <response code="400">Segment has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(SegmentDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] SegmentDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.LastUpdatedBy = this.HttpContext.User.Identity.Name;

            try
            {
                return Ok(SegmentAppService.Update(id, value));
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