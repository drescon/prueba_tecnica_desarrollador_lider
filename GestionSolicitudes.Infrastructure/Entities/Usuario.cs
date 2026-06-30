using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitudes.Infrastructure.Entities;

    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string NombreUsuario { get; set; } = null!;

        [MaxLength(100)]
        public string Contrasenia { get; set; } = null!;
        
        public int RolId { get; set; }
        public bool Activo { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey(nameof(RolId))]
        public virtual Rol Rol { get; set; } = null!;
        
        public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
        public virtual ICollection<Seguimiento> Seguimientos { get; set; } = new List<Seguimiento>();
    }