using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Persistence.Contexts;
using NoteApp.Persistence.Repositories;

namespace NoteApp.Persistence.DependencyInjection;

// Registers Persistence layer services
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NoteAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<INoteRepository, NoteRepository>();

        return services;
    }
}
