using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface IRepositorioCabeceiraPedido : IRepositorio<CabeceiraPedido>
    {
        void CambiarEstadoDePedido(int idCabeceiraPedido, string estado);
    }
}
