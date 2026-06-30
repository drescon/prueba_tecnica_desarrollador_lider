
using Microsoft.EntityFrameworkCore;
using GestionSolicitudes.Infrastructure.Persistence;
using GestionSolicitudes.Infrastructure.Entities;
using GestionSolicitudes.Application.Dto.Request;
using GestionSolicitudes.Application.Dto.Response;
using GestionSolicitudes.Application.Exceptions;

namespace GestionSolicitudes.Application.Solicitudes
{
    public class SolicitudService
    {
        private readonly PruebaTecnicaDbContext _context;

        public SolicitudService(PruebaTecnicaDbContext context)
        {
            _context = context;
        }

       
        public async Task<CreateSolicitudResponse> CrearSolicitudAsync(CreateSolicitudRequest request, int usuarioId)
        {
            // Validar que el usuario exista y esté activo
            ValidarUsuarioActivo(usuarioId);

            // Validar que el tipo de solicitud exista
            var tipoSolicitud = await _context.TipoSolicitudes.FindAsync(request.TipoId);
            if (tipoSolicitud == null)
            {
                throw new EntityNotFoundException("Tipo de solicitud no válido");
            }

            // Validar que no exista una solicitud duplicada del mismo tipo para el mismo usuario el mismo día
            var hoy = DateTime.Now.Date;
            var solicitudDuplicada = await _context.Solicitudes
                .Where(s => s.UsuarioId == usuarioId &&
                            s.TipoId == request.TipoId &&
                            s.FechaSolicitud.Date == hoy)
                .FirstOrDefaultAsync();

            if (solicitudDuplicada != null)
            {
                throw new DuplicateRequestException($"Ya existe una solicitud del tipo '{tipoSolicitud.Nombre}' para hoy del mismo usuario");
            }

            // Obtener el estado "Pendiente"
            var estadoPendiente = await _context.Estados
                .Where(e => e.Nombre.ToLower() == "pendiente")
                .FirstOrDefaultAsync();

            if (estadoPendiente == null)
            {
                throw new EntityNotFoundException("Estado 'Pendiente' no encontrado en la base de datos");
            }

            // Generar número de solicitud único
            var numeroSolicitud = GenerarNumeroSolicitud();

            // Crear la nueva solicitud
            var nuevaSolicitud = new Solicitud
            {
                Numero = numeroSolicitud,
                FechaSolicitud = request.FechaSolicitud ?? DateTime.Now,
                UsuarioId = usuarioId,
                TipoId = request.TipoId,
                EstadoId = estadoPendiente.Id,
                Observaciones = request.Observaciones,
                FechaCreacion = DateTime.Now
            };

            _context.Solicitudes.Add(nuevaSolicitud);
            await _context.SaveChangesAsync();

            // Crear el seguimiento inicial
            var seguimientoInicial = new Seguimiento
            {
                SolicitudId = nuevaSolicitud.Id,
                UsuarioId = usuarioId,
                FechaSeguimiento = DateTime.Now,
                EstadoId = estadoPendiente.Id,
                Comentario = "Solicitud creada",
                FechaCreacion = DateTime.Now
            };

            _context.Seguimientos.Add(seguimientoInicial);
            await _context.SaveChangesAsync();

            return new CreateSolicitudResponse
            {
                SolicitudNumber = numeroSolicitud,
                Message = $"Solicitud creada exitosamente con número {numeroSolicitud}"
            };
        }

    
    
  
        public async Task<SolicitudDetailResponse> ObtenerSolicitudAsync(string numeroSolicitud)
        {
            var solicitudDto = await _context.Solicitudes
                .Where(s => s.Numero == numeroSolicitud)
                .Select(s => new SolicitudDetailResponse
                {
                    Id = s.Id,
                    Numero = s.Numero,
                    FechaSolicitud = s.FechaSolicitud,
                    Observaciones = s.Observaciones,
                    UsuarioNombre = s.Usuario.NombreUsuario,
                    EstadoNombre = s.Estado.Nombre,
                    TipoNombre = s.TipoSolicitud.Nombre,
                    Seguimientos = s.Seguimientos
                        .Select(se => new SeguimientoDto
                        {
                            EstadoNombre = se.Estado.Nombre,
                            FechaSeguimiento = se.FechaSeguimiento,
                            UsuarioNombre = se.Usuario.NombreUsuario,
                            Comentario = se.Comentario
                        })
                        .OrderBy(se => se.FechaSeguimiento)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (solicitudDto == null)
            {
                throw new EntityNotFoundException("Solicitud no encontrada");
            }

            return solicitudDto;
        }

    
        public async Task<Seguimiento> CambiarEstadoAsync(int solicitudId, int nuevoEstadoId, string comentario, int usuarioId)
        {
            ValidarUsuarioActivo(usuarioId);
            var solicitud = await _context.Solicitudes.FindAsync(solicitudId);

            
            if (solicitud == null)
            {
                throw new EntityNotFoundException("Solicitud no encontrada");
            }

            var nuevoEstado = await _context.Estados.FindAsync(nuevoEstadoId);
            if (nuevoEstado == null)
            {
                throw new EntityNotFoundException("Estado no válido");
            }

            // Validar que no se cierre sin observación
            if (nuevoEstado.Nombre.ToLower() == "cerrado" && string.IsNullOrWhiteSpace(comentario))
            {
                throw new MissingObservationException("No se puede cerrar una solicitud sin observación");
            }

            // Actualizar estado
            solicitud.EstadoId = nuevoEstadoId;
            solicitud.FechaModificacion = DateTime.Now;

            // Crear seguimiento
            var seguimiento = new Seguimiento
            {
                SolicitudId = solicitudId,
                UsuarioId = usuarioId,
                FechaSeguimiento = DateTime.Now,
                EstadoId = nuevoEstadoId,
                Comentario = comentario,
                FechaCreacion = DateTime.Now
            };

            _context.Seguimientos.Add(seguimiento);
            _context.Solicitudes.Update(solicitud);
            await _context.SaveChangesAsync();

            return seguimiento;
        }

        //Helpers

         private string GenerarNumeroSolicitud()
        {
            var timestamp = DateTime.Now.Ticks;
            var numeroAleatorio = Random.Shared.Next(1000, 9999);
            return $"{DateTime.Now:yyyyMMdd}{timestamp % 10000:D4}{numeroAleatorio}";
        }

        private void ValidarUsuarioActivo(int usuarioId)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                throw new EntityNotFoundException("Usuario no encontrado");
            }

            if (!usuario.Activo)
            {
                throw new UserNotActiveException("Solo usuarios activos pueden realizar esta acción");
            }
        }


    }
}