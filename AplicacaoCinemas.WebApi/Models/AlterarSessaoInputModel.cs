using System;
using System.ComponentModel.DataAnnotations;

namespace AplicacaoCinemas.WebApi.Models
{
    public sealed class AlterarSessaoInputModel
    {
        public DateTime Dia { get; set; }
        public int HoraInicio { get; set; }
        public int MinutoInicio { get; set; }
        public int QuantidadeLugares { get; set; }
        public float Preco { get; set; }
        public Guid IdFilme { get; set; }

    }
}