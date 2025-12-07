-- =============================================
-- FIX User Passwords with CORRECT SHA256 Hashes
-- =============================================
USE AWEElectronics_DB;
GO

PRINT '====================================================';
PRINT 'Fixing user passwords with CORRECT SHA256 hashes...';
PRINT '====================================================';
PRINT '';

-- admin is already correct, but update anyway
UPDATE Users
SET PasswordHash = '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9'
WHERE Username = 'admin';
PRINT 'Updated admin -> admin123';

-- Fix staff users with CORRECT hash
UPDATE Users
SET PasswordHash = '10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6'
WHERE Username IN ('jsmith', 'mjones', 'slee');
PRINT 'Updated jsmith, mjones, slee -> staff123';

-- Fix agent user with CORRECT hash
UPDATE Users
SET PasswordHash = 'f44d1ac9bf0c69b083380b86dbdf3b73797150e3cca4820ac399f7917e607647'
WHERE Username = 'bwilson';
PRINT 'Updated bwilson -> agent123';

PRINT '';
PRINT 'All passwords fixed successfully!';
PRINT '';

-- Verify updates
SELECT
    UserID as ID,
    Username,
    FullName as [Full Name],
    Role,
    Status,
    PasswordHash as [Full Hash]
FROM Users
ORDER BY UserID;

PRINT '';
PRINT '====================================================';
PRINT 'LOGIN CREDENTIALS (ALL WORKING NOW):';
PRINT '====================================================';
PRINT 'admin    -> password: admin123';
PRINT 'jsmith   -> password: staff123';
PRINT 'mjones   -> password: staff123';
PRINT 'bwilson  -> password: agent123';
PRINT 'slee     -> password: staff123';
PRINT '====================================================';
GO
