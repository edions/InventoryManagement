CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(50) NULL, 
    [Price] INT NULL, 
    [Stock] INT NULL, 
    [Unit] INT NULL, 
    [Category] VARCHAR(50) NULL
)
