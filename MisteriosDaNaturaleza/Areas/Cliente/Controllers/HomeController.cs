using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Extensions;
using MisteriosDaNaturaleza.Modelos.ViewModelos;
using MisteriosDaNaturaleza.Models;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;
        private InicioViewModel InicioVM;

        public HomeController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            //poblamos a vista de datos de Categorias e Destinos. Non imos un menu desplegable
            InicioVM = new InicioViewModel()
            {
                ListaCategorias = _unidadeDeTraballo.Categoria.GetTodo(),
                ListaDestinos = _unidadeDeTraballo.Destino.GetTodo(incluePropiedades: "Frecuencia")
            };
            return View(InicioVM);
        }

        public IActionResult Detalles(int id)
        {
            var destinoDaBaseDeDatos = _unidadeDeTraballo.Destino.GetFirstOrDefault(incluePropiedades: "Categoria,Frecuencia", filtro: c => c.Id == id);
            return View(destinoDaBaseDeDatos);
        }

        public IActionResult EngadirACarro(int destinoId)
        {
            List<int> listaSesion = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(DE.CarroSesion)))
            {
                listaSesion.Add(destinoId);
                HttpContext.Session.SetObxeto(DE.CarroSesion, listaSesion);//aqui usamos os Extension Methods creados en Extensions
            }
            else //se xa hai valores na lista
            {
                listaSesion = HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion);//usamos GetObxeto para convertir os valores nunha lista, agregar a nova id de Destino a Lista, e despois gardar a sesion de volta
                if (!listaSesion.Contains(destinoId))
                {
                    listaSesion.Add(destinoId);
                    HttpContext.Session.SetObxeto(DE.CarroSesion, listaSesion);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
