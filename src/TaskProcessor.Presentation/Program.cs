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
                .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
                .AddScoped<IRepositoryCustomer<Customer>, EFRepositoryCustomer>()
                .AddScoped<CustomerService>()
                .AddScoped<IRepositoryAddress<Address>, EFRepositoryAddress>()
                .AddScoped<AddressService>()
                .AddScoped<AppRunner>()
                .AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=database_task_processor.db"))
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var appRunner = scope.ServiceProvider.GetRequiredService<AppRunner>();
                appRunner.Run();
            }
        }
    }
}

