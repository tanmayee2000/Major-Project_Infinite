
Create Database E_TradingDB
use E_TradingDB



---------------------- HINTS TABLE -----------------
Create table Hints 
(Hint_Id  int Primary Key Identity ,
Questions NVarchar (100))

Insert Into Hints (Questions) Values 
('What is your Favourite Sport?'),
('In what city were you born?'),
('What is the name of your first pet?'),
('What is your favorite movie?'),
('What street did you grow up on?');

select * from Hints

---------------------- ADMIN TABLE ---------------------

Create table [Admin] 
(Admin_Id numeric(10) Primary key not null,
Admin_Email varchar(30) unique not null,
Admin_Name varchar(30) not null, 
[Password] varchar(20) unique not null,
Hint_Id int foreign key references Hints(Hint_Id),
[Hint_Answers] varchar (200) not null)

Insert Into Admin (Admin_Id, Admin_Email, Admin_Name, [Password], Hint_Id, Hint_Answers)
Values (111, 'admin@gmail.com', 'Admin', 'adm#123', 1, 'Volleyball')

Select * from Admin

---------------------- CUSTOMER TABLE -----------------------

Create Table Customer 
([Customer_Id] numeric(10) Primary key IDENTITY,
[Customer_Name] varchar(30) not null,
[Customer_Email] varchar(30) unique not null,
Date_Of_Birth Date not null,
[Address] varchar(50) not null,
Balance float default 0,
Mobile_Number numeric(10),
[Password] varchar(20) not null,
[Hint_Id] int foreign key references Hints(Hint_Id),
[Hint_Answer] varchar(200) not null,
[Status] varchar(20))

Insert Into Customer (Customer_Name, Customer_Email, Date_Of_Birth, Address, Balance, Mobile_Number, [Password], Hint_Id, Hint_Answer, [Status])
Values 
('Raghu Reddy', 'raghu@gmail.com', '2002-03-24', '#12,Kundanahalli', 10000.00, 9263547890, 'rrr#123',4, 'Hint answer 4', 'Active'),
('Abhishek', 'abhi@gamil.com', '2000-09-30', '#004,Jubli hills', 5000.00, 9634258796, 'abhi@123', 5, 'Hint answer 5','Active'),
('Raviteja', 'ravi@gamil.com', '2001-11-30', '#15B,Secunderabad', 1000.00, 9126534278, 'ravi@777', 5, 'Hint answer 5','Active')


select * from Customer


---------------------- WALLET TABLE --------------------

Create Table Wallet (
wallet_Id int primary key identity,
[Customer_Id] numeric(10) foreign key references Customer(Customer_Id),
[Date_of_Top_Up] date,
[Last_Top_Up] float,
[Total_Top_Up] float
)
 
---------------------- VENDORS TABLE --------------------

Create Table Vendors (
Vendor_Id Numeric(10) Primary Key IDENTITY,
Vendor_Name varchar(20) not null,
Vendor_Email varchar(30) unique not null,
Mobile_Number Numeric(10) not null,
[Address] varchar(50) not  null,
Category varchar(40) not null,
Vendor_Age int not null,
[Passowrd] varchar(20) not null,
[Hint_Id] int foreign key references Hints(Hint_Id),
[Hint_Answer] nvarchar(50),
[Status] varchar(20)
)

 INSERT INTO Vendors (Vendor_Name, Vendor_Email, Mobile_Number, [Address], Category, Vendor_Age, [Passowrd], Hint_Id, Hint_Answer, [Status])
VALUES 
('Vendor1', 'ven1@gmail.com', 9165482764, '#2nd street, Mysore', 'Shoes', 37, 'pwd#123', 1, 'Hint answer 1', 'Active'),

select * from Vendors

---------------------- PRODUCTS TABLE ---------------------

Create Table Products
(Product_Id numeric(10) primary key,
Vendor_Id numeric(10) foreign key references Vendors(Vendor_Id),
Product_Name varchar(40),
Brand varchar(30),
Color varchar(20),
Price float,
Available_Stock int,
[Status] varchar(25))

ALTER TABLE Products
ADD ImageFileName varchar(100);

Select * from Products

---------------------- BUCKET LIST (CART) TABLE ---------------------

CREATE TABLE BucketList(
    Serial_Number int IDENTITY,
    Product_Id numeric(10) NOT NULL,
    Customer_Id numeric(10) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    CONSTRAINT PK_BucketList PRIMARY KEY (Serial_Number),
    CONSTRAINT FK_BucketList_Products FOREIGN KEY (Product_Id) REFERENCES Products(Product_Id),
    CONSTRAINT FK_BucketList_Customer FOREIGN KEY (Customer_Id) REFERENCES Customer(Customer_Id)
)



----------------------This is the table for the Orders ----------------

CREATE TABLE Orders (
    Purchase_Id INT PRIMARY KEY IDENTITY(1,1),
    Customer_Id NUMERIC(10) FOREIGN KEY REFERENCES Customer(Customer_Id),
    Delivery_Date DATE,
    Order_Amount FLOAT,
    Payment_Mode VARCHAR(20),
    Address VARCHAR(50),
    Status VARCHAR(30)
)


----------------------This is the table for the Order Details ----------------

CREATE TABLE Order_Details (
    Order_Id INT PRIMARY KEY IDENTITY(1,1),
    Purchase_Id INT FOREIGN KEY REFERENCES Orders(Purchase_Id),
    Product_Name VARCHAR(30),
    Product_Price FLOAT
);
ALTER TABLE Order_Details
ADD Product_Id NUMERIC(10) FOREIGN KEY REFERENCES Products(Product_Id);
ALTER TABLE Order_Details
ADD Quantity INT

  
----------------------This is the table for the Order_Cancellation----------------

CREATE TABLE Order_Cancellation (
    Cancellation_Id INT PRIMARY KEY IDENTITY(1,1),
    Order_Id INT FOREIGN KEY REFERENCES Order_Details(Order_Id),
    Cancellation_Date DATETIME NOT NULL,
    Refund_Amount NUMERIC(8)
);

 
----------------------------------------------------------------------------------
 

select * from Admin
Select * from Customer
select * from Vendors
select * from Products
select * from Order_Details
select * from Order_Cancellation
select * from Orders
select * from Wallet
select * from BucketList
select * from Hints
select * from Orders

----------------------------------------------------------------------------------

------ STORED PROCEDURES -------

CREATE OR ALTER PROCEDURE GetProductsByVendorId
    @VendorId DECIMAL
AS
BEGIN
    SELECT p.Product_Id,
           p.Product_Name,
           p.Brand,
           p.Color,
           p.Price,
           p.Available_Stock,
           p.Status,
           p.ImageFileName,
           p.isdeleted,
           v.Vendor_Name
    FROM Products p
    INNER JOIN Vendors v ON p.Vendor_Id = v.Vendor_Id
    WHERE p.Vendor_Id = @VendorId;
END;


CREATE OR ALTER PROCEDURE InsertIntoBucketList
    @Product_Id NUMERIC(10),
    @Customer_Id NUMERIC(10)
AS
BEGIN
    INSERT INTO BucketList (Product_Id, Customer_Id)
    VALUES (@Product_Id, @Customer_Id);
END;


CREATE OR ALTER PROCEDURE PlaceOrder
    @Customer_Id NUMERIC(10),
    @Product_Id NUMERIC(10),
    @Quantity INT,
    @Delivery_Date DATE,
    @Payment_Mode VARCHAR(20),
    @Address VARCHAR(50)
AS
BEGIN
    -- Insert into Orders table
    INSERT INTO Orders (Customer_Id, Delivery_Date, Order_Amount, Payment_Mode, Address, Status)
    VALUES (@Customer_Id, @Delivery_Date, 
            (SELECT Price FROM Products WHERE Product_Id = @Product_Id) * @Quantity, 
            @Payment_Mode, @Address, 'Pending');

    DECLARE @Purchase_Id INT;
    SET @Purchase_Id = SCOPE_IDENTITY(); -- Get the ID of the last inserted order

    -- Insert into Order_Details table
    INSERT INTO Order_Details (Purchase_Id, Product_Id, Product_Name, Product_Price)
    VALUES (@Purchase_Id, @Product_Id, 
            (SELECT Product_Name FROM Products WHERE Product_Id = @Product_Id), 
            (SELECT Price FROM Products WHERE Product_Id = @Product_Id) * @Quantity);

    -- Reduce the available stock in Products table
    UPDATE Products SET Available_Stock = Available_Stock - @Quantity WHERE Product_Id = @Product_Id;
END;


CREATE OR ALTER PROCEDURE CancelOrder
    @Order_Id INT,
    @Cancellation_Date DATETIME,
    @Refund_Amount NUMERIC(8)
AS
BEGIN
    -- Insert into Order_Cancellation table
    INSERT INTO Order_Cancellation (Order_Id, Cancellation_Date, Refund_Amount)
    VALUES (@Order_Id, @Cancellation_Date, @Refund_Amount);

    -- Update status of the order
    UPDATE Orders SET Status = 'Canceled' WHERE Purchase_Id = @Order_Id;

    -- Increase the available stock in Products table
    DECLARE @Product_Id NUMERIC(10);
    SET @Product_Id = (SELECT Product_Id FROM Order_Details WHERE Purchase_Id = @Order_Id);

    DECLARE @Quantity INT;
    SET @Quantity = (SELECT Quantity FROM Order_Details WHERE Purchase_Id = @Order_Id);

    UPDATE Products SET Available_Stock = Available_Stock + @Quantity WHERE Product_Id = @Product_Id;
END;



CREATE PROCEDURE GetCustomerIdByEmail
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT Customer_Id
    FROM Customer
    WHERE Customer_Email = @Email;
END;
