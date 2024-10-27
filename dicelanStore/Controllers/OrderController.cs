using dicelanStore.DTOs;
using dicelanStore.Models.dicelanApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dicelanStore.Models;

namespace dicelanStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly dicelanAppContext _context;

        public OrderController(dicelanAppContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ErrorResponse { Success = false, Message = "Invalid model state", Errors = errors });
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            // Create the address
            var address = new Address
            {
                Name = model.Name,
                Street = model.Street,
                Ward = model.Ward,
                City = model.District,
                Province = model.Province,
                ZipCode = null, // Assuming ZipCode is not provided in the model
                UserId = userId
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // Create the order
            var order = new Order
            {
                Total = model.Total,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                OrderStatusId = 1, // Default status ID
                AddressId = address.Id,
                Name = model.Name,
                Email = model.Email
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order details
            foreach (var product in model.Products)
            {
                var productEntity = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.ProductId);
                if (productEntity == null)
                {
                    return BadRequest(new ErrorResponse { Success = false, Message = $"Product with ID {product.ProductId} not found" });
                }
                var existingOrderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.OrderId == order.Id && od.ProductId == product.ProductId);
                if (existingOrderDetail == null)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = product.ProductId,
                        Quantity = (short)product.Quantity,
                        UnitPrice = _context.Products.FirstOrDefault(p => p.Id == product.ProductId)?.Price
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
            }
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                // Get the cart items by product ID and delete them
                var cartItems = await _context.CartItems
                    .Where(ci => ci.CartId == cart.Id && model.Products.Select(p => p.ProductId).Contains(ci.ProductId))
                    .ToListAsync();

                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<OrderDTO> { Success = true, Message = "Order created successfully", Data = model });
        }

        [HttpGet("all-orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrdersForAdmin()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatus)
                .Include(o => o.Address) // Include the address
                .ToListAsync();

            var orderDTOs = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                Status = o.OrderStatus.Name, // Assuming OrderStatus has a Name property
                PhoneNumber = o.PhoneNumber,
                Total = o.Total,
                CreatedDate = o.CreatedDate,
                Name = o.Name,
                Email = o.Email,
                PaymentMethod = "COD", // Assuming Order has a PaymentMethod property
                // Discount = o.Discount,  Assuming Order has a Discount property

                Street = o.Address.Street,
                Ward = o.Address.Ward,
                District = o.Address.City,
                Province = o.Address.Province,
                Products = o.OrderDetails.Select(od => new OrderProductDTO
                {
                    ProductId = od.ProductId,
                    Quantity = (int)(od.Quantity ?? 0), // Explicit conversion with default value
                    ProductName = od.Product.Name,
                    Price = (decimal)od.Product.Price, // Assuming Product has a Price property
                    Image = _context.Images
                    .Where(i => i.ProductId == od.ProductId)
                    .OrderBy(i => i.Id)
                    .Select(i => i.Image1)
                    .FirstOrDefault()
                }).ToList()
            }).ToList();

            return Ok(new ApiResponse<List<OrderDTO>> { Success = true, Message = "Orders retrieved successfully", Data = orderDTOs });
        }

        [HttpGet("get-order/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Order not found", Errors = new List<string> { "Order not found" } });
            }

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Total = order.Total,
                PhoneNumber = order.PhoneNumber,
                CreatedDate = order.CreatedDate,
                Products = order.OrderDetails.Select(od => new OrderProductDTO
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity ?? 0,
                    ProductName = od.Product.Name
                }).ToList()
            };

            return Ok(new ApiResponse<OrderDTO> { Success = true, Message = "Order retrieved successfully", Data = orderDTO });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] OrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ErrorResponse { Success = false, Message = "Invalid model state", Errors = errors });
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Order not found", Errors = new List<string> { "Order not found" } });
            }

            order.Total = model.Total;
            order.PhoneNumber = model.PhoneNumber;
            order.Name = model.Name;
            order.Email = model.Email;
            order.LastModifiedDate = DateTime.UtcNow;

            // Update order details
            _context.OrderDetails.RemoveRange(order.OrderDetails);
            foreach (var product in model.Products)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = product.ProductId,
                    Quantity = (short)product.Quantity,
                    UnitPrice = _context.Products.FirstOrDefault(p => p.Id == product.ProductId)?.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();

            return Ok(new ApiResponse<OrderDTO> { Success = true, Message = "Order updated successfully", Data = model });
        }
        [HttpGet("user-orders")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            // Retrieve the user ID from the token
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            // Retrieve orders for the specified user
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatus)
                .Include(o => o.Address) // Include the address
                .Where(o => o.UserId == userId)
                .ToListAsync();

            // Map orders to OrderDTOs
            var orderDTOs = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                Status = o.OrderStatus.Name, // Assuming OrderStatus has a Name property
                PhoneNumber = o.PhoneNumber,
                Total = o.Total,
                CreatedDate = o.CreatedDate,
                Name = o.Name,
                Email = o.Email,
                PaymentMethod = "COD", // Assuming Order has a PaymentMethod property
                // Discount = o.Discount,  Assuming Order has a Discount property
        
                Street = o.Address.Street,
                Ward = o.Address.Ward,
                District = o.Address.City,
                Province = o.Address.Province,
                Products = o.OrderDetails.Select(od => new OrderProductDTO
                {
                    ProductId = od.ProductId,
                    Quantity = (int)(od.Quantity ?? 0), // Explicit conversion with default value
                    ProductName = od.Product.Name,
                    Price = (decimal)od.Product.Price, // Assuming Product has a Price property
                    Image = _context.Images
                    .Where(i => i.ProductId == od.ProductId)
                    .OrderBy(i => i.Id)
                    .Select(i => i.Image1)
                    .FirstOrDefault()
                }).ToList()
            }).ToList();

            return Ok(new ApiResponse<List<OrderDTO>> { Success = true, Message = "Orders retrieved successfully", Data = orderDTOs });
        }
    }
}
