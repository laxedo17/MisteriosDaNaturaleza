using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioFrecuencia : Repositorio<Frecuencia>, IRepositorioFrecuencia
    {
        private readonly ApplicationDbContext _db;

        public RepositorioFrecuencia(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Frecuencia frecuencia)
        {
            var obxetoDaBaseDeDatos = _db.Frecuencia.FirstOrDefault(s => s.Id == frecuencia.Id);

            obxetoDaBaseDeDatos.Nome = frecuencia.Nome;
            obxetoDaBaseDeDatos.ContaFrecuencia = frecuencia.ContaFrecuencia;

            _db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetListaFrecuenciasMenuDesplegable()
        {
            return _db.Frecuencia.Select(i => new SelectListItem()
            {
                Text = i.Nome,
                Value = i.Id.ToString()
            });
        }
    }
}
