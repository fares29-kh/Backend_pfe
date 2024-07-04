using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;


namespace pfe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChambreController : ControllerBase
    {

        private readonly DBContext _db;
        public ChambreController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Chambre>> GetChambres()
        {
            return await _db.chambres.ToListAsync();
        }
        [HttpGet]
        [Route("chambre/{id}")]
        public async Task<List<Chambre>> getChambreByUserId(string id)
        {
            return await _db.chambres.Where(x => x.userId == id)
              //  .Include(x => x.Images)
                .ToListAsync();
        }
        [HttpGet]
        [Route("images/{id}")]
        public async Task<List<Image>> getImagesByChambreId(int id)
        {
            return await _db.Image.Where(x => x.chambreId == id)
                .ToListAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Chambre>> GetById(int id)
        {
            var chambre = await _db.chambres.FindAsync(id);
            if (chambre == null)
            {
                return NotFound();
            }
            return Ok(chambre);
        } 
        [HttpPost]
        public async Task<ActionResult<Chambre>> AddChambre(chambreModel chambreModel)
        {
            var chambre = new Chambre
            {
                NomChambre = chambreModel.NomChambre,
                Adresse = chambreModel.Adresse,
                Ville = chambreModel.Ville,
                Description = chambreModel.Description,
                LienCarte = chambreModel.LienCarte,
                LienVideo = chambreModel.LienVideo,
                Categorie = chambreModel.Categorie,
                NbrAdulte = chambreModel.NbrAdulte,
                NbrEnfant = chambreModel.NbrEnfant,
                Prix = chambreModel.Prix,
                userId = chambreModel.userId
            };
            _db.chambres.Add(chambre);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = chambre.Id }, chambre);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutChambre(int id, chambreModel chambreModel)
        {
            Chambre? result = await _db.chambres.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            result.NomChambre = chambreModel.NomChambre;
            result.Adresse = chambreModel.Adresse;
            result.Ville = chambreModel.Ville;
            result.Description = chambreModel.Description;
            result.LienVideo = chambreModel.LienVideo;
            result.LienCarte = chambreModel.LienCarte;
            result.Categorie = chambreModel.Categorie;
            result.NbrAdulte = chambreModel.NbrAdulte;
            result.NbrEnfant = chambreModel.NbrEnfant;
            result.Prix = chambreModel.Prix;
            result.userId = chambreModel.userId;
            _db.Entry(result).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteChambre(int id)
        //{
        //    var chambre = await _db.chambres.FindAsync(id);
        //    if (chambre == null)
        //    {
        //        return NotFound();
        //    }
        //    _db.chambres.Remove(chambre);
        //    await _db.SaveChangesAsync();
        //    return NoContent();
        //}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            var chambre = await _db.chambres.FindAsync(id);
            if (chambre == null)
            {
                return NotFound();
            }

            // Find and delete the associated image
            var image = await _db.Image.FirstOrDefaultAsync(i => i.chambreId == id);
            if (image != null)
            {
                _db.Image.Remove(image);
            }

            _db.chambres.Remove(chambre);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<Chambre>> GetByuserId(string Id)
        {
            var ch = await _db.chambres.Where(x => x.userId == Id)
                .ToListAsync();
            if (ch == null)
            {
                return NotFound();
            }
            return Ok(ch);
        }
    }
}
