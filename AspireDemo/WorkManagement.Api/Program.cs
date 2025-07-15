
using Microsoft.EntityFrameworkCore;

namespace WorkManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Register CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });


        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName); // Fixes schema ID collision
        });

        // Get the connection string from configuration
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Register DbContext with PostgreSQL
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register HttpClient for a typed client
        builder.Services.AddHttpClient<WorkerApiClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["WorkerUrl"]);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        app.UseCors("AllowAll");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Work management");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // apply migrations
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate(); // Apply any pending migrations
        }

        app.Run();
    }
}