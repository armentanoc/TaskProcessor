using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

namespace TaskProcessor.Presentation
{
    public class AppRunner
    {
        private readonly CustomerService _customerService;
        private readonly AddressService _addressService;

        public AppRunner(CustomerService customerService, AddressService addressService)
        {
            _customerService = customerService;
            _addressService = addressService;
        }

        public void Run()
        {
            DefaultData.TryInserting(_customerService, _addressService);

            DisplayData<Customer>.Display(() => _customerService.GetAllCustomers());
            DisplayData<Address>.Display(() => _addressService.GetAllAddresses());

            Create.Customer(_customerService);

            Console.WriteLine("\nWorks fine. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
