using System;
using System.Collections.Generic;

namespace GestionSolicitudes.Application.Dto.Response
{
    public record SeguimientoDto
    {
        public string EstadoNombre { get; init; } = string.Empty;
        public DateTime FechaSeguimiento { get; init; }
        public string UsuarioNombre { get; init; } = string.Empty;
        public string Comentario { get; init; } = string.Empty;
    }

    public record SolicitudDetailResponse
    {
        public int Id { get; init; }
        public string Numero { get; init; } = string.Empty;
        public DateTime FechaSolicitud { get; init; }
        public string Observaciones { get; init; } = string.Empty;
        public string UsuarioNombre { get; init; } = string.Empty;
        public string EstadoNombre { get; init; } = string.Empty;

        public string TipoNombre { get; init; } = string.Empty;
        public List<SeguimientoDto> Seguimientos { get; init; } = new();
    }
}
