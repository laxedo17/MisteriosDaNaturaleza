using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos.ViewModelos
{
    public class DestinoVM
    {
        public Destino Destino { get; set; }

        public IEnumerable<SelectListItem> ListaCategoria { get; set; }
        public IEnumerable<SelectListItem> ListaFrecuencia { get; set; }
    }
}
