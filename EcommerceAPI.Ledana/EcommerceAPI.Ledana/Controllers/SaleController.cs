using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Models;
using EcommerceAPI.Ledana.Options;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Ledana.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<SaleProductViewDto>>>> Get([FromQuery] SaleOptions saleOptions)
        {
            var response = await _saleService.GetAllSales(saleOptions);

            if (response is null) return BadRequest();
            return Ok(response);
        }
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponseDto<List<SaleProductViewDto>>>> Get()
        {
            var response = await _saleService.GetAllSales();

            if (response is null) return BadRequest();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<SaleProductViewDto>>> Get(int id)
        {
            ApiResponseDto<SaleProductViewDto> response = await _saleService.GetSaleById(id);

            if (response is null) return NotFound();
            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Sale>>> Post([FromBody] SaleDto sale)
        {
            if (!ModelState.IsValid) return BadRequest();

            ApiResponseDto<Sale> response = await _saleService.CreateSale(sale);

            if (response is null) return BadRequest(response);
            return Ok(response);
        }

    }
}
