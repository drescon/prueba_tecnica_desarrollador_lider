using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionSolicitudes.Application.Dto.Response
{
    public partial record CreateSolicitudResponse
    {
        public int SolicitudNumber { get; init; } = 0;
        public string Message { get; init; } = string.Empty;
    }
}