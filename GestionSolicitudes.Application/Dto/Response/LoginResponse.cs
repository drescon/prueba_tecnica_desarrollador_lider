
namespace GestionSolicitudes.Application.Dto.Response
{
    public partial record LoginResponse
    {
        public string Token { get; init; } = string.Empty;
        public DateTime Expiration { get; init; } = DateTime.MinValue;
    }
}