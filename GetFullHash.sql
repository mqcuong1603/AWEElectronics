-- Get full password hash for testing
USE AWEElectronics_DB;
GO

SELECT TOP 1
    Username,
    PasswordHash
FROM Users
WHERE Username = 'admin';
GO
