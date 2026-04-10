using Microsoft.AspNetCore.Mvc;
using Proyecto1_DSW1.Data;
using Proyecto1_DSW1.Models;

namespace Proyecto1_DSW1.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepository _productoRepo;

        public ProductosController(ProductoRepository productoRepo)
        {
            _productoRepo = productoRepo;
        }

        // GET: /Productos/Index
        public async Task<IActionResult> Index(int pagina = 1)
        {
            int registrosPorPagina = 10;
            var (productos, totalRegistros) = await _productoRepo.ObtenerProductosPaginadoAsync(pagina, registrosPorPagina);

            ViewBag.PaginaActual  = pagina;
            ViewBag.TotalPaginas  = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return View(productos);
        }

        // GET: /Productos/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel productoModel)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores, regresa al formulario con los mensajes de error
                return View(productoModel);
            }

            // Guardar en la base de datos
            await _productoRepo.AgregarProductoAsync(productoModel);

            TempData["Message"] = $"Producto '{productoModel.Nombre}' registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
