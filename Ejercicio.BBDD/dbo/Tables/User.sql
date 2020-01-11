CREATE TABLE [dbo].[User] (
    [Id]          INT              NOT NULL IDENTITY(1,1),
    [Uid]         UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (100)    NULL,
    [Birthdate]   DATETIME         NULL,
    [DeleteDate]  DATETIME         NULL,
    [CreatedBy]   NVARCHAR (4000)  DEFAULT ('-') NOT NULL,
    [CreatedDate] DATETIME         DEFAULT (getutcdate()) NULL,
    [UpdateBy]    NVARCHAR (4000)  DEFAULT ('-') NULL,
    [UpdatedDate] DATETIME         DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
