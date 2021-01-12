using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioCabeceiraPedido : Repositorio<CabeceiraPedido> , IRepositorioCabeceiraPedido
    {
        private readonly ApplicationDbContext _db;

        public RepositorioCabeceiraPedido(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Metodo para que o admin revise o estado dun pedido
        /// </summary>
        /// <param name="idCabeceiraPedido"></param>
        /// <param name="estado"></param>
        public void CambiarEstadoDePedido(int idCabeceiraPedido, string estado)
        {
            var pedidoDaBd = _db.CabeceiraPedido.FirstOrDefault(o => o.Id == idCabeceiraPedido);
            pedidoDaBd.Estado = estado; //o estado do pedido sera igual o novo estado
            _db.SaveChanges();
        }
    }
}
