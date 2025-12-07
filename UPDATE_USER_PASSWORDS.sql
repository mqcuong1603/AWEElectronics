-- =============================================
-- Update User Passwords with BCrypt Hashes
-- =============================================
USE AWEElectronics_DB;
GO

PRINT '====================================================';
PRINT 'Updating user passwords with BCrypt hashes...';
PRINT '====================================================';
PRINT '';

-- Update admin user (password: admin123)
UPDATE Users
SET PasswordHash = '$2a$11$vN7mXqZvXKGQs0FHvJH9COLGxW6z.q0bJNLF5yqvqKCHxDkRWqLGe'
WHERE Username = 'admin';
PRINT '✓ Updated admin password -> admin123';

-- Update jsmith (password: staff123)
UPDATE Users
SET PasswordHash = '$2a$11$YqK0N8F7xKYHb1JvKp8Z3.JZn8qVQZvFq8KpNxLz9.qGZH6NvT7yC'
WHERE Username = 'jsmith';
PRINT '✓ Updated jsmith password -> staff123';

-- Update mjones (password: staff123)
UPDATE Users
SET PasswordHash = '$2a$11$YqK0N8F7xKYHb1JvKp8Z3.JZn8qVQZvFq8KpNxLz9.qGZH6NvT7yC'
WHERE Username = 'mjones';
PRINT '✓ Updated mjones password -> staff123';

-- Update bwilson (password: agent123)
UPDATE Users
SET PasswordHash = '$2a$11$8KqN0Z8X7dYIc2KwLq9a4.GZo9rWRawGr9LqOyMa0.rHaI7OwU8zD'
WHERE Username = 'bwilson';
PRINT '✓ Updated bwilson password -> agent123';

-- Update slee (password: staff123)
UPDATE Users
SET PasswordHash = '$2a$11$YqK0N8F7xKYHb1JvKp8Z3.JZn8qVQZvFq8KpNxLz9.qGZH6NvT7yC'
WHERE Username = 'slee';
PRINT '✓ Updated slee password -> staff123';

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
GO
