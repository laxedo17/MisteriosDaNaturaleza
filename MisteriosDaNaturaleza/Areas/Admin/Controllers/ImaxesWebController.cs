using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data;
using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    [Authorize]//Agora so se permite a entrada a determinados metodos a usuarios autorizados, de non selo, pedirá un log in
    [Area("Admin")]//importante non esquecerse de agregar a Area onde traballara este controller
    public class ImaxesWebController : Controller
    {
        //private readonly IUnidadeDeTraballo _unidadeDeTraballo;
        private readonly ApplicationDbContext _db; //en vez de Unidade de Traballo, neste caso accedemos directamente con ApplicationDbContext, anque se pode facer con unidade de traballo tamen.

        /// <summary>
        /// Non usamos o tipico Repository Pattern, pero ApplicationDbContext directamente. Non e consistente co resto da app, pero esta feito con propositos de testeo
        /// </summary>
        /// <param name="db"></param>
        public ImaxesWebController(ApplicationDbContext db)
        {
            _db = db; //para que isto funcione temos que agregar a Dependency Injection no arquivo Startup.cs
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
            ImaxesWeb obxetoImaxe = new ImaxesWeb();
            if (id == null) //se id e null significa que e un Insertar non e Actualizar
            {
                //aqui agora non devolvemos nada, deixamos o return de abaixo que faga iso
                //return View(obxetoImaxe); //enton non queremos leer datos e devolvemos a vista con un obxeto vacio de tipo Imaxen
            }
            else
            {
                //cando a id non e null temos que desplegar as ids o usuario cando a paxina se carga
                obxetoImaxe = _db.ImaxesWeb.SingleOrDefault(m => m.Id == id);
                if (obxetoImaxe == null)
                {
                    return NotFound(); //se a categoria e incorrecta devolver non atopado
                }
            }

            return View(obxetoImaxe);//se atopamos o obxeto e actualizamos a ista co obxeto obtido da nosa base de datos

        }

        /// <summary>
        /// Metodo ActualizarInsertar para peticions http de tipo POST para imaxes. No POST gardamos a imaxe na Base de Datos, usamos o id para iso. Podemos usar o do obxeto Image en si ou facelo a traves dun id que obtemos como parametro, que e a opcion que seguimos aqui.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obxetoImaxe"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]//para usuarios maliciosos que agregan datos de certo tipo na nosa form, que con este token trataronse como unha peticion de hacker
        public IActionResult ActualInsertar(int id, ImaxesWeb obxetoImaxe)
        {
            if (ModelState.IsValid)//pide os atributos correctos da clase Cateogoria
            {
                var arquivos = HttpContext.Request.Form.Files;//obtemos a lista de imaxes que o usuario xa ten subidas

                if (arquivos.Count > 0)//se xa hai imaxes subidas
                {
                    byte[] imaxe1 = null;
                    using(var strAr1 = arquivos[0].OpenReadStream()) //strAr1 e traduccion de FileStream 1
                    {
                        using (var memSt1 = new MemoryStream())
                        {
                            strAr1.CopyTo(memSt1);
                            imaxe1 = memSt1.ToArray();
                        }
                    }
                    obxetoImaxe.Imaxe = imaxe1; //Se CALQUERA imaxe se sube a Base de Datos, convertimola e gardamola como unha Imaxen
                }

                if (obxetoImaxe.Id == 0)//permite diferenciar se e unha nova subida ou un Edit. Tamen podemos facer esta comprobacion coa id que lle pasamos ao metodo
                {
                    _db.ImaxesWeb.Add(obxetoImaxe);
                }
                else //sinon tratase dunha Actualizacion dunha imaxen que xa temos na Base de Datos
                {
                    var imaxeDaBaseDeDatos = _db.ImaxesWeb.Where(c => c.Id == id).FirstOrDefault();//unha vez temos a imaxe
                    imaxeDaBaseDeDatos.Nome = imaxeDaBaseDeDatos.Nome;
                    if (arquivos.Count > 0)//se se subiu algunha imaxe
                    {
                        imaxeDaBaseDeDatos.Imaxe = obxetoImaxe.Imaxe;
                    }
                }
                _db.SaveChanges();//engadamos ou actualizamos temos que gardar os cambios na Base de Datos
                return RedirectToAction(nameof(Index));
            }
            return View(obxetoImaxe); //se o modelo non e valido, queremos volver a de novo a Vista do Subidor de imaxes
        }

        #region CHAMADAS A API
        [HttpGet]
        public IActionResult GetTodo()
        {
            /*return Json(new { data = _unidadeDeTraballo.Categoria.GetTodo() });*///convertimos o obxeto obtido nun obxeto Json. Usado orixinalmente, cambiado despois para usar unha Stored Procedure da Base de Datos
            return Json(new { data = _db.ImaxesWeb.ToList() });
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var obxetoDaBaseDatos = _db.ImaxesWeb.Find(id);//en vez de Get temos un metodo chamado Find provinte de ApplicationDbContext
            if (obxetoDaBaseDatos == null)
            {
                return Json(new { success = false, message = "Erro borrando o elemento." });
            }

            _db.ImaxesWeb.Remove(obxetoDaBaseDatos);
            _db.SaveChanges(); //os cambios gardanse na Base de Datos, en vez do noso metodo Gardar, usamos SaveChanges, incluido na clase ApplicationDbContext. Como Repository Pattern e mais estricto, da lugar a menos erros, e por iso o temos usado ata agora
            return Json(new { success = true, message = "Borrado realizado con exito." });
        }
        #endregion
    }
}
