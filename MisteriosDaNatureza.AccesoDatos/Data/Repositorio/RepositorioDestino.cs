using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioDestino : Repositorio<Destino> , IRepositorioDestino
    {
        private readonly ApplicationDbContext _db;

        public RepositorioDestino(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Destino destino)
        {
            var obxetoDeBd = _db.Destino.FirstOrDefault((System.Linq.Expressions.Expression<Func<Destino, bool>>)(s => s.Id == destino.Id));

            obxetoDeBd.Nome = destino.Nome;
            obxetoDeBd.DescLonga = destino.DescLonga;
            obxetoDeBd.Precio = destino.Precio;
            obxetoDeBd.ImaxeUrl = destino.ImaxeUrl;
            obxetoDeBd.CategoriaId = destino.CategoriaId;
            obxetoDeBd.FrecuenciaId = destino.FrecuenciaId;

            _db.SaveChanges();//gardamos os cambios
        }
    }
}
