-- ==================================================
-- AWE Electronics Database Creation Script
-- SQL Server 2022
-- ==================================================
-- This script creates the complete database schema for the AWE Electronics
-- inventory management system based on the application's DAL layer.
-- ==================================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'AWEElectronics_DB')
BEGIN
    ALTER DATABASE AWEElectronics_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AWEElectronics_DB;
END
GO

-- Create database
CREATE DATABASE AWEElectronics_DB;
GO

USE AWEElectronics_DB;
GO

-- ==================================================
-- Table: Users
-- Stores user accounts for authentication and authorization
-- ==================================================
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Staff', -- Admin, Manager, Staff
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active', -- Active, Inactive, Suspended
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ==================================================
-- Table: Categories
-- Stores product categories with hierarchical structure
-- ==================================================
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ParentCategoryID INT NULL,
    Slug NVARCHAR(100) UNIQUE NOT NULL,
    CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentCategoryID) 
        REFERENCES Categories(CategoryID)
);
GO

-- ==================================================
-- Table: Suppliers
-- Stores supplier/vendor information
-- ==================================================
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(100) NOT NULL,
    ContactName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(500) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active', -- Active, Inactive
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ==================================================
-- Table: Products
-- Stores product information
-- ==================================================
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    CategoryID INT NOT NULL,
    SupplierID INT NULL,
    SKU NVARCHAR(50) UNIQUE NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Specifications NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0,
    StockLevel INT NOT NULL DEFAULT 0,
    IsPublished BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Products_Category FOREIGN KEY (CategoryID) 
        REFERENCES Categories(CategoryID),
    CONSTRAINT FK_Products_Supplier FOREIGN KEY (SupplierID) 
        REFERENCES Suppliers(SupplierID),
    CONSTRAINT CK_Products_Price CHECK (Price >= 0),
    CONSTRAINT CK_Products_StockLevel CHECK (StockLevel >= 0)
);
GO

-- ==================================================
-- Table: Orders
-- Stores sales order headers
-- ==================================================
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(50) UNIQUE NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(100) NULL,
    CustomerPhone NVARCHAR(20) NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Processing, Completed, Cancelled
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedBy INT NOT NULL,
    CONSTRAINT FK_Orders_CreatedBy FOREIGN KEY (CreatedBy) 
        REFERENCES Users(UserID),
    CONSTRAINT CK_Orders_TotalAmount CHECK (TotalAmount >= 0)
);
GO

-- ==================================================
-- Table: OrderDetails
-- Stores line items for each order
-- ==================================================
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderDetails_Order FOREIGN KEY (OrderID) 
        REFERENCES Orders(OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Product FOREIGN KEY (ProductID) 
        REFERENCES Products(ProductID),
    CONSTRAINT CK_OrderDetails_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderDetails_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT CK_OrderDetails_LineTotal CHECK (LineTotal >= 0)
);
GO

-- ==================================================
-- Table: Payments
-- Stores payment transactions
-- ==================================================
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, -- Cash, Card, Transfer, etc.
    ReferenceNumber NVARCHAR(100) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Completed', -- Pending, Completed, Failed, Refunded
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Payments_Order FOREIGN KEY (OrderID) 
        REFERENCES Orders(OrderID),
    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0)
);
GO

-- ==================================================
-- Table: InventoryTransactions
-- Stores all inventory movements (IN, OUT, ADJUST)
-- ==================================================
CREATE TABLE InventoryTransactions (
    TransID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Type NVARCHAR(10) NOT NULL, -- IN, OUT, ADJUST
    Quantity INT NOT NULL,
    ReferenceNumber NVARCHAR(50) NULL,
    PerformedBy INT NOT NULL,
    TransDate DATETIME NOT NULL DEFAULT GETDATE(),
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_InventoryTransactions_Product FOREIGN KEY (ProductID) 
        REFERENCES Products(ProductID),
    CONSTRAINT FK_InventoryTransactions_User FOREIGN KEY (PerformedBy) 
        REFERENCES Users(UserID),
    CONSTRAINT CK_InventoryTransactions_Type CHECK (Type IN ('IN', 'OUT', 'ADJUST'))
);
GO

-- ==================================================
-- INDEXES for Performance
-- ==================================================

-- Users
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Role ON Users(Role);
CREATE INDEX IX_Users_Status ON Users(Status);

-- Categories
CREATE INDEX IX_Categories_ParentID ON Categories(ParentCategoryID);
CREATE INDEX IX_Categories_Slug ON Categories(Slug);

-- Suppliers
CREATE INDEX IX_Suppliers_Status ON Suppliers(Status);

-- Products
CREATE INDEX IX_Products_CategoryID ON Products(CategoryID);
CREATE INDEX IX_Products_SupplierID ON Products(SupplierID);
CREATE INDEX IX_Products_SKU ON Products(SKU);
CREATE INDEX IX_Products_Name ON Products(Name);
CREATE INDEX IX_Products_StockLevel ON Products(StockLevel);

-- Orders
CREATE INDEX IX_Orders_OrderNumber ON Orders(OrderNumber);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_CreatedBy ON Orders(CreatedBy);
CREATE INDEX IX_Orders_CustomerName ON Orders(CustomerName);

-- OrderDetails
CREATE INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID);
CREATE INDEX IX_OrderDetails_ProductID ON OrderDetails(ProductID);

-- Payments
CREATE INDEX IX_Payments_OrderID ON Payments(OrderID);
CREATE INDEX IX_Payments_PaymentDate ON Payments(PaymentDate);
CREATE INDEX IX_Payments_Status ON Payments(Status);

-- InventoryTransactions
CREATE INDEX IX_InventoryTransactions_ProductID ON InventoryTransactions(ProductID);
CREATE INDEX IX_InventoryTransactions_Type ON InventoryTransactions(Type);
CREATE INDEX IX_InventoryTransactions_TransDate ON InventoryTransactions(TransDate);
CREATE INDEX IX_InventoryTransactions_PerformedBy ON InventoryTransactions(PerformedBy);

GO

-- ==================================================
-- SAMPLE DATA
-- ==================================================

-- Insert Default Admin User
-- Username: admin, Password: admin123
-- Password hash is SHA256 of "admin123"
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, Status)
VALUES 
    ('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 
     'System Administrator', 'admin@aweelectronics.com', 'Admin', 'Active'),
    ('manager', 'fd99cc1f6c63570f7c4c0748a2f7f6039c2a56fd1524fa1dcc06dce28d9c1d6e', 
     'Store Manager', 'manager@aweelectronics.com', 'Manager', 'Active'),
    ('staff', 'f57c8e00e0f1e0a9982ae8d4faca0a4f39b6a5f8e51ebf5dd69c8ddfeb4c9d35', 
     'Sales Staff', 'staff@aweelectronics.com', 'Staff', 'Active');
GO

-- Insert Sample Categories
INSERT INTO Categories (Name, ParentCategoryID, Slug)
VALUES 
    ('Electronics', NULL, 'electronics'),
    ('Computer Hardware', 1, 'computer-hardware'),
    ('Smartphones', 1, 'smartphones'),
    ('Audio Equipment', 1, 'audio-equipment'),
    ('CPUs & Processors', 2, 'cpus-processors'),
    ('Graphics Cards', 2, 'graphics-cards'),
    ('Memory (RAM)', 2, 'memory-ram'),
    ('Storage Devices', 2, 'storage-devices'),
    ('Motherboards', 2, 'motherboards'),
    ('Headphones', 4, 'headphones'),
    ('Speakers', 4, 'speakers');
GO

-- Insert Sample Suppliers
INSERT INTO Suppliers (CompanyName, ContactName, Email, Phone, Address, Status)
VALUES 
    ('Tech Distributors Inc', 'John Smith', 'john@techdist.com', '555-0101', '123 Tech Street, CA', 'Active'),
    ('Global Electronics Co', 'Jane Doe', 'jane@globalelec.com', '555-0102', '456 Electronics Ave, NY', 'Active'),
    ('Premium Components Ltd', 'Bob Johnson', 'bob@premiumcomp.com', '555-0103', '789 Component Rd, TX', 'Active');
GO

-- Insert Sample Products
INSERT INTO Products (CategoryID, SupplierID, SKU, Name, Specifications, Price, StockLevel, IsPublished)
VALUES 
    (5, 1, 'CPU-AMD-5600X', 'AMD Ryzen 5 5600X', '6-Core, 12-Thread, 3.7GHz Base, 4.6GHz Boost', 299.99, 25, 1),
    (5, 1, 'CPU-INT-12600K', 'Intel Core i5-12600K', '10-Core, 16-Thread, 3.7GHz Base, 4.9GHz Boost', 289.99, 30, 1),
    (6, 2, 'GPU-NV-3060TI', 'NVIDIA RTX 3060 Ti', '8GB GDDR6, 4864 CUDA Cores', 399.99, 15, 1),
    (6, 2, 'GPU-AMD-6700XT', 'AMD Radeon RX 6700 XT', '12GB GDDR6, 2560 Stream Processors', 479.99, 10, 1),
    (7, 3, 'RAM-COR-16GB', 'Corsair Vengeance 16GB', 'DDR4 3200MHz, 2x8GB Kit', 79.99, 50, 1),
    (7, 3, 'RAM-KIN-32GB', 'Kingston Fury 32GB', 'DDR4 3600MHz, 2x16GB Kit', 149.99, 35, 1),
    (8, 1, 'SSD-SAM-1TB', 'Samsung 980 Pro 1TB', 'NVMe M.2, Read: 7000MB/s, Write: 5000MB/s', 129.99, 40, 1),
    (8, 2, 'SSD-WD-2TB', 'WD Black SN850 2TB', 'NVMe M.2, Read: 7300MB/s, Write: 5300MB/s', 249.99, 20, 1),
    (9, 1, 'MB-ASUS-B550', 'ASUS ROG Strix B550-F', 'AM4 Socket, PCIe 4.0, WiFi 6', 189.99, 18, 1),
    (9, 2, 'MB-MSI-Z690', 'MSI MAG Z690 Tomahawk', 'LGA1700 Socket, DDR4, PCIe 5.0', 279.99, 12, 1),
    (3, 3, 'PHN-SAM-S23', 'Samsung Galaxy S23', '6.1" Display, 128GB, 5G', 799.99, 25, 1),
    (3, 3, 'PHN-APP-14PRO', 'iPhone 14 Pro', '6.1" Display, 256GB, 5G', 1099.99, 15, 1),
    (10, 2, 'AUD-SONY-WH', 'Sony WH-1000XM5', 'Wireless, Noise Cancelling, 30hr Battery', 399.99, 30, 1),
    (11, 3, 'AUD-LOG-Z623', 'Logitech Z623', '200W RMS, 2.1 Speaker System, THX Certified', 149.99, 22, 1);
GO

-- ==================================================
-- VIEWS for Common Queries
-- ==================================================

-- View: Product Inventory Summary
CREATE VIEW vw_ProductInventorySummary AS
SELECT 
    p.ProductID,
    p.SKU,
    p.Name AS ProductName,
    c.Name AS CategoryName,
    s.CompanyName AS SupplierName,
    p.StockLevel,
    p.Price,
    p.IsPublished,
    CASE 
        WHEN p.StockLevel <= 10 THEN 'Low Stock'
        WHEN p.StockLevel <= 20 THEN 'Medium Stock'
        ELSE 'Good Stock'
    END AS StockStatus
FROM Products p
LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID;
GO

-- View: Order Summary
CREATE VIEW vw_OrderSummary AS
SELECT 
    o.OrderID,
    o.OrderNumber,
    o.CustomerName,
    o.OrderDate,
    o.Status,
    o.TotalAmount,
    u.FullName AS CreatedByName,
    COUNT(od.OrderDetailID) AS TotalItems,
    SUM(od.Quantity) AS TotalQuantity
FROM Orders o
LEFT JOIN OrderDetails od ON o.OrderID = od.OrderID
LEFT JOIN Users u ON o.CreatedBy = u.UserID
GROUP BY o.OrderID, o.OrderNumber, o.CustomerName, o.OrderDate, 
         o.Status, o.TotalAmount, u.FullName;
GO

-- ==================================================
-- STORED PROCEDURES
-- ==================================================

-- Procedure: Get Low Stock Products
CREATE PROCEDURE sp_GetLowStockProducts
    @Threshold INT = 10
AS
BEGIN
    SELECT 
        p.ProductID,
        p.SKU,
        p.Name,
        c.Name AS CategoryName,
        p.StockLevel,
        p.Price
    FROM Products p
    LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
    WHERE p.StockLevel <= @Threshold
    ORDER BY p.StockLevel ASC;
END;
GO

-- Procedure: Update Product Stock
CREATE PROCEDURE sp_UpdateProductStock
    @ProductID INT,
    @Quantity INT,
    @Type NVARCHAR(10),
    @PerformedBy INT,
    @Notes NVARCHAR(500) = NULL
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Insert transaction record
        DECLARE @ReferenceNumber NVARCHAR(50);
        SET @ReferenceNumber = CONCAT(@Type, '-', FORMAT(GETDATE(), 'yyyyMMdd'), '-', RIGHT(NEWID(), 4));
        
        INSERT INTO InventoryTransactions (ProductID, Type, Quantity, ReferenceNumber, PerformedBy, Notes)
        VALUES (@ProductID, @Type, @Quantity, @ReferenceNumber, @PerformedBy, @Notes);
        
        -- Update product stock
        IF @Type = 'IN' OR @Type = 'ADJUST'
            UPDATE Products SET StockLevel = StockLevel + @Quantity WHERE ProductID = @ProductID;
        ELSE IF @Type = 'OUT'
            UPDATE Products SET StockLevel = StockLevel - @Quantity WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success, 'Stock updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success, ERROR_MESSAGE() AS Message;
    END CATCH;
END;
GO

-- ==================================================
-- TRIGGERS
-- ==================================================

-- Trigger: Prevent negative stock
CREATE TRIGGER tr_Products_PreventNegativeStock
ON Products
FOR UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE StockLevel < 0)
    BEGIN
        RAISERROR ('Stock level cannot be negative', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- ==================================================
-- COMPLETION MESSAGE
-- ==================================================

PRINT '=================================================='
PRINT 'Database AWEElectronics_DB created successfully!'
PRINT '=================================================='
PRINT ''
PRINT 'Default Users Created:'
PRINT '  Username: admin     Password: admin123    Role: Admin'
PRINT '  Username: manager   Password: manager123  Role: Manager'
PRINT '  Username: staff     Password: staff123    Role: Staff'
PRINT ''
PRINT 'Sample Data:'
PRINT '  - 3 Users'
PRINT '  - 11 Categories'
PRINT '  - 3 Suppliers'
PRINT '  - 14 Products'
PRINT ''
PRINT 'Database is ready for use!'
PRINT '=================================================='
GO

-- Test connection
SELECT 
    'Database Ready!' AS Status,
    DB_NAME() AS DatabaseName,
    @@VERSION AS SQLServerVersion,
    GETDATE() AS CurrentDateTime;
GO