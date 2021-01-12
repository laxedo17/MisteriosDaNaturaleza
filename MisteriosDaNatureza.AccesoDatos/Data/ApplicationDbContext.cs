using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// O DbContext que usaremos para acceder a Base de Datos coa nosa app cando o necesitemos.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Frecuencia> Frecuencia { get; set; }

        public DbSet<Destino> Destino { get; set; }

        public DbSet<CabeceiraPedido> CabeceiraPedido { get; set; }

        public DbSet<DetallesPedido> DetallesPedido { get; set; }

        public DbSet<AplicacionUsuario> AplicacionUsuario { get; set; }

        public DbSet<ImaxesWeb> ImaxesWeb { get; set; } //para despois engadir unha migracion a Base de Datos e ter o noso propio Uploader de imaxes web
    }
}
