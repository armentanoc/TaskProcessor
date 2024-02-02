using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Application.DTOs;

namespace TaskProcessor.Application.Services
{
    public class AddressService
    {
        private readonly IRepository<Address> _addressRepository;

        public AddressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public void UpdateAddress(int addressId, AddressDTO updatedAddressDTO)
        {
            var address = _addressRepository.GetById(addressId);
            if (address != null)
            {
                address.Street = updatedAddressDTO.Street;
                address.Number = updatedAddressDTO.Number;
                address.Complement = updatedAddressDTO.Complement;
                address.Neighborhood = updatedAddressDTO.Neighborhood;
                address.City = updatedAddressDTO.City;
                address.State = updatedAddressDTO.State;
                address.ZipCode = updatedAddressDTO.ZipCode;

                _addressRepository.Update(address);
            }
        }


        public IEnumerable<Address> GetAllAddresses()
        {
            return _addressRepository.GetAll();
        }

        public void CreateAddress(string street, uint number, string? complement, string neighborhood, string city, string state, string zipCode)
        {
            try
            {
                var address = new Address
                {
                    Street = street,
                    Number = number,
                    Complement = complement,
                    Neighborhood = neighborhood,
                    City = city,
                    State = state,
                    ZipCode = zipCode
                };

                _addressRepository.Add(address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
