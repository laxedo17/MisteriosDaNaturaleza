using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class Destino
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo de destino")]
        public string Nome { get; set; }

        [Required]
        public double Precio { get; set; }

        //Descripcion longa da viaxe
        [Display(Name = "Descripcion")]
        public string DescLonga { get; set; }

        [DataType(DataType.ImageUrl)]//arquivo tipo imaxe
        [Display(Name = "Imaxe")]
        public string ImaxeUrl { get; set; } //usamos unha URL porque a imaxe estara gardada no servidor

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required]
        public int FrecuenciaId { get; set; }

        [ForeignKey("FrecuenciaId")]
        public Frecuencia Frecuencia { get; set; }
    }
}
