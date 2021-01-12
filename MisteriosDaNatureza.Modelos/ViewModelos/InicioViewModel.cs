using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos.ViewModelos
{
    public class InicioViewModel
    {
        public IEnumerable<Categoria> ListaCategorias { get; set; }
        public IEnumerable<Destino> ListaDestinos { get; set; }
    }
}
