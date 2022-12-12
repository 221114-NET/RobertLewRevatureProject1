CREATE TABLE Employees (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	FName varchar(255),
	LName varchar(255),
	Email varchar(255),
	Password varchar(255),
	IsManager bit NOT NULL DEFAULT 0
)

CREATE TABLE ExpenseReports(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	Creator UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Employees(Id),
	Type varchar(255),
	Amount float,
	Description varchar(255),
	Status varchar(8) DEFAULT 'Pending'
)
