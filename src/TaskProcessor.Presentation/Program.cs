using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskProcessor.Infra.Context;
using TaskProcessor.Infra.Repositories;
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                //General
                .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
                //Tasks
                .AddScoped<IRepositoryTaskEntity<TaskEntity>, EFRepositoryTaskEntity>()
                .AddScoped<TaskService>()
                //Subtasks
                .AddScoped<IRepositorySubTaskEntity<SubTaskEntity>, EFRepositorySubTaskEntity>()
                .AddScoped<SubTaskService>()
                //AppRunner
                .AddScoped<AppRunner>()
                //DbContext
                .AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=database_task_processor.db"))
                //Build
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var appRunner = scope.ServiceProvider.GetRequiredService<AppRunner>();
                appRunner.Run();
            }
        }
    }
}

