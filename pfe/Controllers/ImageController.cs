using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;

namespace pfe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly DBContext _db;
        public ImageController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Image>> GetChambres()
        {
            return await _db.Image.ToListAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Image>> GetById(int id)
        {
            var image = await _db.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }
        [HttpPost]
        public async Task<ActionResult<Chambre>> AddImage(imageModel imageModel)
        {
            var image = new Image
            {
                titre = imageModel.titre,
                data = imageModel.data,
                maisonId = imageModel.maisonId,
                chambreId = imageModel.chambreId,
            };
            _db.Image.Add(image);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = image.Id }, image);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutImage(int id, imageModel imageModel)
        {
            Image? result = await _db.Image.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            result.titre = imageModel.titre;
            result.data = imageModel.data;
            result.maisonId = imageModel.maisonId;
            result.chambreId = imageModel.chambreId;
            _db.Entry(result).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var ch = await _db.Image.FindAsync(id);
            if (ch == null)
            {
                return NotFound();
            }
            _db.Image.Remove(ch);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
