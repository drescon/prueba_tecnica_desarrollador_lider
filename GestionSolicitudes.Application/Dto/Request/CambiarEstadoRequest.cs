using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionSolicitudes.Application.Dto.Request
{
    public class CambiarEstadoRequest
    {
        public int EstadoId { get; set; }
        public string Comentario { get; set; } = string.Empty;
    }
}