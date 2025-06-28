using System.ComponentModel.DataAnnotations;

namespace ModularMonolith.Core.WebApi.Options;

public class CatalogOptions
{
    [Required]
    public required string OpenaiApiKey { get; set; }

    [Required]
    public required string ModelEmbedding { get; set; }

    [Required]
    public required string MongoConnectionString { get; set; }

    [Required]
    public required string MongoDatabaseName { get; set; }
}