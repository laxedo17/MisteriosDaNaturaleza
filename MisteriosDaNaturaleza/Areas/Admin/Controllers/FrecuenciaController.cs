using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrecuenciaController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;

        public FrecuenciaController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo;
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
            Frecuencia frecuencia = new Frecuencia();
            if (id == null) //se id e null significa que e un Insertar non e Actualizar
            {
                return View(frecuencia); //enton non queremos leer datos e devolvemos a vista con un obxeto vacio de tipo Categoria
            }

            //cando a id non e null temos que desplegar as ids o usuario cando a paxina se carga
            frecuencia = _unidadeDeTraballo.Frecuencia.Get(id.GetValueOrDefault());//GetValueoOrDefault usase porque a id e nullable, e se pasamos directamente a id en forma de null, daria un erro
            if (frecuencia == null)
            {
                return NotFound(); //se a categoria e incorrecta devolver non atopado
            }

            return View(frecuencia);//se atopamos o obxeto e actualizamos a ista co obxeto obtido da nosa base de datos

        }

        /// <summary>
        /// Metodo ActualizarInsertar para peticions http de tipo POST
        /// </summary>
        /// <param name="frecuencia"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]//para usuarios maliciosos que agregan datos de certo tipo na nosa form, que con este token trataronse como unha peticion de hacker
        public IActionResult ActualInsertar(Frecuencia frecuencia)
        {
            if (ModelState.IsValid)//pide os atributos correctos da clase Cateogoria
            {
                if (frecuencia.Id == 0)//se o Id de Categoria e 0, significa que temos que engadir esa Categoria na Base de Datos
                {
                    _unidadeDeTraballo.Frecuencia.Engadir(frecuencia);
                }
                else //sino tratase dunha Actualizacion
                {
                    _unidadeDeTraballo.Frecuencia.Actualizar(frecuencia);
                }
                _unidadeDeTraballo.Gardar();//engadamos ou actualizamos temos que gardar os cambios na Base de Datos
                return RedirectToAction(nameof(Index));
            }
            return View(frecuencia); //se o modelo non e valido, queremos volver a Vista e pasamos o obxeto categoria
        }

        #region CHAMADAS A API
        [HttpGet]
        public IActionResult GetTodo()
        {
            return Json(new { data = _unidadeDeTraballo.Frecuencia.GetTodo() });//convertimos o obxeto obtido nun obxeto Json
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var obxetoDaBaseDatos = _unidadeDeTraballo.Frecuencia.Get(id);
            if (obxetoDaBaseDatos == null)
            {
                return Json(new { success = false, message = "Erro borrando o elemento." });
            }

            _unidadeDeTraballo.Frecuencia.Eliminar(obxetoDaBaseDatos);
            _unidadeDeTraballo.Gardar(); //os cambios gardanse na Base de Datos
            return Json(new { success = true, message = "Borrado realizado con exito." });
        }
        #endregion
    }
}
