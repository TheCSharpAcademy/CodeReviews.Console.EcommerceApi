using Microsoft.AspNetCore.Mvc;

namespace kilozdazolik.Ecommerce.API.Features.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(ISaleService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<SaleDto>> CreateSaleAsync(CreateSaleDto createSaleDto)
        {
            var sale = await service.CreateSaleAsync(createSaleDto);
            return Ok(sale);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<SaleDto>> GetSaleByIdAsync(Guid id)
        {
            var sale = await service.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesAsync(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var sales = await service.GetSalesAsync(pageIndex, pageSize);
            return Ok(sales);
        }

    }
}
