using TaskProcessor.Application.Services;

namespace TaskProcessor.Presentation.Helpers
{
    internal class Create
    {
        public static void Customer(CustomerService customerService)
        {
            Console.WriteLine("\nEnter customer name:");
            string customerName = Console.ReadLine();

            if (!string.IsNullOrEmpty(customerName))
            {
                try
                {
                    customerService.CreateCustomer(customerName);
                    Console.WriteLine("Customer created successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating customer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid customer name. Please provide a non-empty name.");
            }
        }
    }
}
