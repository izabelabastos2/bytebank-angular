using Vale.Geographic.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Api.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GeoJSON.Net.Geometry;
using System;

namespace Vale.Geographic.Api.Controllers
{
    /// <summary>
    /// Controller to Area
    /// </summary>
    [Route("api/Area")]
    [Authorize]
    public class AreaController : Controller
    {
        private IAreaAppService AreaAppService { get; }
        /// <summary>
        /// Constructor to Area Controller 
        /// </summary>
        /// <param name="areaAppService">app service</param>
        public AreaController(IAreaAppService areaAppService)
        {
            this.AreaAppService = areaAppService;
        }


        /// <summary>
        ///     Delete a Area by Id
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. O incentivo ao avanço tecnológico, assim
        ///     como a execução dos pontos do programa nos obriga à análise dos modos de operação convencionais. Por outro lado, a
        ///     complexidade dos estudos efetuados agrega valor ao estabelecimento de todos os recursos funcionais envolvidos.
        ///     Assim mesmo, o aumento do diálogo entre os diferentes setores produtivos exige a precisão e a definição do impacto
        ///     na agilidade decisória.
        /// </remarks>
        /// <param name="id">Area Id</param>
        /// <returns>No content</returns>
        /// <response code="204">Area deleted!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        // [ClaimRequirement(RoleEnum.GestorDeEstoque)]
        public IActionResult Delete(Guid id)
        {
            AreaAppService.Delete(id);
            return NoContent();
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
        /// <param name="active">Retrive all perssons are active or not</param>
        /// <param name="request">Filter parameters</param>
        /// <returns>Areas list have been solicited</returns>
        /// <response code="200">Area list!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AreaDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] bool? active, Guid categoryId, Point location, [FromQuery] FilterDto request)
        {
            var total = 0;

            var result = AreaAppService.Get(active, categoryId, location, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


        /// <summary>
        /// Get all Area with paging, filtering and sorting.
        /// </summary>
        /// <remarks>List can be filtered, sorted and paged based on parameters passed. If no paging is required, pass 'needPaging=false'.
        ///  Filter argument can be used as {Key}:=:{Value}. Ex. FirstName:=:Vijay,LastName:=:Patel or FirstName:like:vij
        /// </remarks>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="AreaDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<AreaDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = AreaAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get Area by Id
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Caros amigos, a estrutura atual da
        ///     organização possibilita uma melhor visão global dos procedimentos normalmente adotados. As experiências acumuladas
        ///     demonstram que a contínua expansão de nossa atividade causa impacto indireto na reavaliação das diversas correntes
        ///     de pensamento. Acima de tudo, é fundamental ressaltar que o surgimento do comércio virtual auxilia a preparação e a
        ///     composição dos índices pretendidos.
        /// </remarks>
        /// <param name="id">Area Id</param>
        /// <returns>Area that has been solicited</returns>
        /// <response code="200">Area!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AreaDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            return Ok(AreaAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new Area
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Nunca é demais lembrar o peso e o
        ///     significado destes problemas, uma vez que a competitividade nas transações comerciais prepara-nos para enfrentar
        ///     situações atípicas decorrentes das diretrizes de desenvolvimento para o futuro. A prática cotidiana prova que a
        ///     determinação clara de objetivos maximiza as possibilidades por conta dos níveis de motivação departamental. Não
        ///     obstante, a constante divulgação das informações desafia a capacidade de equalização da gestão inovadora da qual
        ///     fazemos parte.
        /// </remarks>
        /// <param name="value">Area data</param>
        /// <returns>Area who has been created</returns>
        /// <response code="201">Area created!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(AreaDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] AreaDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = AreaAppService.Insert(value);
            return Created("", response);
        }


        [HttpPost("featureColletion")]
        [ProducesResponseType(typeof(AreaDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult PostFeatureCollection([FromBody] GeoJsonDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = AreaAppService.Insert(obj);
            return Created("", response);
        }

        /// <summary>
        ///     Update a Area
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. É claro que a necessidade de renovação
        ///     processual cumpre um papel essencial na formulação do fluxo de informações. Podemos já vislumbrar o modo pelo qual
        ///     o fenômeno da Internet ainda não demonstrou convincentemente que vai participar na mudança de alternativas às
        ///     soluções ortodoxas. Percebemos, cada vez mais, que o comprometimento entre as equipes estimula a padronização do
        ///     sistema de formação de quadros que corresponde às necessidades.
        /// </remarks>
        /// <param name="id">Area Id</param>
        /// <param name="value">Area data</param>
        /// <returns>Area who has been updated</returns>
        /// <response code="200">Area updated!</response>
        /// <response code="400">Area has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AreaDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] AreaDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(AreaAppService.Update(id, value));
        }
    }
}