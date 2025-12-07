-- Test query to see actual user data in database
USE AWEElectronics_DB;
GO

-- Show all users with their password hashes
SELECT
    UserID,
    Username,
    FullName,
    Email,
    Role,
    Status,
    LEFT(PasswordHash, 30) + '...' as PasswordHashPreview,
    LEN(PasswordHash) as HashLength,
    CASE
        WHEN PasswordHash LIKE '$2a$%' THEN 'BCrypt'
        WHEN PasswordHash LIKE '$2b$%' THEN 'BCrypt'
        WHEN LEN(PasswordHash) = 64 THEN 'SHA256'
        ELSE 'Unknown'
    END as HashType
FROM Users
ORDER BY UserID;
GO
