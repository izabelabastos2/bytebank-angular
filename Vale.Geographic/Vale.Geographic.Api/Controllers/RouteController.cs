﻿using Vale.Geographic.Application.Services;
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
    /// Controller to Route
    /// </summary>
    [Route("api/Route")]
    [Authorize]
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
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. O incentivo ao avanço tecnológico, assim
        ///     como a execução dos pontos do programa nos obriga à análise dos modos de operação convencionais. Por outro lado, a
        ///     complexidade dos estudos efetuados agrega valor ao estabelecimento de todos os recursos funcionais envolvidos.
        ///     Assim mesmo, o aumento do diálogo entre os diferentes setores produtivos exige a precisão e a definição do impacto
        ///     na agilidade decisória.
        /// </remarks>
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
            var lastUpdatedBy = this.HttpContext.User.Identity.Name;

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
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. No entanto, não podemos esquecer que o
        ///     consenso sobre a necessidade de qualificação oferece uma interessante oportunidade para verificação do sistema de
        ///     participação geral. Do mesmo modo, o novo modelo estrutural aqui preconizado garante a contribuição de um grupo
        ///     importante na determinação das posturas dos órgãos dirigentes com relação às suas atribuições. A certificação de
        ///     metodologias que nos auxiliam a lidar com o início da atividade geral de formação de atitudes faz parte de um
        ///     processo de gerenciamento do orçamento setorial.
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
        /// <remarks>List can be filtered, sorted and paged based on parameters passed. If no paging is required, pass 'needPaging=false'.
        ///  Filter argument can be used as {Key}:=:{Value}. Ex. FirstName:=:Vijay,LastName:=:Patel or FirstName:like:vij
        /// </remarks>
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
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Caros amigos, a estrutura atual da
        ///     organização possibilita uma melhor visão global dos procedimentos normalmente adotados. As experiências acumuladas
        ///     demonstram que a contínua expansão de nossa atividade causa impacto indireto na reavaliação das diversas correntes
        ///     de pensamento. Acima de tudo, é fundamental ressaltar que o surgimento do comércio virtual auxilia a preparação e a
        ///     composição dos índices pretendidos.
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
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Nunca é demais lembrar o peso e o
        ///     significado destes problemas, uma vez que a competitividade nas transações comerciais prepara-nos para enfrentar
        ///     situações atípicas decorrentes das diretrizes de desenvolvimento para o futuro. A prática cotidiana prova que a
        ///     determinação clara de objetivos maximiza as possibilidades por conta dos níveis de motivação departamental. Não
        ///     obstante, a constante divulgação das informações desafia a capacidade de equalização da gestão inovadora da qual
        ///     fazemos parte.
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

            value.CreatedBy = this.HttpContext.User.Identity.Name;

            var response = RouteAppService.Insert(value);
            return Created("", response);
        }


        /// <summary>
        ///     Update a Route
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. É claro que a necessidade de renovação
        ///     processual cumpre um papel essencial na formulação do fluxo de informações. Podemos já vislumbrar o modo pelo qual
        ///     o fenômeno da Internet ainda não demonstrou convincentemente que vai participar na mudança de alternativas às
        ///     soluções ortodoxas. Percebemos, cada vez mais, que o comprometimento entre as equipes estimula a padronização do
        ///     sistema de formação de quadros que corresponde às necessidades.
        /// </remarks>
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

             value.LastUpdatedBy = this.HttpContext.User.Identity.Name;

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