using System.ComponentModel.DataAnnotations;

namespace ModularMonolith.Core.WebApi.Options;

public class IdentityOptions
{
    [Range(1, 100)]
    public int MaxItems { get; set; }

    [Required]
    [MinLength(10)]
    public required string Secret { get; set; }

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }

    [Required]
    [Range(0, 10_000)]
    public int TokenExpirationInMinutes { get; set; }
}