using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class RepositorioUsuario : Repositorio<AplicacionUsuario> , IRepositorioUsuario
    {
        private readonly ApplicationDbContext _db;

        public RepositorioUsuario(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void BloquearUsuario(string IdUsuario)
        {
            var usuarioDeDb = _db.AplicacionUsuario.FirstOrDefault(u => u.Id == IdUsuario);//obtemos a id de usuario da base de datos
            usuarioDeDb.LockoutEnd = DateTime.Now.AddYears(1000); //bloqueamos o usuario durante 1000 anos usando un metodo para unha taboa da BD de ASP Net users
            _db.SaveChanges();
        }

        public void DesbloquearUsuario(string IdUsuario)
        {
            var usuarioDeDb = _db.AplicacionUsuario.FirstOrDefault(u => u.Id == IdUsuario);//obtemos a id de usuario da base de datos
            usuarioDeDb.LockoutEnd = DateTime.Now; //desbloqueamos o usuario agora mesmo, no momento actual
            _db.SaveChanges();
        }
    }
}
