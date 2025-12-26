using AutoMapper;
using JafnaEcommerceApi.Exceptions;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;
using JafnaEcommerceApi.Models.DTOs.ProductDTOs;
using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Models.Entities;
using JafnaEcommerceApi.Repositories;
using JafnaEcommerceApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Services.Implementation;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    private readonly IProductRepository _productRepository;

    public SaleService(ISaleRepository saleRepository, IMapper mapper, IValidationService validationService, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _validationService = validationService;
        _productRepository = productRepository;

    }
    
    public async Task CreateAsync(SaleCreateDto saleCreateDto)
    {
        var validatedItems = ValidateAndAggregateItems(saleCreateDto);

        var productIds = validatedItems.Select(x => x.ProductId).ToList();

        var products = await GetProductsByIdsAsync(productIds);

        var existingIds = products.Select(x => x.Id).ToList();

        ThrowIfProductsMissing(productIds, existingIds);

        var (saleDetails, totalPrice) = BuildSaleDetailsAndComputeTotal(validatedItems, products);

        await CreateSaleAsync(saleDetails, totalPrice);
    }

    private List<SaleDetailCreateDto> ValidateAndAggregateItems(SaleCreateDto saleCreateDto)
    {
        return _validationService.ValidateAndAggregateItems(saleCreateDto);
    }

    private async Task<List<ProductDto>> GetProductsByIdsAsync(List<int> productIds)
    {
        var entities = await _productRepository.GetProductsByIdsAsync(productIds);
        return _mapper.Map<List<ProductDto>>(entities);
    }

    private void ThrowIfProductsMissing(List<int> requestedIds, List<int> existingIds)
    {
        var missing = requestedIds.Except(existingIds).ToList();
        if (missing.Any())
            throw new ValidationException($"Products not found: {string.Join(", ", missing)}");
    }

    private (List<SaleDetailDto> saleDetails, decimal totalPrice) BuildSaleDetailsAndComputeTotal(List<SaleDetailCreateDto> validatedItems, List<ProductDto> products)
    {
        var details = new List<SaleDetailDto>();
        decimal total = 0m;

        foreach (var item in validatedItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null) continue;

            var lineTotal = product.Price * item.ProductQuantity;
            total += lineTotal;

            details.Add(new SaleDetailDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductQuantity = item.ProductQuantity,
                ProductPrice = product.Price,
                ProductTotalPrice = lineTotal,
            });
        }

        return (details, total);
    }

    private async Task CreateSaleAsync(List<SaleDetailDto> saleDetails, decimal totalPrice)
    {
        var saleId = await _saleRepository.CreateSale(totalPrice);

        var saleDetailEntity = _mapper.Map<List<SaleDetail>>(saleDetails);

        foreach (var d in saleDetailEntity) d.SaleId = saleId;

        await _saleRepository.CreateSaleDetails(saleDetailEntity);
    }

    public async Task<PagedResponse<SaleDto>> GetAllSaleAsync(int pageNumber, int pageSize)
    {
        var salesQuery = _saleRepository.GetSalesQuery();

        var totalCount = await salesQuery.CountAsync();

        var salesList = await salesQuery
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

        var salesDto = _mapper.Map<List<SaleDto>>(salesList);

        return new PagedResponse<SaleDto>(salesDto, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResponse<SaleDetailDto>> GetSaleDetailsAsync(int saleId, int pageNumber, int pageSize)
    {
        var detailsQuery = _saleRepository.GetSaleDetailsQuery(saleId);

        var totalCount = await detailsQuery.CountAsync();

        var detailsList = await detailsQuery
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

        var detailsDto = _mapper.Map<List<SaleDetailDto>>(detailsList);

        return new PagedResponse<SaleDetailDto>(detailsDto, totalCount, pageNumber, pageSize);
    }


}
