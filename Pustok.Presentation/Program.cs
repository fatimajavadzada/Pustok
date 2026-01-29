using Pustok.DataAccess.ServiceRegistrations;
using Pustok.Business.ServiceRegistrations;
using Pustok.Presentation.Middlewares;

namespace Pustok.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();



        builder.Services.AddCors(options => options.AddPolicy("MyPolicy", policy =>
        {
            policy.AllowAnyOrigin() //WithOrigins("http://localhost:3000", "https://www.examplefrontend.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }));

        builder.Services.AddDataAccessServices(builder.Configuration);
        builder.Services.AddBusinessServices();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionHandler>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        { 
            app.UseSwagger(); // Enables middleware to serve generated Swagger as a JSON endpoint
            app.UseSwaggerUI(); // Enables middleware to serve swagger-ui (HTML, JS, CSS, etc.)
        }

        app.UseHttpsRedirection();

        app.UseCors("MyPolicy");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
