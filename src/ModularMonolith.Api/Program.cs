using ModularMonolith.Catalog.Infraestructure;
using ModularMonolith.Core.WebApi;
using ModularMonolith.ExternalServices;
using ModularMonolith.Identity.Infraestructure;
using ModularMonolith.Purchase.Infraestructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddDiscoveredControllers<MainController>();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Import modules
builder.Services.AddIdentityConfiguration(configuration);
builder.Services.AddCatalogConfiguration(configuration);
builder.Services.AddPurchaseConfiguration(configuration);
builder.Services.AddExternalServices(configuration);
builder.Services.AddJwtTokenConfiguration(configuration);

var app = builder.Build();
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.UseAuthenticationAndAuthorization();
app.MapControllers();
app.Run();
