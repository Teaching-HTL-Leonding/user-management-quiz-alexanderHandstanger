using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "administrator")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly UserManagementDataContext context;

        public record GroupResult(int id, string name);

        public GroupController(UserManagementDataContext context)
        {
            this.context = context;
        }

        [HttpGet("su/{id}")]
        public IActionResult GetSingleGroup(int id)
        {
            var group = context.Groups.Find(id);

            if (group == null) return NotFound();

            return Ok(group);
        }

        [HttpGet("allGroups")]
        public IActionResult GetAllGroups()
        {
            var groups = context.Groups.ToArray();

            return Ok(groups);
        }
    }
}
