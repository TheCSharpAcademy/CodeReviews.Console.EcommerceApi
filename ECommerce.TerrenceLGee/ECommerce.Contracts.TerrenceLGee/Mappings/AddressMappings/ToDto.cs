using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.AddressMappings;

public static class ToDto
{
    extension(Address address)
    {
        public RetrievedAddressDto ToRetrievedAddressDto()
        {
            return new RetrievedAddressDto
            {
                Id = address.Id,
                CustomerId = address.CustomerId,
                CustomerName = (address.Customer is not null) 
                ? $"{address.Customer.FirstName} {address.Customer.LastName}"
                : "N/A",
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsBillingAddress = address.IsBillingAddress,
                IsShippingAddress = address.IsShippingAddress
            };
        }
      
    }
}