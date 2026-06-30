using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitudes.Infrastructure.Entities;

    [Table("Seguimientos")]
    public class Seguimiento
    {
        [Key]
        public int Id { get; set; }
        
        public int SolicitudId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public int EstadoId { get; set; }
        public string Comentario { get; set; } = null!;
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey(nameof(SolicitudId))]
        public virtual Solicitud Solicitud { get; set; } = null!;
        
        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; } = null!;
        
        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; } = null!;
    }
