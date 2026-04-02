using ECommerce.Contracts.TerrenceLGee.Mappings.AddressMappings;
using ECommerce.Contracts.TerrenceLGee.Mappings.SaleMappings;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.CustomerDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.CustomerMappings;

public static class ToDto
{
    extension(ApplicationUser customer)
    {
        public RetrievedCustomerDto ToRetrievedCustomerDto()
        {
            return new RetrievedCustomerDto
            {
                CustomerId = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.Email ?? "N/A",
                UserName = customer.UserName ?? "N/A",
                DateOfBirth = customer.DateOfBirth,
                RegistrationDate = customer.RegistrationDate
            };
        }
    }
}