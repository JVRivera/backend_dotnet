using System.ComponentModel.DataAnnotations;

namespace BackendTareas.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [MinLength(3, ErrorMessage = "El título debe tener al menos 3 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public string Estado { get; set; } = "pendiente";

        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
        public DateTime? FechaVencimiento { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}