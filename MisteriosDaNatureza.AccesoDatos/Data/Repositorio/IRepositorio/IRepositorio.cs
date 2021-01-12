using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
    /// <summary>
    /// Permite usar todas estas funcions comuns (engadir, obter, borrar en TODOS os repositorios
    /// Repository Pattern, permite saber con k repositorio estamos traballando cando usamos calquer modelo. Forza a usar un modelo determinado, e preven erros e estilos de codificacion aleatorios.
    /// Clase xenerica, por iso o <typeparamref name="T"/> < T > (Generics) que indica que calquera clase pode usala, de maneira k calquera obxeto pode chamar a esta interface
    /// </summary>
    public interface IRepositorio<T> where T : class //indicamos que T sera unha clase
    {
        T Get(int id); //devolvera un T Generic dunha clase

        //para recibir unha lista -por iso o IEnumerable- de Categorias. Para ordenar por OrderBy necesitamos IQueryable.
        //Con GetTodo() necesitamos certos parametros: temos que filtrar datos, OrderBy ou eager loading
        IEnumerable<T> GetTodo(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string incluePropiedades = null
            );

        //Funcion FirstOrDefault, que devolvera un SOLO obxeto que cumpla certos criterios. Orderby non e necesario porque imos devolver solo un obxeto
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filtro = null,
            string incluePropiedades = null
            );


        //Funcions para engadir ou eliminar unha entidade na base de datos
        void Engadir(T entidade); //afecta a unha entidade da base de datos

        void Eliminar(int id); //se o usuario pasa unha id podemos eliminar a entidade pasandolle un id

        void Eliminar(T entidade); //se pasamos unha entidade completa para eliminar da entidade
    }
}
