using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class DetallesPedido
    {
        public int Id { get; set; }

        [Required]
        public int IdCabeceiraPedido { get; set; }

        [ForeignKey("IdCabeceiraPedido")]
        public CabeceiraPedido CabeceiraPedido { get; set; }

        [Required]
        public int DestinoId { get; set; }

        [ForeignKey("IdDestino")]
        public Destino Destino { get; set; }

        [Required]
        public string NomeServicio { get; set; }

        [Required]
        public double Precio { get; set; } //importante xk anque a entidade Destino xa ten un atributo Precio se cambia o precio dun articulo pero xa se vendera con outro precio, o que se lle cobra ao cliente non debe cambiar
    }
}
