using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
/// <summary>
/// Solicita metodos para cando un usuario marche da compañia poder bloquear a conta ou cambiar detalles.
/// </summary>
    public interface IRepositorioUsuario : IRepositorio<AplicacionUsuario>
    {
        void BloquearUsuario(string IdUsuario); //id e un string e non un integer porque na base de datos de ASP se garda como unha query e non un numero enteiro
        void DesbloquearUsuario(string IdUsuario);
    }
}
