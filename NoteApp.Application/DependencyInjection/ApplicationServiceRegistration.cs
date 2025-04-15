using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Application.Behaviors;
using NoteApp.Application.Features.Notes.Profiles;

namespace NoteApp.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(typeof(NoteMappingProfile).Assembly);

        // mediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // behavior
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;

        
    }
}
