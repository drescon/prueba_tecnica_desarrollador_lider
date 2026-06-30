
using GestionSolicitudes.Infrastructure.Persistence;
using GestionSolicitudes.Application.dto.request;
using GestionSolicitudes.Application.dto.response;
using GestionSolicitudes.Application.Exceptions;
using GestionSolicitudes.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

namespace GestionSolicitudes.Application.Auth
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly PruebaTecnicaDbContext _dbContext;

        public AuthService(TokenService tokenService, PruebaTecnicaDbContext dbContext)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Validate credentials are provided
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new MissingCredentialsException();
            }

            // Retrieve user from database
            Usuario? user = await _dbContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == request.Username);

            if (user == null || !BCryptNet.Verify(request.Password, user.Contrasenia))
            {
                throw new InvalidCredentialsException("Usuario o contraseña inválidos.");
            }

            if (!user.Activo)
            {
                throw new UserNotActiveException($"Usuario  '{request.Username}' esta bloqueado.");
            }

            // Generate and return token
            return _tokenService.GenerateToken(user.NombreUsuario, user.Rol.Nombre);
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            // Validate credentials are provided
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new MissingCredentialsException("Username and password are required for registration.");
            }

            // Check if username already exists
            var existingUser = await _dbContext.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == request.Username);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException($"User '{request.Username}' already exists.");
            }

            // Verify the role exists
            var role = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Id == request.RolId);

            if (role == null)
            {
                throw new RoleNotFoundException($"Role with ID {request.RolId} not found.");
            }

            // Create new user
            var newUser = new Usuario
            {
                NombreUsuario = request.Username,
                Contrasenia = BCryptNet.HashPassword(request.Password),
                RolId = request.RolId,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _dbContext.Usuarios.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new RegisterResponse
            {
                UserId = newUser.Id,
                Username = newUser.NombreUsuario,
                Message = $"User '{newUser.NombreUsuario}' registered successfully."
            };
        }
    }
}
