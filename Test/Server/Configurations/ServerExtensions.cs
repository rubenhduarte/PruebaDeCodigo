using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Test.Server.Data;
using Test.Server.Models;
using Test.Server.Services;

namespace Test.Server.Configurations;

public static class ServerExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfigurationContext(configuration);
        services.AddConfigurationMapster(configuration);
        services.AddConfigurationJWT(configuration);
        services.AddConfigurationIdentity(configuration);
        services.AddConfigurationPdfGeneration(configuration);

    }

    public static void AddConfigurationContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServerLocal");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }


    public static void AddConfigurationMapster(this IServiceCollection services, IConfiguration configuration)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddScoped<IMapper, Mapper>();

    }

    public static void AddConfigurationJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWTSettings");
        // Add Auth and JWT
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
            };
        });
    }

    public static void AddConfigurationIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    public static void AddConfigurationPdfGeneration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPdfGenerationService, PdfGenerationService>();
    }

}
