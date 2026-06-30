using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitudes.Infrastructure.Entities;

    [Table("Solicitudes")]
    public class Solicitud
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Numero { get; set; } = null!;
        
        public DateTime FechaSolicitud { get; set; }
        public int UsuarioId { get; set; }
        
        [MaxLength(50)]
        public string Tipo { get; set; } = null!;
        
        public int EstadoId { get; set; }
        public string? Observaciones { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; } = null!;
        
        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; } = null!;
        
        public virtual ICollection<Seguimiento> Seguimientos { get; set; } = new List<Seguimiento>();
    }