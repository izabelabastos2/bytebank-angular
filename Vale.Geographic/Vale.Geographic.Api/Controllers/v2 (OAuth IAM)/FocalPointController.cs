using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to FocalPoint
    /// </summary>
    [Route("api/FocalPoint")]
    [Authorize]
    [ApiVersion("2")]
    public class FocalPointController : Controller
    {
        private IFocalPointAppService FocalPointAppService { get; }

        /// <summary>
        ///  Constructor to FocalPoint Controller 
        /// </summary>
        /// <param name="FocalPointAppService"></param>
        public FocalPointController(IFocalPointAppService FocalPointAppService)
        {
            this.FocalPointAppService = FocalPointAppService;
        }

        /// <summary>
        /// Get all FocalPoint with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<FocalPointDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = this.FocalPointAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        /// Get FocalPoint by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(FocalPointDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            var response = this.FocalPointAppService.GetById(id);
            return Ok(response);
        }

        /// <summary>
        /// Get FocalPoint by Matricula Active
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        [HttpGet("{matricula}")]
        [ProducesResponseType(typeof(FocalPointDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetByMatricula(string matricula)
        {
            var response = this.FocalPointAppService.GetByMatricula(matricula);
            return Ok(response);
        }
        /// <summary>
        ///     Filter FocalPoint
        /// </summary>
        /// <remarks>
        ///    Search FocalPoint by filters
        /// </remarks>
        /// <param name="active"></param>
        /// <param name="localityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <param name="device"></param>
        /// <param name="request"></param>        
        /// <returns>Areas list have been solicited</returns>
        /// <response code="200">Area list!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FocalPointDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] bool? active, Guid? localityId, Guid? pointOfInterestId, string matricula, FilterDto request)
        {
            var total = 0;

            var result = FocalPointAppService.Get(active, localityId, pointOfInterestId, matricula, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


        /// <summary>
        /// Create new FocalPoint
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FocalPointDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] FocalPointDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            var response = this.FocalPointAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        /// Update FocalPoint
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(FocalPointDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] FocalPointDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.LastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                return Ok(this.FocalPointAppService.Update(id, value));
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


        /// <summary>
        /// Delete FocalPoint by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <returns></returns>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            var lastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                this.FocalPointAppService.Delete(id, lastUpdatedBy);

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
               
    }
}