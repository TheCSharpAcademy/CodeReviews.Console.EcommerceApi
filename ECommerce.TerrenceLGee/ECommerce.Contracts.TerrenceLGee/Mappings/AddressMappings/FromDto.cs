using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.AddressMappings;

public static class FromDto
{
    extension(CreateAddressDto addressDto)
    {
        public Address FromCreateAddressDto()
        {
            return new Address
            {
                CustomerId = addressDto.CustomerId,
                AddressLine1 = addressDto.AddressLine1,
                AddressLine2 = addressDto.AddressLine2,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                IsBillingAddress = addressDto.IsBillingAddress,
                IsShippingAddress = addressDto.IsShippingAddress
            };
        }
    }

    extension(UpdateAddressDto addressDto)
    {
        public Address FromUpdateAddressDto()
        {
            return new Address
            {
                Id = addressDto.Id,
                CustomerId = addressDto.CustomerId,
                AddressLine1 = addressDto.AddressLine1,
                AddressLine2 = addressDto.AddressLine2,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                IsBillingAddress = addressDto.IsBillingAddress,
                IsShippingAddress = addressDto.IsShippingAddress
            };
        }
    }
}