using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace JWTAuthentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskByUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskByUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskByUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskByUser>>> GetTaskByUsers()
        {
            return await _context.TaskByUsers.ToListAsync();
        }

        // GET: api/TaskByUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskByUser>> GetTaskByUser(int id)
        {
            var taskByUser = await _context.TaskByUsers.FindAsync(id);

            if (taskByUser == null)
            {
                return NotFound();
            }

            return taskByUser;
        }

        // PUT: api/TaskByUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskByUser(int id, TaskByUser taskByUser)
        {
            if (id != taskByUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskByUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskByUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskByUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TaskByUser>> PostTaskByUser(TaskByUser taskByUser)
        {
            _context.TaskByUsers.Add(taskByUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskByUser", new { id = taskByUser.Id }, taskByUser);
        }

        // DELETE: api/TaskByUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskByUser>> DeleteTaskByUser(int id)
        {
            var taskByUser = await _context.TaskByUsers.FindAsync(id);
            if (taskByUser == null)
            {
                return NotFound();
            }

            _context.TaskByUsers.Remove(taskByUser);
            await _context.SaveChangesAsync();

            return taskByUser;
        }

        private bool TaskByUserExists(int id)
        {
            return _context.TaskByUsers.Any(e => e.Id == id);
        }
    }
}
