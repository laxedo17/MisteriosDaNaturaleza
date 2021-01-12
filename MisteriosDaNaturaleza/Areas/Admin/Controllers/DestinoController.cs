using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos.ViewModelos;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DestinoController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;
        private readonly IWebHostEnvironment _hostEntorno; //no caso de Destino, necesitamenos subir arquivos o servidor, neste caso imaxes, para saber a ruta do arquivo

        //Con este atributo DestVM queda automaticamente asociado a este controlador, co cal nos metodos da vista non temos que crear un obxeto o que chamamos desde o metodo
        [BindProperty]
        public DestinoVM DestVM { get; set; }

        public DestinoController(IUnidadeDeTraballo unidadeDeTraballo, IWebHostEnvironment hostEntorno)
        {
            _unidadeDeTraballo = unidadeDeTraballo;
            _hostEntorno = hostEntorno;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ActualInsertar(int? id)
        {
            DestVM = new DestinoVM() //como temos BindProperty xa non temos que crear un obxeto DestinoVM DestVM = new DestinoVM()
            {
                Destino = new Modelos.Destino(),
                ListaCategoria = _unidadeDeTraballo.Categoria.GetListaCategoriasMenuDesplegable(),
                ListaFrecuencia = _unidadeDeTraballo.Frecuencia.GetListaFrecuenciasMenuDesplegable()
            };

            //se estamos editando un Destino, necesitamos obter os Datos da Base de Datos, senon o codigo de arriba non funcionara para editar xa que non estamos obtendo datos
            if (id != null)
            {
                DestVM.Destino = _unidadeDeTraballo.Destino.Get(id.GetValueOrDefault());
            }

            return View(DestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualInsertar() //o ter BindProperty non temos que pasar un obxeto DestinoVM nos parametros
        {
            if (ModelState.IsValid)
            {
                string rutaDirectorioWeb = _hostEntorno.WebRootPath;
                var arquivos = HttpContext.Request.Form.Files;

                if (DestVM.Destino.Id == 0) //se e 0, significa k estamos creando un novo destino
                {
                    //Novo Destino
                    string nomeArquivo = Guid.NewGuid().ToString();//usamos unha Guid random para nomear o arquivo, senon podemos nomealo basandonos na Id do obxeto
                    var subidas = Path.Combine(rutaDirectorioWeb, @"imaxes\destinos");//usaremos a ruta da carpeta Imaxes e o subdirectorio Destinos para subir os arquivos o servidor
                    var extension = Path.GetExtension(arquivos[0].FileName);//poñemos arquivos[0] porque so queremos subir unha imaxen. Como xa obrigamos a subir unha imaxen non temos que facer a comprobacion aqui de se realmente se sube unha imaxe

                    //con using liberamos o elemento da base de datos de memoria para que non este ocupando un bufer de memoria cando non usemos o elemento
                    using (var streamsArquivo = new FileStream(Path.Combine(subidas, nomeArquivo + extension), FileMode.Create))
                    {
                        arquivos[0].CopyTo(streamsArquivo);
                    }

                    DestVM.Destino.ImaxeUrl = @"\imaxes\destinos\" + nomeArquivo + extension; //gardamos isto na nosa ImaxeUrl

                    _unidadeDeTraballo.Destino.Engadir(DestVM.Destino);//agora gardamos isto na base de datos                   
                }
                else //en caso de non ser un elemento novo senon que queremos editar
                {
                    //Editar Destino
                    var destinoDaBaseDeDatos = _unidadeDeTraballo.Destino.Get(DestVM.Destino.Id);

                    //se a cantidade de arquivos e superior a 0, necesitamos un novo nome de arquivo
                    if (arquivos.Count > 0)
                    {
                        string nomeArquivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaDirectorioWeb, @"imaxes\destinos");
                        var extension_nova = Path.GetExtension(arquivos[0].FileName);

                        var rutaImaxen = Path.Combine(rutaDirectorioWeb, destinoDaBaseDeDatos.ImaxeUrl.TrimStart('\\')); //queremos eliminar a barra invertida da ruta
                        if (System.IO.File.Exists(rutaImaxen))
                        {
                            System.IO.File.Delete(rutaImaxen);//se existe a ruta da imaxen, enton queremos eliminar o arquivo existente e subimos o novo arquivo
                        }

                        using (var streamsArquivo = new FileStream(Path.Combine(subidas, nomeArquivo + extension_nova), FileMode.Create))
                        {
                            arquivos[0].CopyTo(streamsArquivo);
                        }
                        DestVM.Destino.ImaxeUrl = @"\imaxes\destinos\" + nomeArquivo + extension_nova;
                    }
                    else //se o arquivo non existe, mantemos a ImaxeUrl
                    {
                        DestVM.Destino.ImaxeUrl = destinoDaBaseDeDatos.ImaxeUrl;//se non borramos a imaxen non cambiamos tampouco a ruta da ImaxeUrl na base de  datos
                    }

                    _unidadeDeTraballo.Destino.Actualizar(DestVM.Destino);
                }
                _unidadeDeTraballo.Gardar();//salvamos para que se vexan reflexados os cambios na base de datos

                return RedirectToAction(nameof(Index));
            }
            else
            {
                DestVM.ListaCategoria = _unidadeDeTraballo.Categoria.GetListaCategoriasMenuDesplegable();
                DestVM.ListaFrecuencia = _unidadeDeTraballo.Frecuencia.GetListaFrecuenciasMenuDesplegable();
                return View(DestVM);//se o modelstate non e valido volvemos á vista e pasamos o Destino ViewModel
            }
        }


        //Este POST fariase agregando un obxeto tipo DestinoVM (Destino ViewModel), pero usaremos o atributo BindProperty e asi non temos que chamar o obxeto no metodo
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ActualInsertar(DestinoVM DestVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string rutaDirectorioWeb = _hostEntorno.WebRootPath;
        //    }
        //}

        #region API Chamadas
        public IActionResult GetTodo()
        {
            return Json(new { data = _unidadeDeTraballo.Destino.GetTodo(incluePropiedades: "Categoria,Frecuencia") }); //con estas propiedades asociamos o destino da Viaxe coa clave de Categoria e Frecuencia, que son entidades necesarias, GetTodo obten os datos das entidade Categoria e Frecuencia e e MOI importante que os valores vaian separados por COMAS SEN UN ESPACIO despois da coma, senon dara un erro, xa que utilizamos como separador de Entidades as comas
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            var destinoDaBaseDeDatos = _unidadeDeTraballo.Destino.Get(id);
            string rutaWeb = _hostEntorno.WebRootPath;
            var rutaImaxen = Path.Combine(rutaWeb, destinoDaBaseDeDatos.ImaxeUrl.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImaxen))
            {
                System.IO.File.Delete(rutaImaxen);
            }

            if (destinoDaBaseDeDatos == null)
            {
                return Json(new { success = false, message = "Erro durante o borrado" });
            }

            _unidadeDeTraballo.Destino.Eliminar(destinoDaBaseDeDatos);
            _unidadeDeTraballo.Gardar();//temos que asegurarnos SEMPRE de gardar os cambios na base de datos cando algo se modifique
            return Json(new { success = true, message = "Elemento borrado satisfactoriamente." });
        }
        #endregion
    }
}
