﻿using Bookify.Application.Abstractions.Behaviours;
using Bookify.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
namespace Bookify.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration => {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddTransient<PricingService>();
            return services;
        }
    }
}
