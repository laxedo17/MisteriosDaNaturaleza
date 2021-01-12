using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos.ViewModelos
{
    public class CarroViewModel
    {
        public IList<Destino> ListaDestinos { get; set; }
        public CabeceiraPedido CabeceiraPedido { get; set; }

        //[EnumDataType(typeof(Viaxantes))]
        //public Viaxantes Viaxantes { get; set; } //para elexir a cantidade de viaxantes dunha lista, solo disponible neste ViewModel

    }

    //public enum Viaxantes
    //{
    //    Unha_persona = 1,
    //    Duas_personas = 2,
    //    Tres_personas = 3,
    //    Catro_persona = 4,
    //    Cinco_personas = 5,
    //    Seis_personas = 6,
    //    Sete_personas = 7,
    //    Oito_personas = 8,
    //    Nove_personas = 9,
    //    Dez_personas = 10
    //}
}
