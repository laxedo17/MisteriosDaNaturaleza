using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos.ViewModelos
{
    public class PedidoViewModel
    {
        public CabeceiraPedido CabeceiraPedido { get; set; }
        public IEnumerable<DetallesPedido> DetallesPedido { get; set; }
    }
}
