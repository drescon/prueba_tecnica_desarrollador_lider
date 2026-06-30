namespace GestionSolicitudes.Application.dto.response
{
    public record RegisterResponse
    {
        public int UserId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}
