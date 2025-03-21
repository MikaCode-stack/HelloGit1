CREATE TABLE [Users] (
	[userId] int IDENTITY(1,1) NOT NULL UNIQUE,
	[name] nvarchar(255) NOT NULL DEFAULT '50',
	[email] nvarchar(max) NOT NULL DEFAULT '100',
	[passwordHash] int NOT NULL,
	[accessType] nvarchar(10) NOT NULL DEFAULT '15',
	[createdAt] rowversion NOT NULL DEFAULT GETDATE(),
	PRIMARY KEY ([userId])
);

CREATE TABLE [Videos] (
	[videoID] nvarchar(max) NOT NULL UNIQUE,
	[title] nvarchar(max) DEFAULT '100',
	[genre] nvarchar(max) NOT NULL DEFAULT '50',
	[releaseYear] datetime NOT NULL,
	[price] decimal(18,0) NOT NULL,
	[availableCopies] int NOT NULL,
	PRIMARY KEY ([videoID])
);

CREATE TABLE [Transactions] (
	[transactionID] int IDENTITY(1,1) NOT NULL UNIQUE,
	[customerID] int NOT NULL,
	[videoID] int NOT NULL,
	[price] decimal(18,0) NOT NULL,
	[status] nvarchar(10) NOT NULL DEFAULT ''Active'',
	[rentalDate] date NOT NULL,
	[returnDate] date DEFAULT 'NULL',
	PRIMARY KEY ([transactionID])
);

CREATE TABLE [Payments] (
	[paymentId] int NOT NULL,
	[transactionId] int NOT NULL,
	[customerId] int NOT NULL,
	[amount] decimal(18,0) NOT NULL,
	[paymentDate] date NOT NULL,
	[paymentMethod] nvarchar(max) DEFAULT '50',
	[status] nvarchar(max) NOT NULL DEFAULT ''Success'',
	PRIMARY KEY ([paymentId])
);

CREATE TABLE [Refunds] (
	[refundID] int IDENTITY(1,1) NOT NULL,
	[paymentID] nvarchar(max) NOT NULL DEFAULT '50',
	[customerID] nvarchar(max) NOT NULL DEFAULT '50',
	[refundAmount] date,
	[refundDate] nvarchar(max) DEFAULT '50',
	[status] nvarchar(max) NOT NULL DEFAULT ''Pending'',
	PRIMARY KEY ([refundID])
);

CREATE TABLE [LateFees] (
	[feeID] int NOT NULL,
	[transactionID] int NOT NULL,
	[amount] decimal((10,2)) NOT NULL,
	[paid] bit NOT NULL,
	[feeDate] date,
	PRIMARY KEY ([feeID])
);

CREATE TABLE [Customers_Reviews] (
	[reviewText] nvarchar(max),
	[reviewId] int NOT NULL,
	[customerId] int NOT NULL,
	[videoId] int NOT NULL,
	[rating] int NOT NULL,
	PRIMARY KEY ([reviewId])
);

CREATE TABLE [AdminLogs] (
	[logId] int NOT NULL,
	[adminId] int NOT NULL DEFAULT '50',
	[action] nvarchar(255) NOT NULL,
	[actionDate] rowversion NOT NULL DEFAULT GETDATE(),
	[isActive] bit NOT NULL,
	PRIMARY KEY ([logId])
);



ALTER TABLE [Transactions] ADD CONSTRAINT [Transactions_fk1] FOREIGN KEY ([customerID]) REFERENCES [Users]([userId]);

ALTER TABLE [Transactions] ADD CONSTRAINT [Transactions_fk2] FOREIGN KEY ([videoID]) REFERENCES [Videos]([videoID]);
ALTER TABLE [Payments] ADD CONSTRAINT [Payments_fk1] FOREIGN KEY ([transactionId]) REFERENCES [Transactions]([transactionID]);

ALTER TABLE [Payments] ADD CONSTRAINT [Payments_fk2] FOREIGN KEY ([customerId]) REFERENCES [Users]([userId]);
ALTER TABLE [Refunds] ADD CONSTRAINT [Refunds_fk1] FOREIGN KEY ([paymentID]) REFERENCES [Payments]([paymentId]);

ALTER TABLE [Refunds] ADD CONSTRAINT [Refunds_fk2] FOREIGN KEY ([customerID]) REFERENCES [Users]([userId]);
ALTER TABLE [LateFees] ADD CONSTRAINT [LateFees_fk1] FOREIGN KEY ([transactionID]) REFERENCES [Transactions]([transactionID]);
ALTER TABLE [Customers_Reviews] ADD CONSTRAINT [Customers_Reviews_fk2] FOREIGN KEY ([customerId]) REFERENCES [Users]([userId]);

ALTER TABLE [Customers_Reviews] ADD CONSTRAINT [Customers_Reviews_fk3] FOREIGN KEY ([videoId]) REFERENCES [Videos]([videoID]);
ALTER TABLE [AdminLogs] ADD CONSTRAINT [AdminLogs_fk1] FOREIGN KEY ([adminId]) REFERENCES [Users]([userId]);