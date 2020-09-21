using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Api.Provider;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Audit
    /// </summary>
    [Route("api/Auditory")]
    [IAMAuthorize]
    [ApiVersion("2")]
    public class AuditoryController : Controller
    {
        private IAuditoryAppService AuditoryAppService { get; }

        /// <summary>
        /// Constructor to Auditory Controller 
        /// </summary>
        /// <param name="auditoryAppService"></param>
        public AuditoryController(IAuditoryAppService auditoryAppService)
        {
            AuditoryAppService = auditoryAppService;
        }

        /// <summary>
        /// Get all Audit with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<AuditoryDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = this.AuditoryAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        /// Get Audit by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(AuditoryDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            var response = this.AuditoryAppService.GetById(id);
            return Ok(response);
        }

        /// <summary>
        ///     Filter Audit
        /// </summary>
        /// <remarks>
        ///    Search Audit by filters
        /// </remarks>
        /// <param name="areaId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <param name="categoryId"></param>
        /// <param name="typeEntitie"></param>
        /// <param name="request">Filter parameters</param>
        /// <returns>Areas list have been solicited</returns>
        /// <response code="200">Area list!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuditoryDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] Guid? areaId, Guid? pointOfInterestId, Guid? categoryId, TypeEntitieEnum? typeEntitie, [FromQuery] FilterDto request)
        {
            var total = 0;

            var result = AuditoryAppService.Get(areaId, pointOfInterestId, categoryId, typeEntitie, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


    }
}