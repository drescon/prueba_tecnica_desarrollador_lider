namespace GestionSolicitudes.Application.Exceptions;

public class DuplicateRequestException : Exception
{
    public DuplicateRequestException(string message = "La solicitud ya existe.")
        : base(message)
    {
    }

    public DuplicateRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
