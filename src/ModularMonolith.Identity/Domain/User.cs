namespace ModularMonolith.Identity.Domain;

internal class User
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedPassword { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public List<UserClaim> Claims { get; private set; } = new();

    protected User() { }

    public User(Guid id, string userName, string email, string passwordHash)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTime.Now;
        UpdatedPassword = DateTime.Now;
        LastLoginAt = null;
        Claims = [];
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.Now;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedPassword = DateTime.Now;
    }

    public void AddClaim(string value)
    {
        const string defaultType = "role";
        if (!Claims.Any(c => c.Type == defaultType && c.Value == value))
            Claims.Add(new UserClaim(defaultType, value));
    }

    public void AddClaim(string type, string value)
    {
        if (!Claims.Any(c => c.Type == type && c.Value == value))
            Claims.Add(new UserClaim(type, value));
    }

    public void RemoveClaim(string type, string value)
    {
        Claims.RemoveAll(c => c.Type == type && c.Value == value);
    }
}