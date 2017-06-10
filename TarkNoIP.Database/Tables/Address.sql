CREATE TABLE [dbo].[Address]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Address] VARCHAR(15) NOT NULL, 
    [ServerId] INT NOT NULL, 
    [CreationDate] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_ServerAddress_ToServer] FOREIGN KEY ([ServerId]) REFERENCES [Server]([Id])
)
