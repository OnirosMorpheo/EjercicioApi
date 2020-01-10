CREATE TABLE [dbo].[Trazas] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Uid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Traza_Uid1] DEFAULT (newid()) NOT NULL,
    [UidPeticion] UNIQUEIDENTIFIER CONSTRAINT [DF_Traza_Uid] DEFAULT (newid()) NOT NULL,
    [CreadoEn]    DATETIME         CONSTRAINT [DF_Traza_CreadoEn] DEFAULT (getutcdate()) NOT NULL,
    [Dll]         NVARCHAR (4000)  NULL,
    [EsExcepcion] BIT              CONSTRAINT [DF_Traza_ExExcepcion] DEFAULT ((0)) NOT NULL,
    [Descripcion] NVARCHAR (MAX)   NULL,
    [FullName]    NVARCHAR (4000)  NULL,
    [Name]        NVARCHAR (4000)  NULL,
    [Namespace]   NVARCHAR (4000)  NULL,
    [Nick]        NVARCHAR (4000)  NULL,
    [Parametros]  NVARCHAR (MAX)   NULL,
    [Nivel]       INT              CONSTRAINT [DF_Traza_Nivel] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_Traza] PRIMARY KEY CLUSTERED ([Id] ASC)
);
