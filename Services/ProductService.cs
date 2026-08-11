using AutoMapper;
using Luna.Data;
using Luna.Dtos;
using Luna.Models;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new KeyNotFoundException($"Продукт с id {id} не найден");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createDto)
        {
            var product = _mapper.Map<Product>(createDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Продукт с id {id} не найден");

            _mapper.Map(updateDto, product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Продукт с id {id} не найден");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
