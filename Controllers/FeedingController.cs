using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WlidLife.dData;
using WlidLife.Models;

namespace WlidLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedingController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/feeding
        [HttpGet]
        public async Task<ActionResult> GetAllSchedules()
        {
            var schedules = await _context.FeedingSchedules.ToListAsync();
            return Ok(schedules);
        }

        // GET: api/feeding/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSchedule(int id)
        {
            var schedule = await _context.FeedingSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound(new { message = $"Feeding schedule with ID {id} not found." });
            }
            return Ok(schedule);
        }

        // POST: api/feeding
        [HttpPost]
        public async Task<ActionResult<FeedingSchedule>> PostSchedule(FeedingSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FeedingSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.FeedingId }, schedule);
        }

        // PUT: api/feeding/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSchedule(int id, FeedingSchedule schedule)
        {
            if (id != schedule.FeedingId)
            {
                return BadRequest(new { message = "ID in the URL does not match the ID in the request body." });
            }

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FeedingSchedules.Any(s => s.FeedingId == id))
                {
                    return NotFound(new { message = $"Feeding schedule with ID {id} not found." });
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/feeding/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<FeedingSchedule>> DeleteSchedule(int id)
        {
            var schedule = await _context.FeedingSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound(new { message = $"Feeding schedule with ID {id} not found." });
            }

            _context.FeedingSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }

        // DELETE: api/feeding/Delete?ids=1&ids=2&ids=3
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var schedulesToDelete = await _context.FeedingSchedules.Where(s => ids.Contains(s.FeedingId)).ToListAsync();

            if (!schedulesToDelete.Any())
            {
                return NotFound(new { message = "No feeding schedules found for the provided IDs." });
            }

            _context.FeedingSchedules.RemoveRange(schedulesToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feeding schedules deleted successfully.", schedules = schedulesToDelete });
        }
    }
}
