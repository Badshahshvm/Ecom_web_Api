//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Ecomm_web_api.Data;
//using Ecomm_web_api.Models.Entity;

//namespace Ecomm_web_api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly DbContextApp _context;

//        public UserController(DbContextApp context)
//        {
//            _context = context;
//        }

//        // ✅ GET: api/user
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
//        {
//            return await _context.Users.ToListAsync();
//        }

//        // ✅ GET: api/user/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<User>> GetUser(Guid id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user == null) return NotFound();
//            return user;
//        }

//        // ✅ POST: api/user
//        [HttpPost]
//        public async Task<ActionResult<User>> CreateUser(User user)
//        {
//            user.Id = Guid.NewGuid(); // ensure unique Guid
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
//        }

//        // ✅ PUT: api/user/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateUser(Guid id, User user)
//        {
//            if (id != user.Id) return BadRequest("User ID mismatch");

//            var existingUser = await _context.Users.FindAsync(id);
//            if (existingUser == null) return NotFound();

//            // Update fields
//            existingUser.FirstName = user.FirstName;
//            existingUser.LastName = user.LastName;
//            existingUser.Email = user.Email;
//            existingUser.Password = user.Password;
//            existingUser.Address = user.Address;
//            existingUser.DOB = user.DOB;
//            existingUser.Phone = user.Phone;
//            existingUser.Status = user.Status;
//            existingUser.Type = user.Type;
//            existingUser.Fund = user.Fund;

//            _context.Entry(existingUser).State = EntityState.Modified;
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // ✅ DELETE: api/user/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteUser(Guid id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user == null) return NotFound();

//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}




using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecomm_web_api.Data;
using Ecomm_web_api.Models.Entity;

namespace Ecomm_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbContextApp _context;

        public UserController(DbContextApp context)
        {
            _context = context;
        }

        // ✅ GET: api/user/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // ✅ GET: api/user/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // ✅ POST: api/user/create
        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.Id = Guid.NewGuid(); // ensure unique Guid
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            return Ok(new
            {
                message = "User Registered successfully",
                User = user,

            });
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return NotFound();

            // Update fields (ignore user.Id)
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Address = user.Address;
            existingUser.DOB = user.DOB;
            existingUser.Phone = user.Phone;
            existingUser.Status = user.Status;
            existingUser.Type = user.Type;
            existingUser.Fund = user.Fund;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User updated successfully",
                user = existingUser
            });
        }

        // ✅ DELETE: api/user/delete/{id}
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new
            {
                message = "Login successful",
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Type
            });
        }

        [HttpPost("{userId:guid}/cart/add")]
        public async Task<IActionResult> AddToCart(Guid userId, [FromBody] AddToCartRequest request)
        {
            // 1. Validate user
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { message = "User not found" });

            // 2. Validate medicine
            var medicine = await _context.Medicines.FindAsync(request.MedicineId);
            if (medicine == null) return NotFound(new { message = "Medicine not found" });

            // 3. Check if already in cart (update qty instead of duplicate)
            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.MedicineId == request.MedicineId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    MedicineId = request.MedicineId,
                    Quantity = request.Quantity,
                    UnitPrice = medicine.UnitPrice,
                    Discount = medicine.Discount
                };

                _context.Carts.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to cart successfully" });
        }
    }

    // DTO for login request
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AddToCartRequest
    {
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}

