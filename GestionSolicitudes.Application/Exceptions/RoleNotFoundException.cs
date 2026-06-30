namespace GestionSolicitudes.Application.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException(string message = "Role not found.")
            : base(message)
        {
        }

        public RoleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
