using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogoJogosAPI.InputModel;
using CatalogoJogosAPI.ViewModel;
using CatalogoJogosAPI.Services;
using System.ComponentModel.DataAnnotations;
using CatalogoJogosAPI.Exceptions;

namespace CatalogoJogosAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Busca todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica a página consultada</param>
        /// <param name="quantidade">Jogos por página</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        [HttpGet]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery, Range(1,int.MaxValue)] int pagina = 1, [FromQuery, Range(1,50)] int quantidade = 5) 
        {            
            var jogos = await _jogoService.Obter(pagina, quantidade);
            if (jogos.Count == 0)
                return NoContent();

            return Ok(jogos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid id) 
        {
            var jogo = await _jogoService.Obter(id);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> CriarJogo([FromBody]JogoInputModel jogoparam) 
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoparam);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity(ex.Message);
            }            
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid id,[FromBody] JogoInputModel jogo) 
        {
            try
            {
                await _jogoService.Atualizar(id, jogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }            
        }

        [HttpPatch("{id:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid id,[FromRoute] double preco) 
        {
            try
            {
                await _jogoService.Atualizar(id, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid id) 
        {
            try
            {
                await _jogoService.Remover(id);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
