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
    public class FilmesController : ControllerBase
    {
        private readonly FilmesRepositorio _filmesRepositorio;
        private readonly ILogger<FilmesController> _logger;

        public FilmesController(ILogger<FilmesController> logger,
                                FilmesRepositorio filmesRepositorio)
        {
            _logger = logger;
            _filmesRepositorio = filmesRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirAsync([FromBody] NovoFilmeInputModel inputModel, CancellationToken cancellationToken)
        {
            var filme = Filme.Criar(inputModel.Titulo, inputModel.Sinopse, inputModel.Duracao);
            if (filme.IsFailure)
            {
                _logger.LogError(filme.Error);
                return BadRequest(filme.Error);
            }
                
            await _filmesRepositorio.InserirAsync(filme.Value, cancellationToken);
            await _filmesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Filme {filme} criado", filme.Value.Id);

            return Ok(filme.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filme = await _filmesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            _logger.LogInformation("Recuperado o ID {filme} ", id);

            return Ok(filme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAsync(Guid id, [FromBody] AlterarFilmeInputModel filmeInputModel, CancellationToken cancellationToken)
        {
            var filme = await _filmesRepositorio.RecuperarPorIdAsync(id, cancellationToken);
            if (filme == null)
            {
                var erro = "Filme não localizado";
                _logger.LogError(erro);
                return NotFound(erro);
            }

            if (string.IsNullOrEmpty(filmeInputModel.Titulo))
            {
                var erro = "Título do filme deve ser preenchido";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                
            if (filmeInputModel.Duracao <= 0)
            {
                var erro = "Tempo de duração do filme não pode ser igual a zero";
                _logger.LogError(erro);
                return BadRequest(erro);
            }
                

            filme.Duracao = filmeInputModel.Duracao;
            filme.Sinopse = filmeInputModel.Sinopse;
            filme.Titulo = filmeInputModel.Titulo;

            _filmesRepositorio.Alterar(filme);
            await _filmesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Filme {filme} atualizado", filme.Id);

            return Ok(filme);
        }

        [HttpGet]
        public async Task<IActionResult> ListarFilmes(CancellationToken cancellationToken)
        {
            return Ok(await _filmesRepositorio.ListarFilmesAsync(cancellationToken));
        }

    }
}