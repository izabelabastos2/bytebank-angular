using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using Vale.Geographic.Api.Provider;
using Microsoft.AspNetCore.Http;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to Category
    /// </summary>
    [Route("api/Category")]
    [IAMAuthorize]
    [ApiVersion("2")]
    public class CategoryController : Controller
    {
        private ICategoryAppService CategoryAppService { get; }

        /// <summary>
        /// Constructor to Category Controller 
        /// </summary>
        /// <param name="categoryAppService"></param>
        public CategoryController(ICategoryAppService categoryAppService)
        {
            this.CategoryAppService = categoryAppService;
        }

        /// <summary>
        /// Get all Category with paging, filtering and sorting.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = this.CategoryAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        /// Get Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            var response = this.CategoryAppService.GetById(id);
            return Ok(response);
        }

        /// <summary>
        ///     Filter Category
        /// </summary>
        /// <remarks>
        ///    Search Category by filters
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="active">Retrive all areas are active or not</param>
        /// <param name="TypeEntitie"></param>
        /// <param name="categoryId"></param>
        /// <param name="parentId"></param>
        /// <param name="areaId"></param>
        /// <param name="request">Filter parameters</param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="altitude"></param>
        /// <returns>Areas list have been solicited</returns>
        /// <response code="200">Area list!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] Guid? id, bool? active, TypeEntitieEnum? TypeEntitie, DateTime? lastUpdatedAt, FilterDto request)
        {
            var total = 0;

            var result = CategoryAppService.Get(id, active, TypeEntitie, lastUpdatedAt, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


        /// <summary>
        /// Create new Category
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] CategoryDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            var response = this.CategoryAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] CategoryDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.LastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                return Ok(this.CategoryAppService.Update(id, value));
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
        /// Delete Category by Id
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
                this.CategoryAppService.Delete(id, lastUpdatedBy);

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