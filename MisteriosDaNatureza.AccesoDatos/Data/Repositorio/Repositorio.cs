using Microsoft.EntityFrameworkCore;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //temos que acceder primeiro a Base de Datos
        protected readonly DbContext Contexto;
        internal DbSet<T> dbSet; //podemos instanciar a clase, pero non podemos acceder a este componente xk e internal
        //se a clase fose internal, tampouco se poderia acceder desde fora da assembly

        /// <summary>
        /// Inicializa o campo Contexto con Dependency Injection (un obxeto recibe outros obxetos dos que depende)
        /// </summary>
        /// <param name="contexto"></param>
        public Repositorio(DbContext contexto)
        {
            Contexto = contexto;
            this.dbSet = contexto.Set<T>(); //agora cada vez que fagamos unha operacion das de abaixo, usaremos o obxeto dbSet
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Devolve o primeiro obxeto que queda na query.
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="incluePropiedades"></param>
        /// <returns></returns>
        public T GetFirstOrDefault(Expression<Func<T, bool>> filtro = null, string incluePropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            //as propiedades que inclue seran comma separated, separadas por comas
            if (incluePropiedades != null) //as propiedades necesarias para facer eager loading
            {
                foreach (var incluePropiedade in incluePropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluePropiedade);//engade as propiedades unha a unha de IQueryable
                }
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Obten unha lista de todos os elementos que cumplan os criterios
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="orderBy"></param>
        /// <param name="incluePropiedades"></param>
        /// <returns></returns>
        public IEnumerable<T> GetTodo(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluePropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            //as propiedades que inclue seran comma separated, separadas por comas
            if(incluePropiedades != null) //as propiedades necesarias para facer eager loading
            {
                foreach(var incluePropiedade in incluePropiedades.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluePropiedade);//engade as propiedades unha a unha de IQueryable
                }
            }

            //unha vez filtrados os datos, incluidas as propiedades, usamos OrderBy
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();//se OrderBy e null, pasamos a query a unha lista
        }

        public void Engadir(T entidade)
        {
            dbSet.Add(entidade); //engade unha entidade a nosa DbSet da Base de Datos existente
        }

        /// <summary>
        /// Obtemos a entidade da Base de Datos a traves da Id e chamamos o metodo Eliminar para borrar a entidade.
        /// </summary>
        /// <param name="id"></param>
        public void Eliminar(int id)
        {
            T entidadeAEliminar = dbSet.Find(id);
            Eliminar(entidadeAEliminar); 
        }

        public void Eliminar(T entidade)
        {
            dbSet.Remove(entidade);
        }
    }
}
