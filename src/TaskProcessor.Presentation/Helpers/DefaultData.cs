using TaskProcessor.Presentation.DTOs;
using TaskProcessor.Application.Services;

namespace TaskProcessor.Presentation.Helpers
{
    internal class DefaultData
    {
        internal static void TryInserting(CustomerService customerService, AddressService addressService)
        {
            try
            {
                if (!customerService.GetAllCustomers().Any() && !addressService.GetAllAddresses().Any())
                    InsertDefaultData(customerService, addressService);
                else
                    Console.WriteLine("Customer and Address already exist. No changes made.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting: " + ex.Message, ex.StackTrace);
            }
        }

        private static void InsertDefaultData(CustomerService customerService, AddressService addressService)
        {
            InsertDefaultCustomer(customerService);
            InsertDefaultAddress(addressService);
        }

        private static void InsertDefaultAddress(AddressService addressService)
        {
            var addressDTO = new AddressPresentationDTO
                (
                    street: "123 Main St",
                    number: 42,
                    complement: "Apt 2",
                    neighborhood: "Downtown",
                    city: "Cityville",
                    state: "ST",
                    zipcode: "12345"
                );

            addressService.CreateAddress(addressDTO.Street, addressDTO.Number, addressDTO.Complement, addressDTO.Neighborhood, addressDTO.City, addressDTO.State, addressDTO.ZipCode);

        }

        private static void InsertDefaultCustomer(CustomerService customerService)
        {
            try
            {
                var customerDTO = new CustomerPresentationDTO("John Doe");
                customerService.CreateCustomer(customerDTO.Name);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting default customer: " + ex.Message, ex.StackTrace);
            }
        }
    }
}