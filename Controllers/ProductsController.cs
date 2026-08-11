using AutoMapper;
using Luna.Data;
using Luna.Dtos;
using Luna.Models;
using Luna.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Luna.Controllers
{
    [Authorize]
    [ApiController] // Включает Автом Валидацию Модели И Привязку Параметров из Тела/строки запроса.
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        // private readonly AppDbContext _context;
        // private readonly IMapper _mapper;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productDto = await _productService.GetByIdAsync(id);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
        {
            var productDto = await _productService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
        {
            await _productService.UpdateAsync(id, updateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}