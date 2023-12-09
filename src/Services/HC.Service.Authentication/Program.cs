using HC.Foundation.MessageBus.IoC;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Entities;
using HC.Service.Authentication.Mappings;
using HC.Service.Authentication.Middlewares;
using HC.Service.Authentication.Repositories;
using HC.Service.Authentication.Repositories.IRepositories;
using HC.Service.Authentication.Services;
using HC.Service.Authentication.Services.IServices;
using HC.Service.Authentication.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HC Authentication API", Description = "HC Authentication Rest Api", Version = "v1.0" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                    new string[] { }
                }
            });
        });

        // Add AuthenticationDbContext
        builder.Services.AddDbContext<AuthenticationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Config AppSettings
        var appSettingsSection = builder.Configuration.GetSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();

        // Routing
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        
        // Kafka Message Bus
        builder.Services.AddKafkaMessageBus(config =>
        {
            config.BootstrapServers = "localhost:9092";
        });

        #region Dependency Injection

        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        #endregion Dependency Injection

        #region AutoMapper

        builder.Services.AddAutoMapper(typeof(Maps));

        #endregion AutoMapper

        var app = builder.Build();

        app.UseMiddleware<JwtMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(
                    c =>
                    {
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            swaggerDoc.Servers = new List<OpenApiServer> {
                                new OpenApiServer { Url = "", Description = "Local" }
                            };
                        });
                    });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "HC Auth API");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}