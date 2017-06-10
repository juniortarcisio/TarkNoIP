CREATE TABLE [dbo].[Server]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(20) NOT NULL, 
    [Description] VARCHAR(100) NOT NULL, 
    [ServiceId] INT NOT NULL, 
	[LastKeepAlive] DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [FK_Server_ToService] FOREIGN KEY ([ServiceId]) REFERENCES [Service]([Id])
)

GO

CREATE INDEX [IX_Server_Name] ON [dbo].[Server] ([Name])
