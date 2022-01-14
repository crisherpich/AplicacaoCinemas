using System;
using AplicacaoCinemas.WebApi.Dominio;
using AplicacaoCinemas.WebApi.Infraestrutura;
using AplicacaoCinemas.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoCinemas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        private readonly SessoesRepositorio _sessoesRepositorio;
        private readonly FilmesRepositorio _filmesRepositorio;
        private readonly ILogger<SessoesController> _logger;

        public SessoesController(SessoesRepositorio sessoesRepositorio,
                                 FilmesRepositorio filmesRepositorio,
                                 ILogger<SessoesController> logger)
        {
            _logger = logger;
            _sessoesRepositorio = sessoesRepositorio;
            _filmesRepositorio = filmesRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirAsync([FromBody] NovaSessaoInputModel inputModel, CancellationToken cancellationToken)
        {
            var sessao = Sessao.Criar(inputModel.Dia, inputModel.HoraInicio, inputModel.MinutoInicio,
                inputModel.QuantidadeLugares, inputModel.Preco, inputModel.IdFilme);
            if (sessao.IsFailure)
            {
                _logger.LogError(sessao.Error);
                return BadRequest(sessao.Error);
            }
                
            var filme = await _filmesRepositorio.RecuperarPorIdAsync(inputModel.IdFilme, cancellationToken);
            if (filme == null)
            {
                var erro = "Filme não foi localizado";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            await _sessoesRepositorio.InserirAsync(sessao.Value, cancellationToken);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Sessão {sessao} criada", sessao.Value.Id);

            return Ok(sessao.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarAsync(Guid id,[FromBody] AlterarSessaoInputModel inputModel, CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);
            if (sessao == null)
            {
                var erro = "Sessão não foi localizada";
                _logger.LogError(erro);
                return BadRequest(erro);
            }

            var filme = await _filmesRepositorio.RecuperarPorIdAsync(inputModel.IdFilme, cancellationToken);
            if (filme == null)
            {
                var erro = "Novo filme não foi localizado";
                _logger.LogError(erro);
                return BadRequest(erro);
            }

            if (inputModel.IdFilme.ToString() == null)
            {
                var erro = "O filme da Sessão deve ser preenchido";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            if (inputModel.QuantidadeLugares <= 0)
            {
                var erro = "A quantidade de lugares não pode ser igual a zero";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            if (inputModel.Preco <= 0)
            {
                var erro = "O preço não pode ser igual a zero";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            if (inputModel.HoraInicio < 0 | inputModel.HoraInicio > 23)
            {
                var erro = "Hora de início inválida";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            if (inputModel.MinutoInicio < 0 | inputModel.MinutoInicio > 59)
            {
                var erro = "Hora de início inválida";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                

            sessao.Dia = inputModel.Dia;
            sessao.HoraInicio = inputModel.HoraInicio;
            sessao.IdFilme = inputModel.IdFilme;
            sessao.Preco = inputModel.Preco;
            sessao.MinutoInicio = inputModel.MinutoInicio;

            sessao.QuantidadeLugaresLivres = inputModel.QuantidadeLugares - (sessao.QuantidadeLugares - sessao.QuantidadeLugaresLivres);
            if (sessao.QuantidadeLugaresLivres < 0)
            {
                var erro = "Quantidade inválida, ingressos já vendidos";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            sessao.QuantidadeLugares = inputModel.QuantidadeLugares;
            sessao.AtualizarHashConcorrencia();
            
            _sessoesRepositorio.Alterar(sessao);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Sessão {sessao} atualizada", sessao.Id);

            return Ok(sessao);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            _logger.LogInformation("Recuperado o ID {sessao} ",id);

            return Ok(sessao);
        }

        [HttpGet]
        public async Task<IActionResult> ListarSessoes(CancellationToken cancellationToken)
        {
            return Ok(await _sessoesRepositorio.ListarSessoesAsync(cancellationToken));
        }

        [HttpGet("pesquisafilme/{id},{dia}")]
        public async Task<IActionResult> ListarSessoesPorFilmeDiaAsync(Guid id, DateTime dia, CancellationToken cancellationToken)
        {
            var sessoes = await _sessoesRepositorio.RecuperarPorFilmeDiaAsync(id, dia, cancellationToken);

            _logger.LogInformation("Listadas as sessões do fime {fime} do dia {dia} ", id, dia.ToShortDateString());

            return Ok(sessoes);
        }

        [HttpPost("compraringresso/{id},{quantidade}")]
        public async Task<IActionResult> ComprarIngresso(Guid id, int quantidade, CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);
            if (sessao == null)
            {
                var erro = "Sessão não foi localizada";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            sessao.QuantidadeLugaresLivres -= quantidade;
            if (sessao.QuantidadeLugaresLivres < 0)
            {
                var erro = "Não existem mais ingressos disponíveis";
                _logger.LogError(erro);
                return BadRequest(erro);
            }

            sessao.AtualizarHashConcorrencia();

            _sessoesRepositorio.Alterar(sessao);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("{quantidade} ingressos vendidos para a sessão {sessao} ", quantidade, sessao.Id);

            return Ok(sessao);
        }

    }
}