-- ============================================================================
-- AWE Electronics Online Store System - Complete Web Database Schema
-- Database: AWEElectronics_DB
-- MSSQL Server 2019+
-- Version: 2.0 (Web-Optimized)
-- Generated: January 2025
-- Authors: Ma Quoc Cuong (522I0001) & Tran Huu Van (522K0014)
-- ============================================================================
-- This schema includes full e-commerce functionality:
-- - Customer accounts and authentication
-- - Shopping cart system
-- - Multiple shipping addresses
-- - Order management and tracking
-- - Payment processing
-- - Inventory management
-- ============================================================================

USE master;
GO

-- Drop database if exists (for clean setup)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'AWEElectronics_DB')
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

-- ============================================================================
-- TABLE CREATION
-- ============================================================================

-- Table: Users (Staff/Admin authentication)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Manager', 'Staff', 'Accountant', 'Agent')),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive', 'Locked')),
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME NULL
);
GO

-- Table: Categories
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    ParentCategoryID INT NULL,
    Slug NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500) NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);
GO

-- Table: Suppliers
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(200) NOT NULL,
    ContactName NVARCHAR(100),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive')),
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table: Products
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    CategoryID INT NOT NULL,
    SupplierID INT NULL,
    SKU NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(200) NOT NULL,
    Specifications NVARCHAR(MAX) NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    StockLevel INT NOT NULL DEFAULT 0 CHECK (StockLevel >= 0),
    MinStockLevel INT DEFAULT 5,
    IsPublished BIT DEFAULT 1,
    ImageURL NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);
GO

-- Table: Customers
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NULL,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    DefaultShippingAddressID INT NULL,
    IsActive BIT DEFAULT 1,
    RegisteredDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME NULL,
    EmailVerified BIT DEFAULT 0
);
GO

-- Table: Addresses
CREATE TABLE Addresses (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    AddressLine1 NVARCHAR(255) NOT NULL,
    AddressLine2 NVARCHAR(255),
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    PostalCode NVARCHAR(20),
    Country NVARCHAR(50) NOT NULL DEFAULT 'Australia',
    Type NVARCHAR(20) NOT NULL CHECK (Type IN ('Shipping', 'Billing', 'Both')),
    IsDefault BIT DEFAULT 0,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE
);
GO

-- Add foreign key for DefaultShippingAddressID after Addresses table is created
ALTER TABLE Customers
ADD FOREIGN KEY (DefaultShippingAddressID) REFERENCES Addresses(AddressID);
GO

-- Table: Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerID INT NOT NULL,
    StaffCheckedID INT NULL,
    ShippingAddressID INT NOT NULL,
    SubTotal DECIMAL(18,2) NOT NULL,
    Tax DECIMAL(18,2) NOT NULL,
    ShippingCost DECIMAL(18,2) DEFAULT 0,
    GrandTotal DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled', 'Refunded')),
    OrderDate DATETIME DEFAULT GETDATE(),
    ShippedDate DATETIME NULL,
    DeliveredDate DATETIME NULL,
    TrackingNumber NVARCHAR(100) NULL,
    Notes NVARCHAR(MAX) NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (StaffCheckedID) REFERENCES Users(UserID),
    FOREIGN KEY (ShippingAddressID) REFERENCES Addresses(AddressID)
);
GO

-- Table: OrderDetails (Line items for orders)
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Discount DECIMAL(18,2) DEFAULT 0,
    LineTotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Table: CartItems (Shopping cart for customers)
CREATE TABLE CartItems (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    AddedAt DATETIME DEFAULT GETDATE(),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (CustomerID, ProductID)
);
GO

-- Table: InventoryTransactions (Track stock movements)
CREATE TABLE InventoryTransactions (
    TransID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Type NVARCHAR(10) NOT NULL CHECK (Type IN ('IN', 'OUT', 'ADJUST')),
    Quantity INT NOT NULL,
    ReferenceNumber NVARCHAR(50),
    PerformedBy INT NOT NULL,
    TransDate DATETIME DEFAULT GETDATE(),
    Notes NVARCHAR(MAX),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (PerformedBy) REFERENCES Users(UserID)
);
GO

-- Table: Payments
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Completed', 'Failed', 'Refunded')),
    TransactionRef NVARCHAR(100),
    PaidAt DATETIME NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);
GO

-- ============================================================================
-- INDEXES FOR PERFORMANCE
-- ============================================================================

-- Users
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
CREATE INDEX IX_Users_Status ON Users(Status);

-- Categories
CREATE INDEX IX_Categories_ParentID ON Categories(ParentCategoryID);
CREATE INDEX IX_Categories_Slug ON Categories(Slug);

-- Products
CREATE INDEX IX_Products_CategoryID ON Products(CategoryID);
CREATE INDEX IX_Products_SupplierID ON Products(SupplierID);
CREATE INDEX IX_Products_SKU ON Products(SKU);
CREATE INDEX IX_Products_Name ON Products(Name);
CREATE INDEX IX_Products_IsPublished ON Products(IsPublished);
CREATE INDEX IX_Products_Price ON Products(Price);

-- Customers
CREATE INDEX IX_Customers_Email ON Customers(Email);
CREATE INDEX IX_Customers_IsActive ON Customers(IsActive);

-- Orders
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_Orders_OrderNumber ON Orders(OrderNumber);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_Orders_Status ON Orders(Status);

-- OrderDetails
CREATE INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID);
CREATE INDEX IX_OrderDetails_ProductID ON OrderDetails(ProductID);

-- CartItems
CREATE INDEX IX_CartItems_CustomerID ON CartItems(CustomerID);
CREATE INDEX IX_CartItems_ProductID ON CartItems(ProductID);

-- InventoryTransactions
CREATE INDEX IX_InventoryTransactions_ProductID ON InventoryTransactions(ProductID);
CREATE INDEX IX_InventoryTransactions_Type ON InventoryTransactions(Type);
CREATE INDEX IX_InventoryTransactions_TransDate ON InventoryTransactions(TransDate);

-- Payments
CREATE INDEX IX_Payments_OrderID ON Payments(OrderID);
CREATE INDEX IX_Payments_Status ON Payments(Status);
CREATE INDEX IX_Payments_PaidAt ON Payments(PaidAt);

GO

-- ============================================================================
-- VIEWS FOR COMMON QUERIES
-- ============================================================================

-- View: Product Catalog (for web display)
CREATE VIEW vw_ProductCatalog AS
SELECT 
    p.ProductID,
    p.SKU,
    p.Name,
    p.Description,
    p.Price,
    p.StockLevel,
    p.ImageURL,
    p.IsPublished,
    c.Name AS CategoryName,
    c.Slug AS CategorySlug,
    s.CompanyName AS SupplierName,
    CASE 
        WHEN p.StockLevel = 0 THEN 'Out of Stock'
        WHEN p.StockLevel <= p.MinStockLevel THEN 'Low Stock'
        ELSE 'In Stock'
    END AS StockStatus
FROM Products p
LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
WHERE p.IsPublished = 1;
GO

-- View: Order Summary with Customer Details
CREATE VIEW vw_OrderSummary AS
SELECT 
    o.OrderID,
    o.OrderNumber,
    o.OrderDate,
    o.Status,
    o.GrandTotal,
    c.FullName AS CustomerName,
    c.Email AS CustomerEmail,
    c.Phone AS CustomerPhone,
    u.FullName AS ProcessedBy,
    COUNT(od.OrderDetailID) AS TotalItems,
    SUM(od.Quantity) AS TotalQuantity
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
LEFT JOIN OrderDetails od ON o.OrderID = od.OrderID
GROUP BY o.OrderID, o.OrderNumber, o.OrderDate, o.Status, o.GrandTotal,
         c.FullName, c.Email, c.Phone, u.FullName;
GO

-- View: Low Stock Products
CREATE VIEW vw_LowStockProducts AS
SELECT 
    p.ProductID,
    p.SKU,
    p.Name,
    c.Name AS CategoryName,
    p.StockLevel,
    p.MinStockLevel,
    p.Price,
    s.CompanyName AS SupplierName,
    s.Email AS SupplierEmail,
    s.Phone AS SupplierPhone
FROM Products p
LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
WHERE p.StockLevel <= p.MinStockLevel AND p.IsPublished = 1;
GO

-- View: Customer Shopping Cart
CREATE VIEW vw_CustomerCart AS
SELECT 
    c.CartID,
    c.CustomerID,
    cu.FullName AS CustomerName,
    c.ProductID,
    p.Name AS ProductName,
    p.SKU,
    p.Price AS UnitPrice,
    c.Quantity,
    (p.Price * c.Quantity) AS LineTotal,
    p.StockLevel,
    p.ImageURL,
    c.AddedAt
FROM CartItems c
INNER JOIN Customers cu ON c.CustomerID = cu.CustomerID
INNER JOIN Products p ON c.ProductID = p.ProductID
WHERE p.IsPublished = 1;
GO

-- ============================================================================
-- STORED PROCEDURES
-- ============================================================================

-- Procedure: Add Product to Cart
CREATE PROCEDURE sp_AddToCart
    @CustomerID INT,
    @ProductID INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if product exists and is available
    IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductID = @ProductID AND IsPublished = 1)
    BEGIN
        SELECT 0 AS Success, 'Product not available' AS Message;
        RETURN;
    END
    
    -- Check if enough stock
    DECLARE @StockLevel INT;
    SELECT @StockLevel = StockLevel FROM Products WHERE ProductID = @ProductID;
    
    IF @StockLevel < @Quantity
    BEGIN
        SELECT 0 AS Success, 'Insufficient stock' AS Message;
        RETURN;
    END
    
    -- Add or update cart item
    IF EXISTS (SELECT 1 FROM CartItems WHERE CustomerID = @CustomerID AND ProductID = @ProductID)
    BEGIN
        UPDATE CartItems 
        SET Quantity = Quantity + @Quantity, ModifiedAt = GETDATE()
        WHERE CustomerID = @CustomerID AND ProductID = @ProductID;
    END
    ELSE
    BEGIN
        INSERT INTO CartItems (CustomerID, ProductID, Quantity)
        VALUES (@CustomerID, @ProductID, @Quantity);
    END
    
    SELECT 1 AS Success, 'Added to cart' AS Message;
END;
GO

-- Procedure: Update Product Stock
CREATE PROCEDURE sp_UpdateProductStock
    @ProductID INT,
    @Quantity INT,
    @Type NVARCHAR(10),
    @PerformedBy INT,
    @Notes NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Insert transaction record
        DECLARE @ReferenceNumber NVARCHAR(50);
        SET @ReferenceNumber = CONCAT(@Type, '-', FORMAT(GETDATE(), 'yyyyMMdd'), '-', RIGHT(NEWID(), 8));
        
        INSERT INTO InventoryTransactions (ProductID, Type, Quantity, ReferenceNumber, PerformedBy, Notes)
        VALUES (@ProductID, @Type, @Quantity, @ReferenceNumber, @PerformedBy, @Notes);
        
        -- Update product stock
        IF @Type = 'IN'
            UPDATE Products SET StockLevel = StockLevel + @Quantity, ModifiedDate = GETDATE() WHERE ProductID = @ProductID;
        ELSE IF @Type = 'OUT'
            UPDATE Products SET StockLevel = StockLevel - @Quantity, ModifiedDate = GETDATE() WHERE ProductID = @ProductID;
        ELSE IF @Type = 'ADJUST'
            UPDATE Products SET StockLevel = @Quantity, ModifiedDate = GETDATE() WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success, 'Stock updated successfully' AS Message, @ReferenceNumber AS ReferenceNumber;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success, ERROR_MESSAGE() AS Message;
    END CATCH;
END;
GO

-- Procedure: Create Order from Cart
CREATE PROCEDURE sp_CreateOrderFromCart
    @CustomerID INT,
    @ShippingAddressID INT,
    @StaffID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Check if cart has items
        IF NOT EXISTS (SELECT 1 FROM CartItems WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 0 AS Success, 'Cart is empty' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Calculate totals
        DECLARE @SubTotal DECIMAL(18,2), @Tax DECIMAL(18,2), @GrandTotal DECIMAL(18,2);
        
        SELECT @SubTotal = SUM(p.Price * c.Quantity)
        FROM CartItems c
        INNER JOIN Products p ON c.ProductID = p.ProductID
        WHERE c.CustomerID = @CustomerID;
        
        SET @Tax = @SubTotal * 0.10; -- 10% GST
        SET @GrandTotal = @SubTotal + @Tax;
        
        -- Generate order number
        DECLARE @OrderNumber NVARCHAR(50);
        SET @OrderNumber = CONCAT('ORD-', FORMAT(GETDATE(), 'yyyyMMdd'), '-', RIGHT(NEWID(), 8));
        
        -- Create order
        DECLARE @OrderID INT;
        INSERT INTO Orders (OrderNumber, CustomerID, StaffCheckedID, ShippingAddressID, SubTotal, Tax, GrandTotal, Status)
        VALUES (@OrderNumber, @CustomerID, @StaffID, @ShippingAddressID, @SubTotal, @Tax, @GrandTotal, 'Pending');
        
        SET @OrderID = SCOPE_IDENTITY();
        
        -- Create order details from cart
        INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, LineTotal)
        SELECT @OrderID, c.ProductID, p.Price, c.Quantity, (p.Price * c.Quantity)
        FROM CartItems c
        INNER JOIN Products p ON c.ProductID = p.ProductID
        WHERE c.CustomerID = @CustomerID;
        
        -- Clear cart
        DELETE FROM CartItems WHERE CustomerID = @CustomerID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success, 'Order created successfully' AS Message, @OrderID AS OrderID, @OrderNumber AS OrderNumber;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success, ERROR_MESSAGE() AS Message;
    END CATCH;
END;
GO

-- ============================================================================
-- TRIGGERS
-- ============================================================================

-- Trigger: Prevent negative stock
CREATE TRIGGER tr_Products_PreventNegativeStock
ON Products
AFTER UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE StockLevel < 0)
    BEGIN
        RAISERROR ('Stock level cannot be negative', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- Trigger: Update modified date on product update
CREATE TRIGGER tr_Products_UpdateModifiedDate
ON Products
AFTER UPDATE
AS
BEGIN
    UPDATE Products
    SET ModifiedDate = GETDATE()
    FROM Products p
    INNER JOIN inserted i ON p.ProductID = i.ProductID
    WHERE p.ProductID = i.ProductID;
END;
GO

-- ============================================================================
-- SAMPLE DATA INSERTION
-- ============================================================================

-- Insert Users (Staff and Admin)
-- Password hash for 'admin123'
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, Status) VALUES
('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'System Administrator', 'admin@aweelectronics.com', 'Admin', 'Active'),
('manager', 'fd99cc1f6c63570f7c4c0748a2f7f6039c2a56fd1524fa1dcc06dce28d9c1d6e', 'Store Manager', 'manager@aweelectronics.com', 'Manager', 'Active'),
('staff', 'f57c8e00e0f1e0a9982ae8d4faca0a4f39b6a5f8e51ebf5dd69c8ddfeb4c9d35', 'Sales Staff', 'staff@aweelectronics.com', 'Staff', 'Active'),
('accountant', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Finance Manager', 'accountant@aweelectronics.com', 'Accountant', 'Active'),
('agent', '7c4a8d09ca3762af61e59520943dc26494f8941b', 'Support Agent', 'agent@aweelectronics.com', 'Agent', 'Active');
GO

-- Insert Categories
INSERT INTO Categories (Name, ParentCategoryID, Slug, Description, IsActive) VALUES
('Electronics', NULL, 'electronics', 'All electronic devices and components', 1),
('Computer Hardware', 1, 'computer-hardware', 'Desktop and laptop components', 1),
('Smartphones', 1, 'smartphones', 'Mobile phones and accessories', 1),
('Audio Equipment', 1, 'audio-equipment', 'Headphones, speakers, and audio devices', 1),
('Gaming', 1, 'gaming', 'Gaming consoles and accessories', 1),
('CPUs & Processors', 2, 'cpus-processors', 'Central processing units', 1),
('Graphics Cards', 2, 'graphics-cards', 'Video cards and GPUs', 1),
('Memory (RAM)', 2, 'memory-ram', 'Computer memory modules', 1),
('Storage Devices', 2, 'storage-devices', 'SSDs, HDDs, and storage solutions', 1),
('Motherboards', 2, 'motherboards', 'PC motherboards', 1),
('Headphones', 4, 'headphones', 'Over-ear and in-ear headphones', 1),
('Speakers', 4, 'speakers', 'Desktop and home speakers', 1);
GO

-- Insert Suppliers
INSERT INTO Suppliers (CompanyName, ContactName, Email, Phone, Address, Status) VALUES
('Tech Distributors Inc', 'John Smith', 'john@techdist.com', '555-0101', '123 Tech Street, CA', 'Active'),
('Global Electronics Co', 'Jane Doe', 'jane@globalelec.com', '555-0102', '456 Electronics Ave, NY', 'Active'),
('Premium Components Ltd', 'Bob Johnson', 'bob@premiumcomp.com', '555-0103', '789 Component Rd, TX', 'Active'),
('Digital World Suppliers', 'Alice Williams', 'alice@digitalworld.com', '555-0104', '321 Digital Blvd, FL', 'Active');
GO

-- Insert Products
INSERT INTO Products (CategoryID, SupplierID, SKU, Name, Specifications, Description, Price, StockLevel, MinStockLevel, IsPublished) VALUES
(6, 1, 'CPU-AMD-5600X', 'AMD Ryzen 5 5600X', '6-Core, 12-Thread, 3.7GHz Base, 4.6GHz Boost', 'High-performance desktop processor with excellent gaming capabilities', 299.99, 25, 5, 1),
(6, 1, 'CPU-INT-12600K', 'Intel Core i5-12600K', '10-Core, 16-Thread, 3.7GHz Base, 4.9GHz Boost', 'Latest generation Intel processor for gaming and productivity', 289.99, 30, 5, 1),
(7, 2, 'GPU-NV-3060TI', 'NVIDIA RTX 3060 Ti', '8GB GDDR6, 4864 CUDA Cores', 'Excellent 1440p gaming performance with ray tracing', 399.99, 15, 3, 1),
(7, 2, 'GPU-AMD-6700XT', 'AMD Radeon RX 6700 XT', '12GB GDDR6, 2560 Stream Processors', 'Powerful graphics card for high-resolution gaming', 479.99, 10, 3, 1),
(8, 3, 'RAM-COR-16GB', 'Corsair Vengeance 16GB', 'DDR4 3200MHz, 2x8GB Kit', 'High-performance memory for gaming and multitasking', 79.99, 50, 10, 1),
(8, 3, 'RAM-KIN-32GB', 'Kingston Fury 32GB', 'DDR4 3600MHz, 2x16GB Kit', 'Premium memory kit for demanding applications', 149.99, 35, 8, 1),
(9, 1, 'SSD-SAM-1TB', 'Samsung 980 Pro 1TB', 'NVMe M.2, Read: 7000MB/s, Write: 5000MB/s', 'Ultra-fast SSD for gaming and content creation', 129.99, 40, 8, 1),
(9, 2, 'SSD-WD-2TB', 'WD Black SN850 2TB', 'NVMe M.2, Read: 7300MB/s, Write: 5300MB/s', 'Top-tier storage for professionals', 249.99, 20, 5, 1),
(10, 1, 'MB-ASUS-B550', 'ASUS ROG Strix B550-F', 'AM4 Socket, PCIe 4.0, WiFi 6', 'Feature-rich motherboard for AMD processors', 189.99, 18, 4, 1),
(10, 2, 'MB-MSI-Z690', 'MSI MAG Z690 Tomahawk', 'LGA1700 Socket, DDR4, PCIe 5.0', 'High-end motherboard for Intel 12th gen', 279.99, 12, 3, 1),
(3, 3, 'PHN-SAM-S23', 'Samsung Galaxy S23', '6.1" Display, 128GB, 5G', 'Flagship Android smartphone with excellent camera', 799.99, 25, 5, 1),
(3, 3, 'PHN-APP-14PRO', 'iPhone 14 Pro', '6.1" Display, 256GB, 5G', 'Premium iPhone with ProRAW and ProRes', 1099.99, 15, 3, 1),
(11, 2, 'AUD-SONY-WH', 'Sony WH-1000XM5', 'Wireless, Noise Cancelling, 30hr Battery', 'Industry-leading noise cancellation headphones', 399.99, 30, 6, 1),
(12, 3, 'AUD-LOG-Z623', 'Logitech Z623', '200W RMS, 2.1 Speaker System, THX Certified', 'Powerful speaker system for immersive audio', 149.99, 22, 5, 1),
(5, 4, 'GAM-PS5-STD', 'PlayStation 5 Standard', '825GB SSD, 4K Gaming, DualSense Controller', 'Next-gen gaming console from Sony', 499.99, 8, 2, 1),
(5, 4, 'GAM-XBX-SER', 'Xbox Series X', '1TB SSD, 4K 120fps, Game Pass Compatible', 'Microsoft flagship gaming console', 499.99, 10, 2, 1);
GO

-- Insert Customers
-- Password hash for 'customer123'
INSERT INTO Customers (Email, PasswordHash, FullName, Phone, IsActive, EmailVerified) VALUES
('alice.nguyen@email.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Alice Nguyen', '+61 412 345 678', 1, 1),
('robert.taylor@email.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Robert Taylor', '+61 423 456 789', 1, 1),
('emma.davis@email.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Emma Davis', '+61 434 567 890', 1, 1),
('michael.wang@email.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Michael Wang', '+61 445 678 901', 1, 1),
('sophia.martinez@email.com', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Sophia Martinez', '+61 456 789 012', 1, 1);
GO

-- Insert Addresses
INSERT INTO Addresses (CustomerID, AddressLine1, AddressLine2, City, State, PostalCode, Country, Type, IsDefault) VALUES
(1, '12 Smith Street', 'Apartment 5', 'Melbourne', 'VIC', '3000', 'Australia', 'Both', 1),
(2, '45 Queen Street', NULL, 'Brisbane', 'QLD', '4000', 'Australia', 'Shipping', 1),
(2, '78 King Avenue', 'Unit 10', 'Brisbane', 'QLD', '4001', 'Australia', 'Billing', 0),
(3, '123 George Street', NULL, 'Sydney', 'NSW', '2000', 'Australia', 'Both', 1),
(4, '67 Collins Street', 'Suite 200', 'Melbourne', 'VIC', '3001', 'Australia', 'Both', 1),
(5, '89 Elizabeth Street', NULL, 'Sydney', 'NSW', '2001', 'Australia', 'Shipping', 1);
GO

-- Update Customers with DefaultShippingAddressID
UPDATE Customers SET DefaultShippingAddressID = 1 WHERE CustomerID = 1;
UPDATE Customers SET DefaultShippingAddressID = 2 WHERE CustomerID = 2;
UPDATE Customers SET DefaultShippingAddressID = 4 WHERE CustomerID = 3;
UPDATE Customers SET DefaultShippingAddressID = 5 WHERE CustomerID = 4;
UPDATE Customers SET DefaultShippingAddressID = 6 WHERE CustomerID = 5;
GO

-- ============================================================================
-- COMPLETION MESSAGE
-- ============================================================================

PRINT '=================================================='
PRINT 'Database AWEElectronics_DB created successfully!'
PRINT '=================================================='
PRINT ''
PRINT 'WEB STORE VERSION - Full E-Commerce Functionality'
PRINT ''
PRINT 'Default Staff Users Created:'
PRINT '  Username: admin       Password: admin123      Role: Admin'
PRINT '  Username: manager     Password: manager123    Role: Manager'
PRINT '  Username: staff       Password: staff123      Role: Staff'
PRINT '  Username: accountant  Password: accountant123 Role: Accountant'
PRINT '  Username: agent       Password: agent123      Role: Agent'
PRINT ''
PRINT 'Default Customer Users:'
PRINT '  Email: alice.nguyen@email.com    Password: customer123'
PRINT '  Email: robert.taylor@email.com   Password: customer123'
PRINT '  Email: emma.davis@email.com      Password: customer123'
PRINT ''
PRINT 'Sample Data:'
PRINT '  - 5 Staff Users'
PRINT '  - 12 Categories'
PRINT '  - 4 Suppliers'
PRINT '  - 16 Products'
PRINT '  - 5 Customers with Addresses'
PRINT ''
PRINT 'Views Created:'
PRINT '  - vw_ProductCatalog'
PRINT '  - vw_OrderSummary'
PRINT '  - vw_LowStockProducts'
PRINT '  - vw_CustomerCart'
PRINT ''
PRINT 'Stored Procedures:'
PRINT '  - sp_AddToCart'
PRINT '  - sp_UpdateProductStock'
PRINT '  - sp_CreateOrderFromCart'
PRINT ''
PRINT 'Database is ready for web deployment!'
PRINT '=================================================='
GO

-- Test connection
SELECT 
    'Web Store Database Ready!' AS Status,
    DB_NAME() AS DatabaseName,
    @@VERSION AS SQLServerVersion,
    GETDATE() AS CurrentDateTime;
GO
