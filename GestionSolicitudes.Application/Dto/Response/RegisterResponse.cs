namespace GestionSolicitudes.Application.Dto.Response
{
    public record RegisterResponse
    {
        public int UserId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}
