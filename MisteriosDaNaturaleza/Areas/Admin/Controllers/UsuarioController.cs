using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Areas.Admin.Controllers
{
    //password de Admin Pa7777777.
    [Area("Admin")]
    [Authorize(Roles = DE.Admin)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadeDeTraballo _unidadeDeTraballo;
        public UsuarioController(IUnidadeDeTraballo unidadeDeTraballo)
        {
            _unidadeDeTraballo = unidadeDeTraballo;
        }

        /// <summary>
        /// Obtemos todos os usuarios da base de datos, EXCEPTO o que esta logueado, porque non queremos que se borre accidentalmente.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var reivindicarIdentidade = (ClaimsIdentity)this.User.Identity;
            var reivindicacions = reivindicarIdentidade.FindFirst(ClaimTypes.NameIdentifier);

            return View(_unidadeDeTraballo.Usuario.GetTodo(u => u.Id != reivindicacions.Value)); //mostramos todos os usuarios menos o que esta logueado
        }

        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unidadeDeTraballo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unidadeDeTraballo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
