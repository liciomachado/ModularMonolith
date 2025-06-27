namespace ModularMonolith.Identity.Domain;

public class UserClaim
{
    public string Type { get; private set; }
    public string Value { get; private set; }

    protected UserClaim() { }
    public UserClaim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public static UserClaim GetDefaultType()
    {
        return new UserClaim("role", "admin");
    }
}