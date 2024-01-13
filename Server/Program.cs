using System.Reflection;
using ECS;
using MessagePack;
using MessagePack.Resolvers;
using Serialization;
using Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
}).AddMessagePackProtocol();
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddSingleton<World>();
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("Handlers")); 
});

var options = MessagePackSerializerOptions.Standard.
    WithResolver(CompositeResolver.Create(new Vector2Formatter()))
    .WithResolver(ContractlessStandardResolver.Instance);



var app = builder.Build();

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ServerHub>("/server");

app.Run();