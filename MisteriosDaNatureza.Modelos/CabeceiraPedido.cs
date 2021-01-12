using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class CabeceiraPedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Poblacion { get; set; }

        [Required]
        public string CP { get; set; }

        public DateTime DataPedido { get; set; }

        //indica status/condicion do pedido
        public string Estado { get; set; } 

        public string Comentarios { get; set; }

        //conta cantidade de destinos que solicita cliente
        public int ContaDestinos { get; set; }
    }
}
