using ModularMonolith.Core.WebApi;
using ModularMonolith.ExternalServices;
using ModularMonolith.Identity.Infraestructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddDiscoveredControllers<MainController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Import modules
builder.Services.AddIdentityConfiguration();
builder.Services.AddExternalServices(configuration);


var app = builder.Build();
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
