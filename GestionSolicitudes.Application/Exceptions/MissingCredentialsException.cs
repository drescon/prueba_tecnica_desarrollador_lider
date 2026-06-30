namespace GestionSolicitudes.Application.Exceptions
{
    public class MissingCredentialsException : Exception
    {
        public MissingCredentialsException(string message = "Username and password are required.")
            : base(message)
        {
        }

        public MissingCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
