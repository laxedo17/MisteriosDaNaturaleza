using Microsoft.AspNetCore.Mvc.Rendering;

using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio
{
    /// <summary>
    /// Agregamos metodos que non estan implementados en IRepositorio, como Actualizar ou Listar todas as categorias
    /// xa que non se trata de metodos comuns a todas as clases.
    /// </summary>
    public interface IRepositorioCategoria : IRepositorio<Categoria>
    {
        IEnumerable<SelectListItem> GetListaCategoriasMenuDesplegable();

        void Actualizar(Categoria categoria);
    }
}
