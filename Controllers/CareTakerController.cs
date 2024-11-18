using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WlidLife.dData;
using WlidLife.Models;

namespace WlidLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareTakerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CareTakerController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated(); nameof 
        }

        // GET: api/caretaker
        [HttpGet]
        public async Task<ActionResult> GetAllCareTakers()
        {
            var caretakers = await _context.Caretaker.ToListAsync();
            return Ok(caretakers);
        }

        // GET: api/caretaker/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCaretaker(int id)
        {
            var caretaker = await _context.Caretaker.FindAsync(id);
            if (caretaker == null)
            {
                return NotFound(new { message = $"Caretaker with ID {id} not found." });
            }
            return Ok(caretaker);
        }

        // POST: api/caretaker
        [HttpPost]
        public async Task<ActionResult<Caretaker>> PostCaretaker(Caretaker caretaker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Caretaker.Add(caretaker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCaretaker), new { id = caretaker.Id }, caretaker);
        }

        // PUT: api/caretaker/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCaretaker(int id, Caretaker caretaker)
        {
            if (id != caretaker.Id)
            {
                return BadRequest(new { message = "ID in the URL does not match the ID in the request body." });
            }

            _context.Entry(caretaker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Caretaker.Any(c => c.Id == id))
                {
                    return NotFound(new { message = $"Caretaker with ID {id} not found." });
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/caretaker/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Caretaker>> DeleteCaretaker(int id)
        {
            var caretaker = await _context.Caretaker.FindAsync(id);
            if (caretaker == null)
            {
                return NotFound(new { message = $"Caretaker with ID {id} not found." });
            }

            _context.Caretaker.Remove(caretaker);
            await _context.SaveChangesAsync();

            return Ok(caretaker);
        }

        // DELETE: api/caretaker/Delete?ids=1&ids=2&ids=3
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var caretakersToDelete = await _context.Caretaker.Where(c => ids.Contains(c.Id)).ToListAsync();

            if (!caretakersToDelete.Any())
            {
                return NotFound(new { message = "No caretakers found for the provided IDs." });
            }

            _context.Caretaker.RemoveRange(caretakersToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Caretakers deleted successfully.", caretakers = caretakersToDelete });
        }
    }
}
