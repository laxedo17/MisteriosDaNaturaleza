using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class Chama_StoredProcedure : IChamadaStoreProcedure
    {
        private readonly ApplicationDbContext _db;

        private static string StringConexion = "";

        //sera a 1ª vez en todo o programa que non se usa EntityFrameworkCore para conectarse coa base de datos senon que nos comunicaremos con SQL Server
        public Chama_StoredProcedure(ApplicationDbContext db)
        {
            _db = db;
            StringConexion = db.Database.GetDbConnection().ConnectionString; //isto obten a cadena de texto da nosa conexion de ApplicationDbContext
        }

        public IEnumerable<T> DevolverLista<T>(string nomeStProcedure, DynamicParameters param = null)
        {
            //using permite que 
            using (SqlConnection conexion = new SqlConnection(StringConexion))
            {
                conexion.Open();
                return conexion.Query<T>(nomeStProcedure, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Convirte a saida do metodo nun obxeto de tipo T (Generic)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nomeStProcedure"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecutarDevolverEscalar<T>(string nomeStProcedure, DynamicParameters param = null)
        {
            using (SqlConnection conexion = new SqlConnection(StringConexion))
            {
                conexion.Open();
                return (T)Convert.ChangeType(conexion.ExecuteScalar<T>(nomeStProcedure, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }

        /// <summary>
        /// En vez de usar return Query<T>, utilizamos a conexion para facer Execute e non unha consulta tipica. 
        /// </summary>
        /// <param name="nomeStProcedure"></param>
        /// <param name="param"></param>
        public void ExecutarSenReturn(string nomeStProcedure, DynamicParameters param = null)
        {
            using (SqlConnection conexion = new SqlConnection(StringConexion))
            {
                conexion.Open();
                conexion.Execute(nomeStProcedure, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
