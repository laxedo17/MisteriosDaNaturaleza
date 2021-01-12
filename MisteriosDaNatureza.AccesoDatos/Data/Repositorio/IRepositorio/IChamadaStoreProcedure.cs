using Dapper;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface IChamadaStoreProcedure : IDisposable
    {
        IEnumerable<T> DevolverLista<T>(string nomeStProcedure, DynamicParameters param = null); //usamos Generics, por iso a T, generic types permitennos usar calquer tipo de clase ou struct

        void ExecutarSenReturn(string nomeStProcedure, DynamicParameters param = null);

        T ExecutarDevolverEscalar<T>(string nomeStProcedure, DynamicParameters param = null);
    }
}
