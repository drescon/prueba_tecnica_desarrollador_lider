namespace GestionSolicitudes.Application.Exceptions
{
    public class UserNotActiveException : Exception
    {
        public UserNotActiveException(string message = "User account is not active.")
            : base(message)
        {
        }

        public UserNotActiveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
