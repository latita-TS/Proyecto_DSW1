using System.ComponentModel.DataAnnotations;

namespace Proyecto1_DSW1.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio (S/)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, 99999, ErrorMessage = "La cantidad no puede ser negativa.")]
        [Display(Name = "Stock")]
        public int Cantidad { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;
    }
}
