using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    /// <summary>
    /// A clase para usar o famoso procedemento Unit of Work e implementa a interfaz IUnidadeDeTraballo
    /// </summary>
    public class UnidadeDeTraballo : IUnidadeDeTraballo
    {
        private readonly ApplicationDbContext _db;

        public UnidadeDeTraballo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new RepositorioCategoria(_db);
            Frecuencia = new RepositorioFrecuencia(_db);
            Destino = new RepositorioDestino(_db);
            CabeceiraPedido = new RepositorioCabeceiraPedido(_db);
            DetallesPedido = new RepositorioDetallesPedido(_db);
            Usuario = new RepositorioUsuario(_db);
            Chamada_SP = new Chama_StoredProcedure(_db);
        }

        public IRepositorioCategoria Categoria { get; private set; } //e private porque non queremos que se configure isto fora de esta clase

        public IRepositorioFrecuencia Frecuencia { get; private set; }

        public IRepositorioDestino Destino { get; private set; }

        public IRepositorioCabeceiraPedido CabeceiraPedido { get; private set; }

        public IRepositorioDetallesPedido DetallesPedido { get; private set; }

        public IRepositorioUsuario Usuario { get; private set; }

        public IChamadaStoreProcedure Chamada_SP { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Gardar()
        {
            _db.SaveChanges();
        }
    }
}
