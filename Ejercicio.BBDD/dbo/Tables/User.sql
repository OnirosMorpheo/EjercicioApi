CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] VARCHAR(100),
	[Birthdate] DATETIME,
	[DeleteDate]  DATETIME	NULL,
    [CreatedBy]     NVARCHAR (4000)   DEFAULT '-' NOT NULL,
    [CreatedDate]      DATETIME         DEFAULT getutcdate() NULL,
    [UpdateBy] NVARCHAR (4000)   DEFAULT '-' NULL,
    [UpdatedDate]  DATETIME         DEFAULT getutcdate() NULL
)
