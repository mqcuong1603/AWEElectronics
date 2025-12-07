import bcrypt

print("Generating BCrypt hashes for AWE Electronics users")
print("=" * 60)
print()

# Generate hashes for each user with different passwords
users = {
    'admin': 'admin123',
    'jsmith': 'staff123',
    'mjones': 'staff123',
    'bwilson': 'agent123',
    'slee': 'staff123'
}

hashes = {}
for username, password in users.items():
    hash_bytes = bcrypt.hashpw(password.encode(), bcrypt.gensalt(rounds=11))
    hash_str = hash_bytes.decode()
    hashes[username] = hash_str
    print(f"{username:10} -> password: {password:12} -> hash: {hash_str}")

print()
print("=" * 60)
print("SQL UPDATE Script")
print("=" * 60)
print()

sql_script = """USE AWEElectronics_DB;
GO

PRINT 'Updating user passwords with BCrypt hashes...';
PRINT '';

-- Update admin user (password: admin123)
UPDATE Users SET PasswordHash = '{admin}' WHERE Username = 'admin';
PRINT 'Updated admin password';

-- Update jsmith (password: staff123)
UPDATE Users SET PasswordHash = '{jsmith}' WHERE Username = 'jsmith';
PRINT 'Updated jsmith password';

-- Update mjones (password: staff123)
UPDATE Users SET PasswordHash = '{mjones}' WHERE Username = 'mjones';
PRINT 'Updated mjones password';

-- Update bwilson (password: agent123)
UPDATE Users SET PasswordHash = '{bwilson}' WHERE Username = 'bwilson';
PRINT 'Updated bwilson password';

-- Update slee (password: staff123)
UPDATE Users SET PasswordHash = '{slee}' WHERE Username = 'slee';
PRINT 'Updated slee password';

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
    LEFT(PasswordHash, 30) + '...' as [Hash Preview]
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
GO
""".format(**hashes)

print(sql_script)

# Save to file
with open('UPDATE_USER_PASSWORDS.sql', 'w') as f:
    f.write(sql_script)

print()
print("SQL script saved to: UPDATE_USER_PASSWORDS.sql")
