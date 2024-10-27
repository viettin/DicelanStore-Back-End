using dicelanStore.DTOs;
using dicelanStore.Models.dicelanApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dicelanStore.Controllers
{
    [Authorize(Roles = "Admin")] // Ensure only users with the 'Admin' role can access
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly dicelanAppContext _context; // Replace with your actual DbContext name

        public AdminController(dicelanAppContext context)
        {
            _context = context;
        }
        // Sample GET action for Admin
        [HttpGet("dashboard")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(new { message = "Welcome to the Admin Dashboard" });
        }

        // Sample POST action
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] UserRegistrationDTO model)
        {
            if (ModelState.IsValid)
            {
                // Logic to create a new user
                return Ok(new { message = "User created successfully!" });
            }
            return BadRequest(ModelState);
        }
    }
 
}
