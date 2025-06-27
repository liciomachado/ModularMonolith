using System.ComponentModel.DataAnnotations;

namespace ModularMonolith.Identity.Application.Settings;

public class IdentityOptions
{
    [Range(1, 100)]
    public int MaxItems { get; set; }

    [Required]
    [MinLength(10)]
    public required string Secret { get; set; }
}