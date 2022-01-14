using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace AplicacaoCinemas.WebApi.Dominio
{
    public sealed class Filme
    {
        public Filme(Guid id, string titulo, string sinopse, int duracao)
        {
            Id = id;
            Titulo = titulo;
            Sinopse = sinopse;
            Duracao = duracao;
        }

        public Guid Id { get; }
        public string Titulo { set; get; }
        public string Sinopse { set; get; }
        public int Duracao { set;  get; }


        public static Result<Filme> Criar(string titulo, string sinopse, int duracao)
        {
            if (string.IsNullOrEmpty(titulo))
                return Result.Failure<Filme>("Título do filme deve ser preenchido");
            if (duracao <= 0)
                return Result.Failure<Filme>("Tempo de duração do filme não pode ser igual a zero");
            return new Filme(Guid.NewGuid(), titulo, sinopse, duracao);
        }
    }
}