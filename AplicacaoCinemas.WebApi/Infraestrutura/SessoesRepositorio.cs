using System;
using System.Collections.Generic;
using System.Linq;
using AplicacaoCinemas.WebApi.Dominio;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace AplicacaoCinemas.WebApi.Infraestrutura
{
    public sealed class SessoesRepositorio
    {
        private readonly CinemasDbContext _dbContext;

        public SessoesRepositorio(CinemasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InserirAsync(Sessao novaSessao, CancellationToken cancellationToken = default)
        {
            await _dbContext.Sessoes.AddAsync(novaSessao, cancellationToken);
        }
        public void Alterar(Sessao sessao)
        {
            // Nada a fazer EF CORE fazer o Tracking da Entidade quando recuperamos a mesma
        }

        public async Task <Sessao> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext
                  .Sessoes
                  .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Sessao>> ListarSessoesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext
                         .Sessoes
                         .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Sessao>> RecuperarPorFilmeDiaAsync(Guid idFilme, DateTime dia, CancellationToken cancellationToken = default)
        {
            return await _dbContext
                         .Sessoes
                         .Where(c => c.IdFilme == idFilme && c.Dia == dia)
                         .ToListAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}