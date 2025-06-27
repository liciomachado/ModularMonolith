using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Infraestructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddDiscoveredControllers<MainController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddIdentityConfiguration(configuration);
builder.Services.AddJwtTokenConfiguration(configuration);

var app = builder.Build();
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.UseAuthenticationAndAuthorization();
app.MapControllers();
app.Run();
