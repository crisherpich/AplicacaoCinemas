using System;
using System.ComponentModel.DataAnnotations;

namespace AplicacaoCinemas.WebApi.Models
{
    public class NovoFilmeInputModel
    {
        [Required]
        public string Titulo { get; set; }
        public string Sinopse { get; set; }
        [Required] 
        public int Duracao { get; set; }
    }
}