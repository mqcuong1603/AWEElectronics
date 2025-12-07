-- =============================================
-- Update User Passwords with SHA256 Hashes
-- =============================================
-- Simple SHA256 hashing - no external dependencies needed!
-- =============================================

USE AWEElectronics_DB;
GO

PRINT '====================================================';
PRINT 'Updating user passwords with SHA256 hashes...';
PRINT '====================================================';
PRINT '';

-- Update admin user (password: admin123)
UPDATE Users
SET PasswordHash = '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9'
WHERE Username = 'admin';
PRINT 'Updated admin password -> admin123';

-- Update jsmith (password: staff123)
UPDATE Users
SET PasswordHash = 'a3c024e0d5076773e1e5e8cdb3e0c57f6e1f4ed26a73a70df8ad8a77bc0a3f51'
WHERE Username = 'jsmith';
PRINT 'Updated jsmith password -> staff123';

-- Update mjones (password: staff123)
UPDATE Users
SET PasswordHash = 'a3c024e0d5076773e1e5e8cdb3e0c57f6e1f4ed26a73a70df8ad8a77bc0a3f51'
WHERE Username = 'mjones';
PRINT 'Updated mjones password -> staff123';

-- Update bwilson (password: agent123)
UPDATE Users
SET PasswordHash = '3c9909afec25354d551dae21590bb26e38d53f2173b8d3dc3eee4c047e7ab1c1'
WHERE Username = 'bwilson';
PRINT 'Updated bwilson password -> agent123';

-- Update slee (password: staff123)
UPDATE Users
SET PasswordHash = 'a3c024e0d5076773e1e5e8cdb3e0c57f6e1f4ed26a73a70df8ad8a77bc0a3f51'
WHERE Username = 'slee';
PRINT 'Updated slee password -> staff123';

PRINT '';
PRINT 'All passwords updated successfully!';
PRINT '';

-- Verify updates
PRINT 'Current users:';
SELECT
    UserID as ID,
    Username,
    FullName as [Full Name],
    Role,
    Status,
    LEFT(PasswordHash, 30) + '...' as [Hash Preview],
    LEN(PasswordHash) as [Hash Length]
FROM Users
ORDER BY UserID;

PRINT '';
PRINT '====================================================';
PRINT 'LOGIN CREDENTIALS:';
PRINT '====================================================';
PRINT 'admin    -> password: admin123';
PRINT 'jsmith   -> password: staff123';
PRINT 'mjones   -> password: staff123';
PRINT 'bwilson  -> password: agent123';
PRINT 'slee     -> password: staff123';
PRINT '====================================================';
PRINT '';
PRINT 'You can now login to both Web and Desktop applications!';
PRINT 'Using simple SHA256 - no BCrypt dependencies needed!';
GO
