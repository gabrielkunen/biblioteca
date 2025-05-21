using Biblioteca.Api.AppConfig;
using Biblioteca.Api.Middleware;
using Biblioteca.Application.Models;
using Biblioteca.Data.Context;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration();
builder.Services.AddVersioningServices();
builder.Services.AddSwaggerServices();
builder.Services.AddAutenticacaoServices();
builder.Services.ResolveDependencies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode is 401 or 403)
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new RespostaPadraoModel(false, "Acesso não autorizado"));
    }
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();

    foreach (var description in descriptions)
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.GroupName.ToUpperInvariant();
        
        options.SwaggerEndpoint(url, name);
    }
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

if(!builder.Environment.IsDevelopment())
    app.UseHttpsRedirection();

await using(var serviceScope = app.Services.CreateAsyncScope())
await using (var context = serviceScope.ServiceProvider.GetRequiredService<BibliotecaContext>())
{
    context.Database.EnsureCreated();
}

app.MapControllers();

app.Run();

public partial class Program;