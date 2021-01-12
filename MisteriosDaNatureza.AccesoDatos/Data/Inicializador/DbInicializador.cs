using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MisteriosDaNaturaleza.Modelos;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Inicializador
{
    /// <summary>
    /// Inicializa unha Base de Datos desde cero, con Admins e outros usuarios.
    /// </summary>
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _xestorUsuarios;
        private readonly RoleManager<IdentityRole> _xestorRoles;

        public DbInicializador(ApplicationDbContext db, UserManager<IdentityUser> xestorUsuarios, RoleManager<IdentityRole> xestorRoles)
        {
            _db = db;
            _xestorUsuarios = xestorUsuarios;
            _xestorRoles = xestorRoles;
        }

        /// <summary>
        /// Inicializa a Base de Datos con usuarios creados cando se publica a Web e non temos Admins e demais
        /// </summary>
        public void Inicializar()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                //a veces cando non hai migracions pendientes a app pode dar un erro, co cal usamos este bloque catch para recoller posibles erros
                throw;
            }

            if (_db.Roles.Any(r => r.Name == DE.Admin))
            {
                return; //se o Admin existe, volvemos, porque o noso rol esta creado
            }

            //CREACION DE ROLES
            _xestorRoles.CreateAsync(new IdentityRole(DE.Admin)).GetAwaiter().GetResult();//sendo Async e GetAwaiter, aseguramonos de que antes de proceder o seguinte paso, este foi ben, e creamos antes un Admin automaticamente, e DESPOIS un Supervisor, non ao reves
            _xestorRoles.CreateAsync(new IdentityRole(DE.Supervisor)).GetAwaiter().GetResult();

            //CREACION DE USUARIO ADMIN -unha vez os roles foron creados, procedemos a agregar o usuario correspondente. Poderias usar IdentityUser xa que AplicacionUsuario e unha implementacion de IdentityUser personalizada para este proxecto
            _xestorUsuarios.CreateAsync(new IdentityUser
            {
                UserName = "admin@hotmail.com",
                Email = "admin@hotmail.com",
                EmailConfirmed = true
            }, "Admin277.").GetAwaiter().GetResult(); //Admin277 e o password

            IdentityUser usuarioAdmin = _db.Users.Where(u => u.Email == "admin@hotmail.com").FirstOrDefault(); //con IdentityUser haberia que cambiar AplicacionUsuario por Users despois do _db. tal que asi: IdentityUser usuarioAdmin = _db.Users.Where(u => u.Email == "admin@hotmail.com").FirstOrDefault();
            _xestorUsuarios.AddToRoleAsync(usuarioAdmin, DE.Admin).GetAwaiter().GetResult();
        }
    }
}

//Na 1ª version o admin ten de email nonseiqueeesto@hotmail.com e a clave e Pa7777777.
