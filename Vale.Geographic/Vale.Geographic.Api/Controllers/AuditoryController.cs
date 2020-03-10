using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Vale.Geographic.Api.Controllers
{
    [Route("api/Auditory")]
    [ApiController]
    public class AuditoryController : Controller
    {
        private IAuditoryAppService AuditoryAppService { get; }

        public AuditoryController(IAuditoryAppService auditoryAppService)
        {
            AuditoryAppService = auditoryAppService;
        }

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
        /// Get Category by Id
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
        ///     Filter Areas
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. No entanto, não podemos esquecer que o
        ///     consenso sobre a necessidade de qualificação oferece uma interessante oportunidade para verificação do sistema de
        ///     participação geral. Do mesmo modo, o novo modelo estrutural aqui preconizado garante a contribuição de um grupo
        ///     importante na determinação das posturas dos órgãos dirigentes com relação às suas atribuições. A certificação de
        ///     metodologias que nos auxiliam a lidar com o início da atividade geral de formação de atitudes faz parte de um
        ///     processo de gerenciamento do orçamento setorial.
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