# Prueba Técnica CasaToro - Gestión de Solicitudes

Este proyecto es una API REST construida con .NET 8 y SQL Server, diseñada para gestionar solicitudes y sus seguimientos.

## Arquitectura

- Arquitectura basada en capas:
  - `GestionSolicitudes.Api`: capa de presentación / API.
  - `GestionSolicitudes.Application`: lógica de negocio y casos de uso.
  - `GestionSolicitudes.Infrastructure`: acceso a datos y entidades.
- La aplicación está pensada como una API, por lo que no incluye interfaz de usuario. Las peticiones se realizan mediante Swagger o cualquier cliente HTTP.

## Tecnologías

- .NET 8
- SQL Server
- Entity Framework Core
- JWT para seguridad y autenticación
- Swagger ya configurado para probar endpoints

## Seguridad

- Se utiliza JWT (JSON Web Token) para proteger los endpoints.
- El token incluye claims de usuario, identificador y rol.
- Los endpoints están protegidos con autenticación y autorización de ASP.NET Core.

## Uso

1. Configura la cadena de conexión a SQL Server en `GestionSolicitudes.Api/appsettings.json`.
2. Ejecuta la API:

```bash
dotnet run --project GestionSolicitudes.Api/GestionSolicitudes.Api.csproj
```

3. Abre Swagger en el navegador cuando la aplicación esté en ejecución. Normalmente estará en:

```text
http://localhost:5052/swagger/index.html
```

4. Usa Swagger para hacer las peticiones a los endpoints.

## Endpoints principales

- `POST /api/auth/login` — autenticación de usuario.
- `POST /api/auth/register` — registro de usuario.
- `POST /api/solicitudes` — crear nueva solicitud.
- `GET /api/solicitudes/{numeroSolicitud}` — obtener información de una solicitud.
- `PUT /api/solicitudes/{solicitudId}/estado` — cambiar estado de una solicitud.

## Notas

- La API devuelve datos específicos y usa DTOs para evitar serializar entidades completas.
- La protección de endpoints se realiza con JWT y roles.
