using Application.Commands.CreateUserCommand;
using Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title   = "User API", 
        Version = "v1" 
    });
});

// 2) Infrastructure, EF, MediatR, etc.
await builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();