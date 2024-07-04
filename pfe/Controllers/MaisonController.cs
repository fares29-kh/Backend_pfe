using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;

namespace pfe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaisonController : ControllerBase
    {
        private readonly DBContext _db;
        public MaisonController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Maison>> GetMaisons()
        {
            return await _db.maisons.ToListAsync();
        }

        [HttpGet]
        [Route("HomeImages/{id}")]
        public async Task<List<Image>> getImagesByMaisonId(int id)
        {
           return await _db.Image.Where(x => x.maisonId == id).
                ToListAsync();
           
        }

        [HttpGet]
        [Route("house/{id}")]
        public async Task<List<Maison>> getMaisonByUserId(string id)
        {
            return await _db.maisons.Where(x=>x.userId == id)
              //  .Include(x=>x.Images)
                .ToListAsync();
        }

        

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Maison>> GetById(int id)
        {
            var maison = await _db.maisons.FindAsync(id);
                //.Include(x => x.Images).ToListAsync();
            if (maison == null)
            {
                return NotFound();
            }
            return Ok(maison);
        }
        [HttpPost]
        public async Task<ActionResult<Maison>> AddMaison(maisonModel maisonModel)
        {
            var maison = new Maison
            {
                NomMaison = maisonModel.NomMaison,
                Adresse = maisonModel.Adresse,
                Ville = maisonModel.Ville,
                Description = maisonModel.Description,
                LienCarte = maisonModel.LienCarte,
                LienVideo = maisonModel.LienVideo,
                Categorie = maisonModel.Categorie,
                NbrAdulte = maisonModel.NbrAdulte,
                NbrEnfant = maisonModel.NbrEnfant,
                Prix = maisonModel.Prix,
                userId = maisonModel.userId
            };
            _db.maisons.Add(maison);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = maison.Id }, maison);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutMaison(int id, maisonModel maisonModel)
        {
            Maison? result = await _db.maisons.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            result.NomMaison = maisonModel.NomMaison;
            result.Adresse = maisonModel.Adresse;
            result.Ville = maisonModel.Ville;
            result.Description = maisonModel.Description;
            result.LienVideo = maisonModel.LienVideo;
            result.LienCarte = maisonModel.LienCarte;
            result.Categorie = maisonModel.Categorie;
            result.NbrAdulte = maisonModel.NbrAdulte;
            result.NbrEnfant = maisonModel.NbrEnfant;
            result.Prix = maisonModel.Prix;
            result.userId = maisonModel.userId;
            _db.Entry(result).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteMaison(int id)
        //{
        //    var maison = await _db.maisons.FindAsync(id);
        //    if (maison == null)
        //    {
        //        return NotFound();
        //    }
        //    _db.maisons.Remove(maison);
        //    await _db.SaveChangesAsync();
        //    return NoContent();
        //}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMaison(int id)
        {
            var maison = await _db.maisons.FindAsync(id);
            if (maison == null)
            {
                return NotFound();
            }

            // Find and delete the associated image
            var image = await _db.Image.FirstOrDefaultAsync(i => i.maisonId == id);
            if (image != null)
            {
                _db.Image.Remove(image);
            }

            _db.maisons.Remove(maison);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]

        [Route("user")]
        public async Task<ActionResult<Maison>> GetByuserId(string Id)
        {
            var maison = await _db.maisons.Where(x => x.userId == Id)
                .ToListAsync();
            if (maison == null)
            {
                return NotFound();
            }
            return Ok(maison);
        }
        [HttpGet]
        [Route("stat")]
        public async Task<Array> Getstat()
        {

            int[] stat = new int[2];
            stat[0] = _db.maisons.Count();
            stat[1] = _db.chambres.Count();
            return stat;
        }

        [HttpGet]
        [Route("nombremaison")]
        public int GetNombremaison()
        {
            int result = 0;

            result = _db.maisons.Count();

            return result;
        }
        [HttpGet]
        [Route("nombrechambre")]
        public int GetNombreOwners()
        {
            int result = 0;

            result = _db.chambres.Count();

            return result;
        }

        [HttpGet]
        [Route("stat2")]
        public async Task<Array> Getstat2()
        {

            int[] stat = new int[3];
            stat[0] = _db.reservations.Count();
            stat[1] = _db.Users.Count();
            stat[2] = _db.commentaires.Count();
            return stat;
        }
    }
}