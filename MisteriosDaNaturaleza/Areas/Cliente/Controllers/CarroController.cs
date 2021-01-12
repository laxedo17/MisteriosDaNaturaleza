using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio;
using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Extensions;
using MisteriosDaNaturaleza.Modelos;
using MisteriosDaNaturaleza.Modelos.ViewModelos;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class CarroController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;

        [BindProperty] //BindProperty e + beneficioso telo aqui para non ter que situala nos parametros dos metodos
        public CarroViewModel CarroVM { get; set; }
        public CarroController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo;

            CarroVM = new CarroViewModel
            {
                CabeceiraPedido = new CabeceiraPedido(),//hai que asignar esta propiedade obtendo todos os obxetos para que non cree unha excepcion despois
                ListaDestinos = new List<Destino>()
                //Viaxantes = new Viaxantes()
            };
        }
        public IActionResult Index()
        {
            //poblamos a vista cos detalles da sesion do carro da compra. 
            if(HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion)!=null)
            {
                List<int> listaSesion = new List<int>(); //creamos unha lista para albergar os obxetos da sesion
                listaSesion = HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion);//pasamoslle os obxetos do carro a lista

                //loop recorrendo todos os elementos da lista
                foreach (int idDestino in listaSesion)
                {
                    CarroVM.ListaDestinos.Add(_unidadeDeTraballo.Destino.GetFirstOrDefault(u => u.Id == idDestino, incluePropiedades: "Frecuencia,Categoria"));
                }

            }
            return View(CarroVM);
        }

        public IActionResult Eliminar(int idDestino)
        {
            List<int> listaSesion = new List<int>(); //creamos unha lista para albergar os obxetos da sesion potencialmente para borrar
            listaSesion = HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion);//pasamoslle os obxetos do carro a lista
            listaSesion.Remove(idDestino);
            HttpContext.Session.SetObxeto(DE.CarroSesion, listaSesion);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Crea un resumen dos datos de compra.
        /// </summary>
        /// <returns></returns>
        public IActionResult Resumen()
        {
            // poblamos a vista cos detalles da sesion do carro da compra.
            if (HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion) != null)
            {
                List<int> listaSesion = new List<int>(); //creamos unha lista para albergar os obxetos da sesion
                listaSesion = HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion);//pasamoslle os obxetos do carro a lista

                //loop recorrendo todos os elementos da lista
                foreach (int idDestino in listaSesion)
                {
                    CarroVM.ListaDestinos.Add(_unidadeDeTraballo.Destino.GetFirstOrDefault(u => u.Id == idDestino, incluePropiedades: "Frecuencia,Categoria"));
                }

            }
            return View(CarroVM);
        }

        /// <summary>
        /// Metodo POST para realizar un pedido. En vez de chamar a ResumenPOST, con ActionName podemos chamalo Resumen, e ASP.NET mapeara a peticion POST de Resumen a este metodo ResumenPOST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public IActionResult ResumenPOST()
        {
            // poblamos a vista cos detalles da sesion do carro da compra.
            if (HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion) != null)
            {
                List<int> listaSesion = new List<int>();//creamos unha lista para albergar os obxetos da sesion
                listaSesion = HttpContext.Session.GetObxeto<List<int>>(DE.CarroSesion);//pasamoslle os obxetos do carro a lista
                CarroVM.ListaDestinos = new List<Destino>();
                foreach (int idDestino in listaSesion)
                {
                    CarroVM.ListaDestinos.Add(_unidadeDeTraballo.Destino.GetFirstOrDefault(u => u.Id == idDestino, incluePropiedades:"Frecuencia,Categoria"));
                }
            }

            if (!ModelState.IsValid)//se o modelo non e valido
            {
                return View(CarroVM);
            }
            else
            {
                CarroVM.CabeceiraPedido.DataPedido = DateTime.Now;
                CarroVM.CabeceiraPedido.Estado = DE.EstadoEnviado;
                CarroVM.CabeceiraPedido.ContaDestinos = CarroVM.ListaDestinos.Count;
                _unidadeDeTraballo.CabeceiraPedido.Engadir(CarroVM.CabeceiraPedido);//temos os datos pero hai que "empuxalos" a Base de Datos
                _unidadeDeTraballo.Gardar();//para que se garden os cambios na BD

                //agora recollemos todos os datos dos nosos destinos
                foreach (var elemento in CarroVM.ListaDestinos)
                {
                    //creamos un obxeto DetallesPedido para gardar os detalles
                    DetallesPedido detallesPedido = new DetallesPedido
                    {
                        DestinoId = elemento.Id,
                        IdCabeceiraPedido = CarroVM.CabeceiraPedido.Id,//o gardar antes o obxeto na BD a cabeceira de pedido, non temos que ir buscala a base de datos, senon que xa estar dispoñible na cabeceira do pedido
                        NomeServicio = elemento.Nome,
                        Precio = elemento.Precio
                    };

                    _unidadeDeTraballo.DetallesPedido.Engadir(detallesPedido);

                }
                _unidadeDeTraballo.Gardar();//se deixamos isto no bucle, gardara os elementos un por un, pero iso non e eficiente, se usamos gardar fora do bucle, ASP.NET Core gardara todos os elementos engadidos xuntos nunha sola vez
                //unha vez gardados na base de datos, temos que baleirar a sesion
                HttpContext.Session.SetObxeto(DE.CarroSesion, new List<int>());//setearemos o carro a unha nova lista de obxetos baleiros
                return RedirectToAction("ConfirmacionPedido", "Carro", new { id = CarroVM.CabeceiraPedido.Id });
                //asi saimos a unha nova accion chamada ConfirmacionPedido dentro do controller de Carro e a accion pasamoslle a id do encargo que se realizou
            }
        }


        public IActionResult ConfirmacionPedido(int id)
        {
            return View(id); //pasamoslle a esta visa a id do pedido realizado no metodo de arriba, asi de sinxelo
        }
    }
}
