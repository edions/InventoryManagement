CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Date] DATETIME NULL, 
    [Subtotal] INT NULL, 
    [Cash] INT NULL, 
    [DiscountPercent] INT NULL, 
    [DiscountAmount] DECIMAL(18, 2) NULL, 
    [Total] INT NULL, 
    [Change] DECIMAL(18, 2) NULL, 
    [TransactionId] VARCHAR(MAX) NULL,
)
