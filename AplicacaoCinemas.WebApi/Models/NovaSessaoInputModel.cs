using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace AplicacaoCinemas.WebApi.Models
{
    public class NovaSessaoInputModel
    {
        [Required]
        public DateTime Dia { get; set; }
        [Required]
        public int HoraInicio { get; set; }
        [Required]
        public int MinutoInicio { get; set; }
        [Required]
        public int QuantidadeLugares { get; set; }
        [Required]
        public double Preco { get; set; }
        [Required]
        public Guid IdFilme { get; set; }
    }
}