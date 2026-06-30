
namespace GestionSolicitudes.Application.Dto.Request
{
    public partial record LoginRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}