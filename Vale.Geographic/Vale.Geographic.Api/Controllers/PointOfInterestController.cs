using Vale.Geographic.Application.Services;
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
    /// Controller to PointOfInterest
    /// </summary>
    [Route("api/PointOfInterest")]
    [Authorize]
    public class PointOfInterestController : Controller
    {
        private IPointOfInterestAppService PointOfInterestAppService { get; }
        /// <summary>
        /// Constructor to PointOfInterest Controller 
        /// </summary>
        /// <param name="pointOfInterestAppService">app service</param>
        public PointOfInterestController(IPointOfInterestAppService pointOfInterestAppService)
        {
            this.PointOfInterestAppService = pointOfInterestAppService;
        }


        /// <summary>
        ///     Delete a PointOfInterest by Id
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. O incentivo ao avanço tecnológico, assim
        ///     como a execução dos pontos do programa nos obriga à análise dos modos de operação convencionais. Por outro lado, a
        ///     complexidade dos estudos efetuados agrega valor ao estabelecimento de todos os recursos funcionais envolvidos.
        ///     Assim mesmo, o aumento do diálogo entre os diferentes setores produtivos exige a precisão e a definição do impacto
        ///     na agilidade decisória.
        /// </remarks>
        /// <param name="id">PointOfInterest Id</param>
        /// <returns>No content</returns>
        /// <response code="204">PointOfInterest deleted!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpDelete("{id:GUID}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                PointOfInterestAppService.Delete(id);

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
        ///     Filter PointOfInterests
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. No entanto, não podemos esquecer que o
        ///     consenso sobre a necessidade de qualificação oferece uma interessante oportunidade para verificação do sistema de
        ///     participação geral. Do mesmo modo, o novo modelo estrutural aqui preconizado garante a contribuição de um grupo
        ///     importante na determinação das posturas dos órgãos dirigentes com relação às suas atribuições. A certificação de
        ///     metodologias que nos auxiliam a lidar com o início da atividade geral de formação de atitudes faz parte de um
        ///     processo de gerenciamento do orçamento setorial.
        /// </remarks>
        /// <param name="active">Retrive all points are active or not</param>
        /// <param name="Id"></param>
        /// <param name="categoryId"></param>
        /// <param name="areaId"></param>
        /// <param name="request">Filter parameters</param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="altitude"></param>
        /// <param name="radiusDistance"></param>
        /// <returns>PointOfInterests list have been solicited</returns>
        /// <response code="200">PointOfInterest list!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PointOfInterestDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Get([FromQuery] bool? active, Guid? Id, Guid? categoryId, Guid? areaId,  double? longitude, double? latitude, double? altitude, int? radiusDistance, FilterDto request)
        {
            var total = 0;

            var result = PointOfInterestAppService.Get(active, Id, categoryId, areaId, longitude, latitude, altitude, radiusDistance, request, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }


        /// <summary>
        /// Get all PointOfInterest with paging, filtering and sorting.
        /// </summary>
        /// <remarks>List can be filtered, sorted and paged based on parameters passed. If no paging is required, pass 'needPaging=false'.
        ///  Filter argument can be used as {Key}:=:{Value}. Ex. FirstName:=:Vijay,LastName:=:Patel or FirstName:like:vij
        /// </remarks>
        /// <param name="parameters"><see cref="ResourceParameters"/>
        /// Filter/Sort based on FirstName, LastName and Type
        /// </param>
        /// <returns>List of <see cref="PointOfInterestDto"/> with Total Number of records.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<PointOfInterestDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll([FromQuery]FilterDto parameters)
        {

            var total = 0;

            var result = PointOfInterestAppService.GetAll(parameters, out total);
            Response.Headers.Add("X-Total-Count", total.ToString());

            return Ok(result);
        }

        /// <summary>
        ///     Get PointOfInterest by Id
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Caros amigos, a estrutura atual da
        ///     organização possibilita uma melhor visão global dos procedimentos normalmente adotados. As experiências acumuladas
        ///     demonstram que a contínua expansão de nossa atividade causa impacto indireto na reavaliação das diversas correntes
        ///     de pensamento. Acima de tudo, é fundamental ressaltar que o surgimento do comércio virtual auxilia a preparação e a
        ///     composição dos índices pretendidos.
        /// </remarks>
        /// <param name="id">PointOfInterest Id</param>
        /// <returns>PointOfInterest that has been solicited</returns>
        /// <response code="200">PointOfInterest!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("{id:GUID}")]
        [ProducesResponseType(typeof(PointOfInterestDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetById(Guid id)
        {
            return Ok(PointOfInterestAppService.GetById(id));
        }

        /// <summary>
        ///     Create a new PointOfInterest
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. Nunca é demais lembrar o peso e o
        ///     significado destes problemas, uma vez que a competitividade nas transações comerciais prepara-nos para enfrentar
        ///     situações atípicas decorrentes das diretrizes de desenvolvimento para o futuro. A prática cotidiana prova que a
        ///     determinação clara de objetivos maximiza as possibilidades por conta dos níveis de motivação departamental. Não
        ///     obstante, a constante divulgação das informações desafia a capacidade de equalização da gestão inovadora da qual
        ///     fazemos parte.
        /// </remarks>
        /// <param name="value">PointOfInterest data</param>
        /// <returns>PointOfInterest who has been created</returns>
        /// <response code="201">PointOfInterest created!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(PointOfInterestDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Post([FromBody] PointOfInterestDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = PointOfInterestAppService.Insert(value);
            return Created("", response);
        }


        [HttpPost("LargeScale")]
        [ProducesResponseType(typeof(CollectionPointOfInterestDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult PostLargeScale([FromBody] CollectionPointOfInterestDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = PointOfInterestAppService.Insert(obj);
            return Created("", response);
        }

        /// <summary>
        ///     Update a PointOfInterest
        /// </summary>
        /// <remarks>
        ///     Faça aqui uma decrição mais detalhada do que esse metodo irá fazer. É claro que a necessidade de renovação
        ///     processual cumpre um papel essencial na formulação do fluxo de informações. Podemos já vislumbrar o modo pelo qual
        ///     o fenômeno da Internet ainda não demonstrou convincentemente que vai participar na mudança de alternativas às
        ///     soluções ortodoxas. Percebemos, cada vez mais, que o comprometimento entre as equipes estimula a padronização do
        ///     sistema de formação de quadros que corresponde às necessidades.
        /// </remarks>
        /// <param name="id">PointOfInterest Id</param>
        /// <param name="value">PointOfInterest data</param>
        /// <returns>PointOfInterest who has been updated</returns>
        /// <response code="200">PointOfInterest updated!</response>
        /// <response code="400">PointOfInterest has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("{id:GUID}")]
        [ProducesResponseType(typeof(PointOfInterestDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put(Guid id, [FromBody] PointOfInterestDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(PointOfInterestAppService.Update(id, value));
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