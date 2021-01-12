using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    public class AplicacionUsuario : IdentityUser //IdentityUser na base de datos e a taboa de usuarios de ASP.NET
    {
        [Required]
        public string Nome { get; set; }

        public string Rua { get; set; }
        public string Poboacion { get; set; }
        public string Provincia { get; set; }
        public string CodigoPostal { get; set; }
    }
}
