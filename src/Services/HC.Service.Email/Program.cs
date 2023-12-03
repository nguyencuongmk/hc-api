using HC.Service.Email.Data;
using HC.Service.Email.Repositories;
using HC.Service.Email.Repositories.IRepositories;
using HC.Service.Email.Services;
using HC.Service.Email.Services.IServices;
using HC.Service.Email.Settings;
using Microsoft.EntityFrameworkCore;
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HC Email API", Description = "HC Email Rest Api", Version = "v1.0" });

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
        builder.Services.AddDbContext<EmailDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Config AppSettings
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        // Routing
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        #region Dependency Injection

        builder.Services.AddScoped<IEmailLoggerRepository, EmailLoggerRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        #endregion Dependency Injection

        var app = builder.Build();

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
                c.SwaggerEndpoint("v1/swagger.json", "HC Email API");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}