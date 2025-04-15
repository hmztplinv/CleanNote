using NoteApp.API.Middleware;
using NoteApp.Application.DependencyInjection;
using NoteApp.Persistence.Contexts;
using NoteApp.Persistence.DependencyInjection;
using NoteApp.Persistence.Seed;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();


// Register Application & Persistence services
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// SEED DATABASE
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NoteAppDbContext>();
    await SeedData.InitializeAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
// app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
