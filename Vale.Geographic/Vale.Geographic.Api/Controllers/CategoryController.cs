using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vale.Geographic.Api.Filters;
using Vale.Geographic.Application.Dto;
using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Base;

namespace Vale.Geographic.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoryAppService CategoryAppService { get; }

        /// <summary>
        /// 
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

            return Ok(this.CategoryAppService.Update(id, value));
        }

        /// <summary>
        /// Delete Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            this.CategoryAppService.Delete(id);
            return NoContent();
        }
    }
}