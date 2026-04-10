USE LeonarditoBD;
GO

-- Crear tabla Usuario si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuario' AND xtype='U')
BEGIN
    CREATE TABLE Usuario (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(50)  NOT NULL,
        Correo      NVARCHAR(100) NOT NULL UNIQUE,
        Contrasena  NVARCHAR(100) NOT NULL,
        FechaRegistro DATETIME   NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Tabla Usuario creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Usuario ya existe.';
END
GO