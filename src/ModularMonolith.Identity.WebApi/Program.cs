using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Infraestructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscoveredControllers<MainController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddIdentityConfiguration();

var app = builder.Build();
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
