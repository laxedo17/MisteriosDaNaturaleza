using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioDetallesPedido : Repositorio<DetallesPedido> , IRepositorioDetallesPedido
    {
        private readonly ApplicationDbContext _db;

        public RepositorioDetallesPedido(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
