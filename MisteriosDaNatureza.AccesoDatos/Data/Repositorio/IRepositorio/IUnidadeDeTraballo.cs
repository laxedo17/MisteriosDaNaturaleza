using System;
using System.Collections.Generic;
using System.Text;
//Para utilizar o Repository Pattern e Unit of Work, que nos facilitara moito a vida á larga
namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
    /// <summary>
    /// Unidade de Traballo (Unit of Work) representa o que se fara nunha transaccion simple, e tera acceso a todos os nosos repositorios asi como un metodo para gardar os cambios que fagamos na nosa Base de Datos.
    /// Cada vez que temos un repositorio novo, vimos aqui e engadimolo
    /// </summary>
    public interface IUnidadeDeTraballo : IDisposable
    {
        IRepositorioCategoria Categoria { get; }

        IRepositorioFrecuencia Frecuencia { get; }

        IRepositorioDestino Destino { get; }

        IRepositorioCabeceiraPedido CabeceiraPedido { get; }

        IRepositorioDetallesPedido DetallesPedido { get; }

        IChamadaStoreProcedure Chamada_SP { get; }

        IRepositorioUsuario Usuario { get; }

        void Gardar();
    }
}
