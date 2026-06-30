using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GestionSolicitudes.Application.Dto.Request;
using GestionSolicitudes.Application.Solicitudes;
using GestionSolicitudes.Application.Exceptions;

namespace GestionSolicitudes.Api.Controllers;

[ApiController]
[Route("api/solicitudes")]
[Authorize]
public class SolicitudController : ControllerBase
{
    private readonly SolicitudService _solicitudService;

    public SolicitudController(SolicitudService solicitudService)
    {
        _solicitudService = solicitudService ?? throw new ArgumentNullException(nameof(solicitudService));
    }

   
    [HttpPost]
    public async Task<IActionResult> CrearSolicitud([FromBody] CreateSolicitudRequest request)
    {
        var usuarioId = GetUsuarioIdFromToken();
        var resultado = await _solicitudService.CrearSolicitudAsync(request, usuarioId);
        return CreatedAtAction(nameof(ObtenerSolicitud), new { solicitudId = resultado.SolicitudNumber }, resultado);
    }

 
    [HttpGet("{solicitudId}")]
    public async Task<IActionResult> ObtenerSolicitud(int solicitudId)
    {
        var solicitud = await _solicitudService.ObtenerSolicitudAsync(solicitudId);
        return Ok(solicitud);
    }

    [HttpPut("{solicitudId}/estado")]
    public async Task<IActionResult> CambiarEstado(int solicitudId, [FromBody] CambiarEstadoRequest request)
    {
        var usuarioId = GetUsuarioIdFromToken();
        var seguimiento = await _solicitudService.CambiarEstadoAsync(
            solicitudId, 
            request.EstadoId, 
            request.Comentario, 
            usuarioId
        );

        return Ok(new { mensaje = "Estado actualizado exitosamente", seguimiento });
    }

    //helpers
    private int GetUsuarioIdFromToken()
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int usuarioId))
        {
            throw new EntityNotFoundException("No se pudo obtener el ID del usuario del token");
        }
        return usuarioId;
    }

}



