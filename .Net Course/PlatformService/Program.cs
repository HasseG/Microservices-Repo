using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddDbContext<AppDbContext>
        (opt => opt.UseInMemoryDatabase("InMem"));

        //Dependeny Injection
        builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
        builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        PrepDb.PrepPopulation(app);
        
        Console.WriteLine($"--> CommandService Endpoint {app.Configuration["CommandService"]}");

        app.Run();
    }
}