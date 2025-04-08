using API.Data;
using API.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]     //// http://localhost:5000/api/user
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]   //// http://localhost:5000/api/user
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _dataContext.User.ToListAsync();

            if (users is null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{userId}")]   //// http://localhost:5000/api/user/{userId}
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            var user = await _dataContext.User.FindAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
