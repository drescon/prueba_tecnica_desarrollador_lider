using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitudes.Infrastructure.Entities;
    
    [Table("Estados")]
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
        public virtual ICollection<Seguimiento> Seguimientos { get; set; } = new List<Seguimiento>();
    }