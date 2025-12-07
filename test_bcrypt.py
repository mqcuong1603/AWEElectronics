import bcrypt

# Hash from database
stored_hash = b"$2a$11$xmUVpf71/YynudMDR1VdauOOuBI.bKjUhfK5LDmxjKRs4Ps2kEgHS"

# Common passwords to test
test_passwords = [
    "admin",
    "admin123",
    "Admin123",
    "password",
    "Password123",
    "YourStrong@Password123",
    "Admin",
    "Administrator",
    "123456",
    "awe123",
    "AWE123",
    "password123",
    "AWEElectronics",
    "default",
    "test",
    "test123"
]

print(f"Testing BCrypt hash: {stored_hash.decode()}")
print()

for password in test_passwords:
    try:
        is_valid = bcrypt.checkpw(password.encode(), stored_hash)
        status = "MATCH!" if is_valid else "No match"
        print(f"Password '{password}': {status}")

        if is_valid:
            print(f"\n*** FOUND THE PASSWORD! ***")
            print(f"The password is: {password}")
            break
    except Exception as e:
        print(f"Password '{password}': Error - {e}")

else:
    print("\nNo matching password found.")
    print("\nGenerating new BCrypt hash for 'admin123':")
    new_hash = bcrypt.hashpw("admin123".encode(), bcrypt.gensalt(rounds=11))
    print(f"New hash: {new_hash.decode()}")
