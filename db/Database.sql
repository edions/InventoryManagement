-- Table to store product information
CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(50) NULL, 
    [Price] INT NULL, 
    [Stock] INT NULL, 
    [Unit] INT NULL, 
    [Category] VARCHAR(50) NULL
);

-- Table to store category information
CREATE TABLE [dbo].[Category]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [CategoryItem] VARCHAR(50) NULL,
);

-- Table to track the history of stock updates
CREATE TABLE [dbo].[History]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProductID] INT NULL, 
    [Added Stocks] INT NULL, 
    [Date] DATETIME NULL,
);

-- Table to store items in the shopping cart
CREATE TABLE [dbo].[Cart]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(50) NULL, 
    [Price] INT NULL, 
    [Quantity] INT NULL,
    [ProductId] INT NULL
);

-- Table to store transaction information
CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Date] DATETIME NULL, 
    [Subtotal] VARCHAR(50) NULL, 
    [Cash] VARCHAR(50) NULL, 
    [DiscountPercent] VARCHAR(50) NULL, 
    [DiscountAmount] VARCHAR(50) NULL, 
    [Total] VARCHAR(50) NULL, 
    [Change] VARCHAR(50) NULL, 
    [TransactionId] VARCHAR(MAX) NULL,
);

-- Table to store ordered items in a transaction
CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [TransactionId] VARCHAR(MAX) NULL, 
    [Name] VARCHAR(50) NULL, 
    [Price] VARCHAR(50) NULL, 
    [Quantity] INT NULL
);

-- Creates a table to store user information
CREATE TABLE [dbo].[Users]
(
	[Uid] INT NOT NULL PRIMARY KEY IDENTITY (1000, 1), 
    [Username] VARCHAR(50) NULL, 
    [Password] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL,
);

