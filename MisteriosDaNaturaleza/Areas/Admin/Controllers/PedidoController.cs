using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;
using MisteriosDaNaturaleza.Modelos.ViewModelos;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PedidoController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;

        public PedidoController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalles(int id)
        {
            PedidoViewModel pedidoVM = new PedidoViewModel()
            {
                CabeceiraPedido = _unidadeDeTraballo.CabeceiraPedido.Get(id),
                DetallesPedido = _unidadeDeTraballo.DetallesPedido.GetTodo(filtro: o => o.IdCabeceiraPedido == id)
            };

            return View(pedidoVM);
        }

        public IActionResult Aceptar(int id)
        {
            var pedidoDaBd = _unidadeDeTraballo.CabeceiraPedido.Get(id);//obten o pedido basado na id
            if (pedidoDaBd == null)
            {
                return NotFound();
            }
            _unidadeDeTraballo.CabeceiraPedido.CambiarEstadoDePedido(id, DE.EstadoAprobado); //no metodo CambiarEstadoDePedido xa gardamos os cambios, co cal non necesitamos gardalos aqui e devolvemos a vista
            return View(nameof(Index));
        }

        public IActionResult Rexeitar(int id)
        {
            var pedidoDaBd = _unidadeDeTraballo.CabeceiraPedido.Get(id);//obten o pedido basado na id
            if (pedidoDaBd == null)
            {
                return NotFound();
            }
            _unidadeDeTraballo.CabeceiraPedido.CambiarEstadoDePedido(id, DE.EstadoRexeitado); //no metodo CambiarEstadoDePedido xa gardamos os cambios, co cal non necesitamos gardalos aqui e devolvemos a vista
            return View(nameof(Index));
        }

        #region Chamadas API
        public IActionResult GetTodosPedidos()
        {
            return Json(new { data = _unidadeDeTraballo.CabeceiraPedido.GetTodo() });
        }

        public IActionResult GetTodosPedidosPendientes()
        {
            return Json(new { data = _unidadeDeTraballo.CabeceiraPedido.GetTodo(filtro: o => o.Estado == DE.EstadoEnviado) });
        }

        public IActionResult GetTodosPedidosAceptados()
        {
            return Json(new { data = _unidadeDeTraballo.CabeceiraPedido.GetTodo(filtro: o => o.Estado == DE.EstadoAprobado) });
        }
        #endregion
    }
}
