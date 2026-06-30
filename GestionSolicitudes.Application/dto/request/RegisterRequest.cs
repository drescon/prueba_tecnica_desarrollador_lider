namespace GestionSolicitudes.Application.dto.request
{
    public record RegisterRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public int RolId { get; init; }
    }
}
