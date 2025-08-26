using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/lineitems")]
    [ApiController]
    public class LineItemController : ControllerBase
    {
        private readonly ILineItemService _lineItemService;

        public LineItemController(ILineItemService lineItemService)
        {
            _lineItemService = lineItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LineItemDto>>> GetPagedLineItems([FromQuery] PaginationParams paginationParams)
        {
            var responseWithDtos = await _lineItemService.GetPagedLineItems(paginationParams);

            if (responseWithDtos.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDtos.Message);
            }

            return Ok(responseWithDtos.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LineItemDto>> GetLineItemById(int id)
        {
            var response = await _lineItemService.GetLineItemById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var returnedLineItem = response.Data;

            var lineItemDto = new LineItemDto
            {
                LineItemId = returnedLineItem.LineItemId,
                ProductId = returnedLineItem.ProductId,
                ProductName = returnedLineItem.Product.ProductName,
                Category = returnedLineItem.Product.Category.CategoryName,
                Quantity = returnedLineItem.Quantity,
                UnitPrice = returnedLineItem.UnitPrice,
                SaleId = returnedLineItem.SaleId,
            };

            return Ok(lineItemDto);
        }

        [HttpPost]
        public async Task<ActionResult<LineItemDto>> CreateLineItem([FromBody] WriteLineItemDto writeLineItemDto)
        {
            var responseWithDataDto = await _lineItemService.CreateLineItem(writeLineItemDto);

            if (responseWithDataDto.Message == "Sale not found.")
            {
                return NotFound(responseWithDataDto.Message);
            }

            if (responseWithDataDto.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDataDto.Message);
            }

            return CreatedAtAction(nameof(GetLineItemById),
                new { id = responseWithDataDto.Data.LineItemId }, responseWithDataDto.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LineItemDto>> UpdateLineItem(int id, [FromBody] WriteLineItemDto writeLineItemDto)
        {
            var response = await _lineItemService.UpdateLineItem(id, writeLineItemDto);

            if (response.Message == "Line Item not found." || response.Message == "Sale not found." || response.Message == "Product not found.")
            {
                return NotFound(response.Message);
            }

            if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var response = await _lineItemService.DeleteLineItem(id);

            if (response.Message == "Line Item not found.")
            {
                return NotFound(response.Message);
            }
            else if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }
    }
}
