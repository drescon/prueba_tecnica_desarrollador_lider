namespace GestionSolicitudes.Application.Exceptions;

public class InvalidStateTransitionException : Exception
{
    public InvalidStateTransitionException(string message = "La transición de estado no es válida.")
        : base(message)
    {
    }

    public InvalidStateTransitionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
