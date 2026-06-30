

namespace GestionSolicitudes.Application.Dto.Response
{
    public partial record CreateSolicitudResponse
    {
        public string SolicitudNumber { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}