CREATE DATABASE prueba_tecnica;


CREATE TABLE [Roles] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [nombre] varchar(50) UNIQUE,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

INSERT INTO [Roles] ([nombre], [fecha_creacion], [fecha_modificacion])
VALUES 
    ('Administrador', GETDATE(), NULL),
    ('Usuario', GETDATE(), NULL),
    ('Auditor', GETDATE(), NULL),
    ('Soporte', GETDATE(), NULL);
GO

CREATE TABLE [Usuarios] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [nombre_usuario] varchar(100) UNIQUE,
  [contrasenia] varchar(100) ,
  [rol_id] int,
  [activo] bit,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

CREATE TABLE [Estados] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [nombre] varchar(50) UNIQUE,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

INSERT INTO [Estados] ([nombre], [fecha_creacion], [fecha_modificacion])
VALUES 
    ('Pendiente', GETDATE(), NULL),
    ('En Proceso', GETDATE(), NULL),
    ('Aprobado', GETDATE(), NULL),
    ('Rechazado', GETDATE(), NULL),
    ('Cerrado', GETDATE(), NULL);
GO

CREATE TABLE [TipoSolicitud] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [nombre] varchar(50) UNIQUE,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

INSERT INTO [TipoSolicitud] ([nombre], [fecha_creacion], [fecha_modificacion])
VALUES 
    ('Soporte Técnico', GETDATE(), NULL),
    ('Solicitud de Acceso', GETDATE(), NULL),
    ('Requerimiento de Software', GETDATE(), NULL),
    ('Reporte de Incidencia', GETDATE(), NULL),
    ('Otros', GETDATE(), NULL);
GO

CREATE TABLE [Solicitudes] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [numero] varchar(50) UNIQUE,
  [fecha_solicitud] datetime DEFAULT (GETDATE()),
  [usuario_id] int,
  [tipo_id] int,
  [estado_id] int,
  [observaciones] text not null,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

CREATE TABLE [Seguimientos] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [solicitud_id] int,
  [usuario_id] int,
  [fecha_seguimiento] datetime DEFAULT (GETDATE()),
  [estado_id] int,
  [comentario] text,
  [fecha_creacion] datetime DEFAULT (GETDATE()),
  [fecha_modificacion] datetime
)
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Ej: Administrador, Soporte, Solicitante',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Roles',
@level2type = N'Column', @level2name = 'nombre';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Define los permisos del usuario en el sistema',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Usuarios',
@level2type = N'Column', @level2name = 'rol_id';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '1 = Activo, 0 = Inactivo',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Usuarios',
@level2type = N'Column', @level2name = 'activo';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Ej: Pendiente, En Proceso, Cerrado',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Estados',
@level2type = N'Column', @level2name = 'nombre';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Fecha de negocio de la solicitud',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Solicitudes',
@level2type = N'Column', @level2name = 'fecha_solicitud';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Inicia referenciando al ID de "Pendiente"',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Solicitudes',
@level2type = N'Column', @level2name = 'estado_id';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Requerido para pasar al estado Cerrado',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Solicitudes',
@level2type = N'Column', @level2name = 'observaciones';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Fecha del cambio de estado',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Seguimientos',
@level2type = N'Column', @level2name = 'fecha_seguimiento';
GO

ALTER TABLE [Usuarios] ADD FOREIGN KEY ([rol_id]) REFERENCES [Roles] ([id])
GO

ALTER TABLE [Solicitudes] ADD FOREIGN KEY ([usuario_id]) REFERENCES [Usuarios] ([id])
GO

ALTER TABLE [Solicitudes] ADD FOREIGN KEY ([estado_id]) REFERENCES [Estados] ([id])
GO

ALTER TABLE [Solicitudes] ADD FOREIGN KEY ([tipo_id]) REFERENCES [TipoSolicitud] ([id])
GO

ALTER TABLE [Seguimientos] ADD FOREIGN KEY ([solicitud_id]) REFERENCES [Solicitudes] ([id])
GO

ALTER TABLE [Seguimientos] ADD FOREIGN KEY ([usuario_id]) REFERENCES [Usuarios] ([id])
GO

ALTER TABLE [Seguimientos] ADD FOREIGN KEY ([estado_id]) REFERENCES [Estados] ([id])
GO



