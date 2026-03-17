using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolShuttleBus.Application.Admin;
using SchoolShuttleBus.Application.Attendance;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Dispatching;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Application.Registrations;
using SchoolShuttleBus.Application.Routes;
using SchoolShuttleBus.Infrastructure.Admin;
using SchoolShuttleBus.Infrastructure.Attendance;
using SchoolShuttleBus.Infrastructure.Auth;
using SchoolShuttleBus.Infrastructure.Notifications;
using SchoolShuttleBus.Infrastructure.Persistence;
using SchoolShuttleBus.Infrastructure.Registrations;
using SchoolShuttleBus.Infrastructure.Routes;

namespace SchoolShuttleBus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<MailjetOptions>(configuration.GetSection(MailjetOptions.SectionName));
        services.Configure<ReminderOptions>(configuration.GetSection(ReminderOptions.SectionName));

        services.AddDbContext<SchoolShuttleBusDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SchoolShuttleBus")));

        services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<SchoolShuttleBusDbContext>()
            .AddSignInManager();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((options, jwtOptionsAccessor) =>
            {
                var jwtOptions = jwtOptionsAccessor.Value;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddHttpContextAccessor();
        services.AddHttpClient<MailjetEmailDispatcher>(client =>
        {
            client.BaseAddress = new Uri("https://api.mailjet.com/");
        });
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        services.AddScoped<ITokenFactory, JwtTokenFactory>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IRouteService, RouteService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IEmailDispatcher, MailjetEmailDispatcher>();
        services.AddSingleton<DispatchConflictDetector>();
        services.AddScoped<SeedDataService>();
        services.AddHostedService<ReminderBackgroundService>();

        return services;
    }
}
