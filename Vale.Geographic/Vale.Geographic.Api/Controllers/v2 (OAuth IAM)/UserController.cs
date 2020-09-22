using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vale.Geographic.Api.Filters;
using Vale.Geographic.Api.Provider;
using Vale.Geographic.Application.Dto.Authorization;
using Vale.Geographic.Application.Services;

namespace Vale.Geographic.Api.Controllers.v2
{
    /// <summary>
    /// Controller to User
    /// </summary>
    [Route("api/User")]
    [IAMAuthorize]
    [ApiVersion("2")]
    public class UserController : Controller
    {
        private IUserAppService UserAppService { get; }

        /// <summary>
        /// Constructor to User Controller 
        /// </summary>
        /// <param name="UserAppService"></param>
        public UserController(IUserAppService UserAppService)
        {
            this.UserAppService = UserAppService;
        }
        

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            var response = this.UserAppService.GetById(id);
            return Ok(response);
        }

        /// <summary>
        /// Get User by Matricula
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        [HttpGet("{matricula}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetByMatricula(string matricula)
        {
            var response = this.UserAppService.GetByMatricula(matricula);
            return Ok(response);
        }    


        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] UserDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.CreatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            var response = this.UserAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] UserDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            value.LastUpdatedBy = HttpContext.Session.GetString("USER_INFO_IAM_ID");

            try
            {
                return Ok(this.UserAppService.Update(id, value));
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
        /// Delete User by Id
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
                this.UserAppService.Delete(id, lastUpdatedBy);

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