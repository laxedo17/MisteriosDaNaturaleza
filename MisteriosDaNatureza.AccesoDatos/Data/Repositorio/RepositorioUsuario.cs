using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioCategoria : Repositorio<Categoria> , IRepositorioCategoria
    {
        private readonly ApplicationDbContext _db;

        public RepositorioCategoria(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Categoria categoria)
        {
            var obxetoDeBd = _db.Categoria.FirstOrDefault((System.Linq.Expressions.Expression<Func<Categoria, bool>>)(s => s.Id == categoria.Id));

            obxetoDeBd.Nome = categoria.Nome;
            obxetoDeBd.OrdeDeExposicion = categoria.OrdeDeExposicion;

            _db.SaveChanges();//gardamos os cambios
        }

        public IEnumerable<SelectListItem> GetListaCategoriasMenuDesplegable()
        {
            return _db.Categoria.Select((System.Linq.Expressions.Expression<Func<Categoria, SelectListItem>>)(i => new SelectListItem()
            {
                Text = i.Nome,
                Value = i.Id.ToString()
            }));
        }
    }
}
