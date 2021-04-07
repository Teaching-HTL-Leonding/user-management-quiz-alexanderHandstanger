using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagement.Data;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManagementDataContext context;

        public record UserResult(int id, string nameIdentifier, string email, string? firstName, string? lastName);

        public UserController(UserManagementDataContext context)
        {
            this.context = context;
        }

        [HttpGet("csiu")]
        public IActionResult GetCurrentUser()
        {
            var currentUserId = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var result = context.Users.First(u => u.NameIdentifier == currentUserId);

            if (result != null) return Ok(result);

            return BadRequest();
        }

        [HttpGet("allUsers")]
        public IActionResult GetAllUsers()
        {
            var result = context.Users.Select(u => new UserResult(u.Id, u.NameIdentifier, u.Email, u.FirstName, u.LastName)).ToArray();

            return Ok(result);
        }

        [HttpGet("gu/{filter}")]
        public IActionResult GetUser(string filter)
        {
            var users = context.Users.Where(u => u.Email.Contains(filter) || (u.FirstName != null && u.FirstName.Contains(filter)) || (u.LastName != null && u.LastName.Contains(filter))).ToList();
            
            return Ok(users);
        }
    }
}
