namespace GestionSolicitudes.Application.Exceptions;

public class MissingObservationException : Exception
{
    public MissingObservationException(string message = "Las observaciones son requeridas para esta operación.")
        : base(message)
    {
    }

    public MissingObservationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
