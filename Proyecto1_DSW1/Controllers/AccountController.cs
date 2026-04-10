using Microsoft.AspNetCore.Mvc;
using Proyecto1_DSW1.Data;
using Proyecto1_DSW1.Models;

namespace Proyecto1_DSW1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AccountController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UsuarioModel model)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, regresa al formulario mostrando los errores
                return View(model);
            }

            // Verificar si el correo ya está registrado
            bool correoExiste = await _usuarioRepo.CorreoExisteAsync(model.Correo);
            if (correoExiste)
            {
                ModelState.AddModelError("Correo", "Este correo electrónico ya está registrado.");
                return View(model);
            }

            // Guardar el usuario en la base de datos
            model.FechaRegistro = DateTime.Now;
            await _usuarioRepo.RegistrarUsuarioAsync(model);

            TempData["Message"] = $"¡Bienvenido, {model.Nombre}! Tu cuenta fue registrada correctamente.";
            return RedirectToAction("Index", "Home");
        }
    }
}
