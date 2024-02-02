using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Application.DTOs;

namespace TaskProcessor.Application.Services
{
    public class CustomerService
    {
        private readonly IRepositoryCustomer<Customer> _customerRepository;

        public CustomerService(IRepositoryCustomer<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void CreateCustomer(string name)
        {
            if (ServiceHelper.IsDuplicateName(_customerRepository, name))
                throw new InvalidOperationException("A customer with the same name already exists.");

            var customer = new Customer(name);

            _customerRepository.Add(customer);
        }

        internal bool IsDuplicateName(string name)
        {
            return _customerRepository.GetAll().Any(c => c.Name == name);
        }

        public void UpdateCustomer(int customerId, CustomerDTO updatedCustomerDTO)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer != null)
            {
                customer.Name = updatedCustomerDTO.Name;
                _customerRepository.Update(customer);
            }
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            try
            {
                return _customerRepository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting customers: " + ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
