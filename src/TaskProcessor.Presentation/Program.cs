using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskProcessor.Infra.Context;
using TaskProcessor.Infra.Repositories;
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using Microsoft.Extensions.Configuration;
using TaskProcessor.Presentation.CustomExceptions;

namespace TaskProcessor.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = BuildConfiguration();

            var databaseSettings = configuration.GetSection("DatabaseConnection");
            var connectionString = databaseSettings["ConnectionString"];
            var provider = databaseSettings["Provider"];

            var serviceProvider = new ServiceCollection()

                //General
                .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))

                //Tasks
                .AddScoped<IRepositoryTaskEntity<TaskEntity>, EFRepositoryTaskEntity>()
                .AddScoped<TaskService>()

                //Subtasks
                .AddScoped<IRepositorySubTaskEntity<SubTaskEntity>, EFRepositorySubTaskEntity>()
                .AddScoped<SubTaskService>()

                //Task Execution
                .AddScoped<TaskExecutionService>()

                //AppRunner
                .AddScoped<AppRunner>()

                //DbContext
                .AddDbContext<AppDbContext>(options =>
                {
                    if (provider.Equals("sqlite", StringComparison.OrdinalIgnoreCase))
                        options.UseSqlite(connectionString);
                    else if (provider.Equals("sqlserver", StringComparison.OrdinalIgnoreCase))
                        options.UseSqlServer(connectionString);
                    else
                        throw new UnsupportedDatabaseException();
                }, ServiceLifetime.Transient)

                //Build
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var appRunner = scope.ServiceProvider.GetRequiredService<AppRunner>();
                await appRunner.Run();
            }
        }

        private static IConfiguration BuildConfiguration()
        {
           return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}

