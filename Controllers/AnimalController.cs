using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WlidLife.dData;
using WlidLife.Models;

namespace WlidLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/animal
        [HttpGet]
        public async Task<ActionResult> GetAllAnimals()
        {
            var animals = await _context.Animals.ToArrayAsync();
            return Ok(animals);
        }

        // GET: api/animal/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound(new { message = $"Animal with ID {id} not found." });
            }
            return Ok(animal);
        }

        // POST: api/animal
        [HttpPost]
        public async Task<ActionResult<Animals>> PostAnimal(Animals animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        // PUT: api/animal/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAnimal(int id, Animals animal)
        {
            if (id != animal.Id)
            {
                return BadRequest(new { message = "ID in the URL does not match the ID in the request body." });
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Animals.Any(a => a.Id == id))
                {
                    return NotFound(new { message = $"Animal with ID {id} not found." });
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/animal/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Animals>> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound(new { message = $"Animal with ID {id} not found." });
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return Ok(animal);
        }

        // DELETE: api/animal/Delete?ids=1&ids=2&ids=3
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var animalsToDelete = await _context.Animals.Where(a => ids.Contains(a.Id)).ToListAsync();

            if (!animalsToDelete.Any())
            {
                return NotFound(new { message = "No animals found for the provided IDs." });
            }

            _context.Animals.RemoveRange(animalsToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animals deleted successfully.", animals = animalsToDelete });
        }
    }
}
