using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CSharpFunctionalExtensions;
using System.Security.Cryptography;
using System.Text;

namespace AplicacaoCinemas.WebApi.Dominio
{
    public sealed class Sessao
    {
        private string _hashConcorrencia;
        public Sessao(Guid id, DateTime dia, int horaInicio, 
                      int minutoInicio, int quantidadeLugares, 
                      double preco, Guid idFilme,
                      string hashConcorrencia)
        {
            Id = id;
            Dia = dia;
            HoraInicio = horaInicio;
            MinutoInicio = minutoInicio;
            QuantidadeLugares = quantidadeLugares;
            QuantidadeLugaresLivres = quantidadeLugares;
            Preco = preco;
            IdFilme = idFilme;
            _hashConcorrencia = hashConcorrencia;
        }

        public Guid Id { get; } 
        public DateTime Dia { set; get; }
        public int HoraInicio { set; get; }
        public int MinutoInicio { set; get; }
        public int QuantidadeLugares { set; get; }
        public int QuantidadeLugaresLivres { set; get; }
        public double Preco { set; get; }
        public Guid IdFilme { set; get; }


        public static Result<Sessao > Criar(DateTime dia, int horaInicio, int minutoInicio, int quantidadeLugares, double preco, Guid idFilme)
        {
            if (idFilme.ToString() == null)
                return Result.Failure<Sessao>("O filme da Sessão deve ser preenchido");
            if (quantidadeLugares <= 0)
                return Result.Failure<Sessao>("A quantidade de lugares não pode ser igual a zero");
            if (preco <= 0)
                return Result.Failure<Sessao>("O preço não pode ser igual a zero");
            if (horaInicio < 0 | horaInicio > 23)
                return Result.Failure<Sessao>("Hora de início inválida");
            if (minutoInicio < 0 | minutoInicio > 59)
                return Result.Failure<Sessao>("Hora de início inválida");

            var sessao = new Sessao(Guid.NewGuid(), dia, horaInicio, minutoInicio, quantidadeLugares, preco, idFilme, "");
            sessao.AtualizarHashConcorrencia();
            return sessao;
        }

        public void AtualizarHashConcorrencia()
        {
            using var hash = MD5.Create();
            var data = hash.ComputeHash(
                Encoding.UTF8.GetBytes(
                    $"{Id}{Dia}{HoraInicio}{QuantidadeLugares}{QuantidadeLugaresLivres}{Preco}{IdFilme}"));
            var sBuilder = new StringBuilder();
            foreach (var tbyte in data)
                sBuilder.Append(tbyte.ToString("x2"));
            _hashConcorrencia = sBuilder.ToString();
        }
    }
}