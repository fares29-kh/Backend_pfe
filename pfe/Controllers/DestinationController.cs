using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;


namespace pfe.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DestinationController : ControllerBase
    {
        private readonly DBContext _db;
        public DestinationController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Destination>> GetDestinations()
        {
            return await _db.destinations.ToListAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Destination>> GetById(int id)
        {
            var destination = await _db.destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            return Ok(destination);
        }
        [HttpPost]
        public async Task<ActionResult<Destination>> Adddestination(destinationModel destinationModel)
        {
            var destination = new Destination
            {
                NomDestination = destinationModel.NomDestination,
                Images = destinationModel.Images,
                Information = destinationModel.Information,
                LienWikepedia = destinationModel.LienWikepedia,

            };
            _db.destinations.Add(destination);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = destination.Id }, destination);
        } 

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutDestination(int id, destinationModel destinationModel)
        {
            Destination? result = await _db.destinations.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            result.NomDestination = destinationModel.NomDestination;
            result.Images = destinationModel.Images;
            result.Information = destinationModel.Information;
            result.LienWikepedia = destinationModel.LienWikepedia;

            _db.Entry(result).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var destination = await _db.destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            _db.destinations.Remove(destination);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
