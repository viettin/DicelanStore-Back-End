using dicelanStore.DTOs;
using dicelanStore.Models.dicelanApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using dicelanStore.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using dicelanStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace dicelanStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly dicelanAppContext _context;

        public UserController(dicelanAppContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ErrorResponse { Success = false, Message = "Invalid model state", Errors = errors });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Username or Email already exists", Errors = new List<string> { "Username or Email already exists" } });
            }
            // Create a new user entity
            var newUser = new User
            {   
                Username = model.Username,
                Email = model.Email,
                Password = HashPassword(model.Password), // You should hash the password
                PhoneNumber = "123456789",
                FullName = model.FullName,
                CreatedDate = DateTime.UtcNow,
                RoleId = 2 // You should validate if this role exists before assignment
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            var newCart = new Cart
            {
                UserId = newUser.Id,
                CreatedDate = DateTime.UtcNow
            };
            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(model.Street) 
                || !string.IsNullOrEmpty(model.City) 
                || !string.IsNullOrEmpty(model.Province) 
                || !string.IsNullOrEmpty(model.Ward) 
                ||!string.IsNullOrEmpty(model.ZipCode))
            {
                var address = new Address
                {
                    Name = model.FullName,  // Assuming Name is the full name in Address table
                    Street = model.Street,
                    City = model.City,
                    Province = model.Province,
                    Ward = model.Ward,
                    ZipCode = model.ZipCode,
                    UserId = newUser.Id,
                    // This will be the foreign key relationship
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "User registered successfully", Data = null });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !VerifyPassword(model.Password, user.Password))
            {
                return Unauthorized(new ErrorResponse { Success = false, Message = "Invalid username or password", Errors = new List<string> { "Invalid username or password" } });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("FullName", user.FullName),
                    new Claim("PhoneNumber", user.PhoneNumber)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var response = new
            {
                Token = tokenString,
                User = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FullName,
                    user.PhoneNumber,
                    user.CreatedDate,
                }
            };
            return Ok(new ApiResponse<object> { Success = true, Message = "Login successful", Data = response });
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        [HttpPost("cart/add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO model)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Cart not found", Errors = new List<string> { "Cart not found" } });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += model.Quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity
                };
                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string> { Success = true, Message = "Product added to cart successfully", Data = null });
        }

        [HttpGet("cart")]
        [Authorize]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Cart not found", Errors = new List<string> { "Cart not found" } });
            }

            var cartItems = await Task.WhenAll(cart.CartItems.Select(async ci =>
            {
                

                return new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown Product",
                    Quantity = ci.Quantity ?? 0,
                    Price = (decimal)(ci.Product?.Price ?? 0.0),
                    Image = _context.Images
                        .Where(i => i.ProductId == ci.ProductId)
                        .OrderBy(i => i.Id)
                        .Select(i => i.Image1)
                        .FirstOrDefault()
                };
            }).ToList());

            var cartItemsList = cartItems.ToList();
            return base.Ok(new ApiResponse<List<CartItemDTO>> { Success = true, Message = "Cart items retrieved successfully", Data = cartItemsList });
        }
    }
}
