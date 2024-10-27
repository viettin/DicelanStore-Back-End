using dicelanStore.DTOs;
using dicelanStore.Models;
using dicelanStore.Models.dicelanApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dicelanStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly dicelanAppContext _context;

        public ProductController(dicelanAppContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO model, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ErrorResponse { Success = false, Message = "Invalid model state", Errors = errors });
            }
            var newProduct = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CreatedDate = DateTime.UtcNow,
                MaterialId = model.MaterialId,
                ProductTypeId = model.ProductTypeId,
                Discount = model.Discount
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        var imageBytes = ms.ToArray();

                        var imageEntity = new Image
                        {
                            Image1 = imageBytes,
                            ProductId = newProduct.Id
                        };
                        await _context.Images.AddAsync(imageEntity);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "Product created successfully", Data = null });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ErrorResponse { Success = false, Message = "Invalid model state", Errors = errors });
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound(new ErrorResponse { Success = false, Message = "Product not found", Errors = new List<string> { "Product not found" } });
            
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.StockQuantity = model.StockQuantity;
            existingProduct.MaterialId = model.MaterialId;
            existingProduct.ProductTypeId = model.ProductTypeId;
            existingProduct.Discount = model.Discount;
            existingProduct.LastModifiedDate = DateTime.UtcNow;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string> { Success = true, Message = "Product updated successfully", Data = null });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetList()
        {
            var products = await _context.Products
                .Select(p => new ProductListDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    LastModifiedDate = p.LastModifiedDate,
                    IsDeleted = p.IsDeleted,
                    MaterialId = p.MaterialId,
                    MaterialName = _context.Materials
                    .Where(m => m.Id == p.MaterialId)
                    .Select(m => m.Name)
                    .FirstOrDefault(),
                    ProductTypeId = p.ProductTypeId,
                    ProductTypeName = _context.ProductTypes
                    .Where(pt => pt.Id == p.ProductTypeId)
                    .Select(pt => pt.Name)
                    .FirstOrDefault(),
                    Discount = p.Discount,
                    Images = _context.Images
                    .Where(i => i.ProductId == p.Id)
                    .OrderBy(i => i.Id)
                    .Select(i => i.Image1)
                    .Where(i => i != null)
                    .Cast<byte[]>()
                    .ToList(),
                    FirstImage = _context.Images
                    .Where(i => i.ProductId == p.Id)
                    .OrderBy(i => i.Id)
                    .Select(i => i.Image1)
                    .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterials()
        {
            var materials = await _context.Materials
                .Select(m => new
                {
                    m.Id,
                    m.Name
                })
                .ToListAsync();

            return Ok(materials);
        }
        [HttpGet("producttypes")]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes
                .Select(pt => new
                {
                    pt.Id,
                    pt.Name
                })
                .ToListAsync();

            return Ok(productTypes);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Material)
                .Include(p => p.ProductType)
                .Where(p => p.Id == id)
                .Select(p => new ProductListDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    LastModifiedDate = p.LastModifiedDate,
                    IsDeleted = p.IsDeleted,
                    MaterialId = p.MaterialId,
                    MaterialName = p.Material.Name,
                    ProductTypeId = p.ProductTypeId,
                    ProductTypeName = p.ProductType.Name,
                    Discount = p.Discount,
                    Images = _context.Images
                        .Where(i => i.ProductId == p.Id)
                        .OrderBy(i => i.Id)
                        .Select(i => i.Image1)
                        .Where(i => i != null)
                        .Cast<byte[]>()
                        .ToList(),
                    FirstImage = _context.Images
                        .Where(i => i.ProductId == p.Id)
                        .OrderBy(i => i.Id)
                        .Select(i => i.Image1)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Product not found", Errors = new List<string> { "Product not found" } });
            }

            return Ok(new ApiResponse<ProductListDTO> { Success = true, Message = "Product retrieved successfully", Data = product });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Product not found", Errors = new List<string> { "Product not found" } });
            }

            existingProduct.IsDeleted = true;
            existingProduct.LastModifiedDate = DateTime.UtcNow;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string> { Success = true, Message = "Product marked as deleted successfully", Data = null });
        }

        [HttpDelete("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Product not found", Errors = new List<string> { "Product not found" } });
            }

            existingProduct.IsDeleted = false;
            existingProduct.LastModifiedDate = DateTime.UtcNow;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string> { Success = true, Message = "Product marked as deleted successfully", Data = null });
        }
    }

}
