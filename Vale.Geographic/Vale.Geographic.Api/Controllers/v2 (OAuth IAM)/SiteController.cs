using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Vale.Geographic.Api.Models.Sites;
using Vale.Geographic.Domain.Entities;
using System.Linq;
using Vale.Geographic.Api.Provider;
using Microsoft.AspNetCore.Http;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Sites
    /// </summary>
    [Route("api/Site")]
    [IAMAuthorize]
    [ApiVersion("2")]
    public class SiteController : Controller
    {
        readonly ISiteAppService siteAppService;
        /// <summary>
        /// Constructor to Site Controller 
        /// </summary>
        public SiteController(ISiteAppService siteAppService)
        {
            this.siteAppService = siteAppService;
        }

        /// <summary>
        /// Get all Sites
        /// </summary>
        /// <param name="requestModel">Filter sites by unit name</param>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<SiteAsCountryDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]GetRequestModel requestModel)
        {
            var result = siteAppService.GetAll(requestModel.unit_name);

            if (result.Count() == 0)
                return NoContent();

            return Ok(result);
        }

        /// <summary>
        ///     Get Site by Id
        /// </summary>
        /// <param name="id">Site Id</param>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(SiteAsCountryDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            var result = siteAppService.GetById(id);

            return Ok(result);
        }

        /// <summary>
        ///     Get Site by Code
        /// </summary>
        /// <param name="codeSite">Site Code</param>
        [HttpGet]
        [ProducesResponseType(typeof(SiteAsCountryDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetByCode(string codeSite)
        {
            var result = siteAppService.GetByCode(codeSite);

            return Ok(result);
        }

        [HttpGet("env")]
        public IActionResult env()
        {
            return Ok(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower());
        }
    }
}