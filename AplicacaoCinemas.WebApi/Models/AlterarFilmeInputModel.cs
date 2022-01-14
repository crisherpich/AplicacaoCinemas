using System;
using System.ComponentModel.DataAnnotations;

namespace AplicacaoCinemas.WebApi.Models
{
    public sealed class AlterarFilmeInputModel
    {
        public string Titulo { get; set; }
        public string Sinopse { get; set; }
        public int Duracao { get; set; }

    }
}