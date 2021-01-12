using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lugar de viaxe")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Orden de mostra")]
        public int OrdeDeExposicion { get; set; }
    }
}
