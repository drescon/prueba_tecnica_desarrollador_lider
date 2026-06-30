using System.ComponentModel.DataAnnotations;

namespace GestionSolicitudes.Application.Dto.Request;

public partial record CreateSolicitudRequest
{
    [Required(ErrorMessage = "El tipo de solicitud es requerido")]
    public int TipoId { get; set; }

    [Required(ErrorMessage = "Las observaciones son requeridas")]
    [MinLength(5, ErrorMessage = "Las observaciones deben tener al menos 5 caracteres")]
    [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
    public string Observaciones { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }
}
