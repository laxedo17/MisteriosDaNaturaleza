using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class Frecuencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Nome da Frecuencia")]
        public string Nome { get; set; }

        [Required]
        public int ContaFrecuencia { get; set; }
    }
}
