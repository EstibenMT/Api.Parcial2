USE master;
GO

-- Verificar si la base de datos existe y eliminarla si es necesario
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BDExamen2')
BEGIN
    ALTER DATABASE BDExamen2 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BDExamen2;
END
GO

CREATE DATABASE BDExamen2;
GO

USE [BDExamen2]
GO
/****** Object:  Table [dbo].[Camion]    Script Date: 4/3/2025 5:24:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Camion](
	[Placa] [varchar](10) NOT NULL,
	[Marca] [varchar](50) NOT NULL,
	[NumeroEjes] [int] NOT NULL,
 CONSTRAINT [PK_Camion] PRIMARY KEY CLUSTERED 
(
	[Placa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FotoPesaje]    Script Date: 4/3/2025 5:24:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FotoPesaje](
	[idFotoPesaje] [int] IDENTITY(1,1) NOT NULL,
	[ImagenVehiculo] [varchar](50) NOT NULL,
	[idPesaje] [int] NOT NULL,
 CONSTRAINT [PK_FotoPesaje] PRIMARY KEY CLUSTERED 
(
	[idFotoPesaje] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pesaje]    Script Date: 4/3/2025 5:24:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pesaje](
	[id] [int] NOT NULL,
	[FechaPesaje] [date] NOT NULL,
	[PlacaCamion] [varchar](10) NOT NULL,
	[Peso] [real] NOT NULL,
	[Estacion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Pesaje] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FotoPesaje]  WITH CHECK ADD  CONSTRAINT [FK_FotoPesaje_Pesaje] FOREIGN KEY([idPesaje])
REFERENCES [dbo].[Pesaje] ([id])
GO
ALTER TABLE [dbo].[FotoPesaje] CHECK CONSTRAINT [FK_FotoPesaje_Pesaje]
GO
ALTER TABLE [dbo].[Pesaje]  WITH CHECK ADD  CONSTRAINT [FK_Pesaje_Camion] FOREIGN KEY([PlacaCamion])
REFERENCES [dbo].[Camion] ([Placa])
GO
ALTER TABLE [dbo].[Pesaje] CHECK CONSTRAINT [FK_Pesaje_Camion]
GO
