using Bookify.Application.Abstractions;
using Bookify.Application.Abstractions.Clock;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
              this IServiceCollection services,
              IConfiguration configuration
            )
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = configuration.GetConnectionString("database")
                ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(
                 options => options.UseNpgsql(connectionString)
                 .UseSnakeCaseNamingConvention()
            );

            return services;
        }
    }
}
