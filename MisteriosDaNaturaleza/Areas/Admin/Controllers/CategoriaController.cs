using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    [Authorize]//Agora so se permite a entrada a determinados metodos a usuarios autorizados, de non selo, pedirá un log in
    [Area("Admin")]//importante non esquecerse de agregar a Area onde traballara este controller
    public class CategoriaController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;

        public CategoriaController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo; //para que isto funcione temos que agregar a Dependency Injection no arquivo Startup.cs
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Este e o metodo ActualizarInsertar para peticions http de tipo GET.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ActualInsertar(int? id)
        {
            Categoria categoria = new Categoria();
            if (id == null) //se id e null significa que e un Insertar non e Actualizar
            {
                return View(categoria); //enton non queremos leer datos e devolvemos a vista con un obxeto vacio de tipo Categoria
            }

            //cando a id non e null temos que desplegar as ids o usuario cando a paxina se carga
            categoria = _unidadeDeTraballo.Categoria.Get(id.GetValueOrDefault());//GetValueoOrDefault usase porque a id e nullable, e se pasamos directamente a id en forma de null, daria un erro
            if (categoria == null)
            {
                return NotFound(); //se a categoria e incorrecta devolver non atopado
            }

            return View(categoria);//se atopamos o obxeto e actualizamos a ista co obxeto obtido da nosa base de datos

        }

        /// <summary>
        /// Metodo ActualizarInsertar para peticions http de tipo POST
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]//para usuarios maliciosos que agregan datos de certo tipo na nosa form, que con este token trataronse como unha peticion de hacker
        public IActionResult ActualInsertar(Categoria categoria)
        {
            if (ModelState.IsValid)//pide os atributos correctos da clase Cateogoria
            {
                if (categoria.Id == 0)//se o Id de Categoria e 0, significa que temos que engadir esa Categoria na Base de Datos
                {
                    _unidadeDeTraballo.Categoria.Engadir(categoria);
                }
                else //sino tratase dunha Actualizacion
                {
                    _unidadeDeTraballo.Categoria.Actualizar(categoria);
                }
                _unidadeDeTraballo.Gardar();//engadamos ou actualizamos temos que gardar os cambios na Base de Datos
                return RedirectToAction(nameof(Index));
            }
            return View(categoria); //se o modelo non e valido, queremos volver a Vista e pasamos o obxeto categoria
        }

        #region CHAMADAS A API
        [HttpGet]
        public IActionResult GetTodo()
        {
            /*return Json(new { data = _unidadeDeTraballo.Categoria.GetTodo() });*///convertimos o obxeto obtido nun obxeto Json. Usado orixinalmente, cambiado despois para usar unha Stored Procedure da Base de Datos
            return Json(new { data = _unidadeDeTraballo.Chamada_SP.DevolverLista<Categoria>(DE.sp_GetTodasCategorias, null) });
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var obxetoDaBaseDatos = _unidadeDeTraballo.Categoria.Get(id);
            if (obxetoDaBaseDatos == null)
            {
                return Json(new { success = false, message = "Erro borrando o elemento." });
            }

            _unidadeDeTraballo.Categoria.Eliminar(obxetoDaBaseDatos);
            _unidadeDeTraballo.Gardar(); //os cambios gardanse na Base de Datos
            return Json(new { success = true, message = "Borrado realizado con exito." });
        }
        #endregion
    }
}
