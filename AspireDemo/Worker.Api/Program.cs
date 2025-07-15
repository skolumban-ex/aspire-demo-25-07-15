
using Microsoft.EntityFrameworkCore;

namespace Worker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName); // Fixes schema ID collision
        });

        // Get the connection string from configuration
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Register DbContext with PostgreSQL
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

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
            app.UseSwagger();
            app.UseSwaggerUI();
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
