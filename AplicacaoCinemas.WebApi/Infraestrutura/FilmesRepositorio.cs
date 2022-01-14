﻿using System;
using System.Collections.Generic;
using System.Linq;
using AplicacaoCinemas.WebApi.Dominio;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoCinemas.WebApi.Infraestrutura
{
    public sealed class FilmesRepositorio
    {
        private readonly CinemasDbContext _dbContext;

        public FilmesRepositorio(CinemasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InserirAsync(Filme novoFilme, CancellationToken cancellationToken = default)
        {
            await _dbContext.Filmes.AddAsync(novoFilme, cancellationToken);
        }
        public void Alterar(Filme filme)
        {
            // Nada a fazer EF CORE fazer o Tracking da Entidade quando recuperamos a mesma
        }

        public async Task<Filme> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext
                         .Filmes
                         .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Filme>> ListarFilmesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext
                         .Filmes
                         .ToListAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}