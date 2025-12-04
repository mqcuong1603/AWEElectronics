-- AWE Electronics Sample Data Population Script
-- This script adds realistic sample orders and related data

USE AWEElectronics_DB;
GO

-- Step 1: Insert Sample Orders
PRINT 'Inserting sample orders...';

-- Order 1: Recent order (today)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-001', 'John Doe', 'john.doe@email.com', '555-0101', GETDATE(), 'Pending', 959.97, 1);

-- Order 2: Processing (yesterday)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-002', 'Jane Smith', 'jane.smith@email.com', '555-0102', DATEADD(DAY, -1, GETDATE()), 'Processing', 1549.96, 1);

-- Order 3: Shipped (3 days ago)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-003', 'Bob Johnson', 'bob.j@email.com', '555-0103', DATEADD(DAY, -3, GETDATE()), 'Shipped', 699.98, 1);

-- Order 4: Delivered (1 week ago)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-004', 'Alice Williams', 'alice.w@email.com', '555-0104', DATEADD(DAY, -7, GETDATE()), 'Delivered', 1289.95, 1);

-- Order 5: Delivered (2 weeks ago)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-005', 'Charlie Brown', 'charlie.b@email.com', '555-0105', DATEADD(DAY, -14, GETDATE()), 'Delivered', 459.99, 1);

-- Order 6: Delivered (1 month ago)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-006', 'Diana Prince', 'diana.p@email.com', '555-0106', DATEADD(MONTH, -1, GETDATE()), 'Delivered', 879.98, 1);

-- Order 7: Cancelled (2 days ago)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-007', 'Eve Adams', 'eve.a@email.com', '555-0107', DATEADD(DAY, -2, GETDATE()), 'Cancelled', 299.99, 1);

-- Order 8: Today
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-008', 'Frank Miller', 'frank.m@email.com', '555-0108', GETDATE(), 'Pending', 159.98, 1);

-- Order 9: Last month
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-009', 'Grace Lee', 'grace.l@email.com', '555-0109', DATEADD(MONTH, -1, GETDATE()), 'Delivered', 2199.94, 1);

-- Order 10: Today (high value)
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, OrderDate, Status, TotalAmount, CreatedBy)
VALUES 
('ORD-2025-010', 'Henry Ford', 'henry.f@email.com', '555-0110', GETDATE(), 'Processing', 1799.95, 1);

PRINT 'Sample orders inserted successfully!';
GO

-- Step 2: Insert Order Details (line items)
PRINT 'Inserting order details...';

-- Order 1 details (3 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(1, 1, 1, 299.99, 299.99),  -- AMD Ryzen 5
(1, 5, 2, 79.99, 159.98),    -- RAM x2
(1, 10, 1, 499.99, 499.99);  -- Monitor

-- Order 2 details (4 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(2, 2, 1, 289.99, 289.99),   -- Intel CPU
(2, 3, 1, 399.99, 399.99),   -- RTX 3060 Ti
(2, 6, 1, 129.99, 129.99),   -- SSD
(2, 8, 1, 729.99, 729.99);   -- Motherboard

-- Order 3 details (2 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(3, 4, 1, 479.99, 479.99),   -- AMD GPU
(3, 7, 1, 219.99, 219.99);   -- PSU

-- Order 4 details (3 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(4, 1, 1, 299.99, 299.99),
(4, 3, 1, 399.99, 399.99),
(4, 9, 1, 589.97, 589.97);

-- Order 5 details (1 product)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(5, 11, 1, 459.99, 459.99);

-- Order 6 details (2 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(6, 12, 2, 439.99, 879.98);

-- Order 7 details (1 product - cancelled)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(7, 1, 1, 299.99, 299.99);

-- Order 8 details (2 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(8, 5, 2, 79.99, 159.98);

-- Order 9 details (5 products - large order)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(9, 2, 1, 289.99, 289.99),
(9, 3, 1, 399.99, 399.99),
(9, 6, 1, 129.99, 129.99),
(9, 8, 1, 729.99, 729.99),
(9, 10, 1, 649.98, 649.98);

-- Order 10 details (3 products)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice)
VALUES 
(10, 4, 1, 479.99, 479.99),
(10, 13, 1, 869.98, 869.98),
(10, 14, 1, 449.98, 449.98);

PRINT 'Order details inserted successfully!';
GO

-- Step 3: Insert Sample Payments
PRINT 'Inserting sample payments...';

-- Payments for delivered and processing orders
INSERT INTO Payments (OrderID, PaymentDate, PaymentMethod, Amount, Status, TransactionReference)
VALUES 
(2, DATEADD(DAY, -1, GETDATE()), 'Credit Card', 1549.96, 'Completed', 'TXN-CC-20250101-001'),
(3, DATEADD(DAY, -3, GETDATE()), 'PayPal', 699.98, 'Completed', 'TXN-PP-20250103-001'),
(4, DATEADD(DAY, -7, GETDATE()), 'Credit Card', 1289.95, 'Completed', 'TXN-CC-20250107-001'),
(5, DATEADD(DAY, -14, GETDATE()), 'Debit Card', 459.99, 'Completed', 'TXN-DC-20250114-001'),
(6, DATEADD(MONTH, -1, GETDATE()), 'Bank Transfer', 879.98, 'Completed', 'TXN-BT-20241201-001'),
(9, DATEADD(MONTH, -1, GETDATE()), 'Credit Card', 2199.94, 'Completed', 'TXN-CC-20241215-001'),
(10, GETDATE(), 'Credit Card', 1799.95, 'Pending', 'TXN-CC-20250115-001');

PRINT 'Sample payments inserted successfully!';
GO

-- Step 4: Verify Data
PRINT '';
PRINT '========================================';
PRINT 'Data Population Summary';
PRINT '========================================';

SELECT 'Orders' as TableName, COUNT(*) as RecordCount FROM Orders;
SELECT 'Order Details' as TableName, COUNT(*) as RecordCount FROM OrderDetails;
SELECT 'Payments' as TableName, COUNT(*) as RecordCount FROM Payments;

PRINT '';
PRINT 'Orders by Status:';
SELECT Status, COUNT(*) as OrderCount, SUM(TotalAmount) as TotalRevenue
FROM Orders
GROUP BY Status
ORDER BY Status;

PRINT '';
PRINT 'Revenue Summary:';
SELECT 
    COUNT(*) as TotalOrders,
    SUM(TotalAmount) as TotalRevenue,
    AVG(TotalAmount) as AverageOrderValue,
    MIN(TotalAmount) as SmallestOrder,
    MAX(TotalAmount) as LargestOrder
FROM Orders;

PRINT '';
PRINT '========================================';
PRINT 'Sample data population complete!';
PRINT 'You can now refresh your dashboard to see the data.';
PRINT '========================================';
GO
