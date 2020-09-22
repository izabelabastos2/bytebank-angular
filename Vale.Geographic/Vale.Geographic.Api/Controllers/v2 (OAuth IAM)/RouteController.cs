using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Vale.Geographic.Api.Provider;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Route
    /// </summary>
    [Route("api/Route")]
    [IAMAuthorize]
    [ApiVersion("2")]
    public class RouteController : Controller
    {
        private IRouteAppService RouteAppService { get; }
        /// <summary>
        /// Constructor to Route Controller 
        /// </summary>
        /// <param name="routeAppService">app service</param>
        public RouteController(IRouteAppService routeAppService)
        {
            this.RouteAppService = routeAppService;
        }


        /// <summary>
        ///     Delete a Route by Id
        /// </summary>
        /// <param name="id">Route Id</param>
        /// <returns>No content</returns>
        /// <response code="204">Route deleted!</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            var lastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                RouteAppService.Delete(id, lastUpdatedBy);
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
        ///     Filter Routes
        /// </summary>
        /// <remarks>
        ///    Search Routes by filters
        /// </remarks>
        /// <param name="active">Retrive all perssons are active or not</param>
        /// <param name="areaId"></param>
        /// <param name="request">Filter parameters</param>
        /// <returns>Routes list have been solicited</returns>
        /// <response code="200">Route list!</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RouteDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] bool? active, Guid? id, Guid? areaId, FilterDto request)
        {
            var total = 0;

            var result = RouteAppService.Get(active, id, areaId, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

               
        /// <summary>
        /// Get all Route with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="RouteDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<RouteDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = RouteAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get Route by Id
        /// </summary>
        /// <remarks>
        ///     Get Route by Id
        /// </remarks>
        /// <param name="id">Route Id</param>
        /// <returns>Route that has been solicited</returns>
        /// <response code="200">Route!</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(RouteDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            return Ok(RouteAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new Route
        /// </summary>
        /// <remarks>
        ///     Create a new Route
        /// </remarks>
        /// <param name="value">Route data</param>
        /// <returns>Route who has been created</returns>
        /// <response code="201">Route created!</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(RouteDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] RouteDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            var response = RouteAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        ///     Update a Route
        /// </summary>
        /// <param name="id">Route Id</param>
        /// <param name="value">Route data</param>
        /// <returns>Route who has been updated</returns>
        /// <response code="200">Route updated!</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(RouteDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] RouteDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

             value.LastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                return Ok(RouteAppService.Update(id, value));
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