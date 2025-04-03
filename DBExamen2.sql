USE master;
GO

-- Verificar si la base de datos existe y eliminarla si es necesario
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BDExamen2')
BEGIN
    ALTER DATABASE BDExamen2 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BDExamen2;
END
GO

-- Crear la base de datos
CREATE DATABASE BDExamen2;
GO

-- Usar la base de datos recién creada
USE BDExamen2;
GO

-- Crear la tabla Class
CREATE TABLE PartialClass (
    IdClass INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO

-- Crear la tabla Imagenes con una clave foránea a Class
CREATE TABLE PartialImage (
    IdImagen INT IDENTITY(1,1) PRIMARY KEY,
    NombreImagen NVARCHAR(255) NOT NULL,
    IdClass INT NOT NULL,
    CONSTRAINT FK_Imagenes_Class FOREIGN KEY (idClass) REFERENCES PartialClass(IdClass)
);
GO


INSERT INTO PartialClass (Name)  
VALUES 
	('Matemáticas'),
    ('Historia'),  
    ('Ciencias'),  
    ('Lengua Española'),  
    ('Física'); 