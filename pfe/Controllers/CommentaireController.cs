using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;

namespace pfe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentaireController : ControllerBase
    {
        private readonly DBContext _db;
        public CommentaireController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Commentaire>> GetCommentaires()
        {
            return await _db.commentaires.ToListAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Commentaire>> GetById(int id)
        {
            var result = await _db.commentaires.FindAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Commentaire>> AddCommentaire(commentaireModel commentaireModel)   
        {
           
            var commentaire = new Commentaire
            {
               
                Contenu = commentaireModel.Contenu,
                date=commentaireModel.date,
                maisonId=commentaireModel.maisonId,
                chambreId=commentaireModel.chambreId,
                userId = commentaireModel.userId,
            };
            _db.commentaires.Add(commentaire);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = commentaire.Id }, commentaire);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutCommentaire(int id, commentaireModel commentaireModel)
        {
            var commentaire = await _db.commentaires.FindAsync(id);
            if(commentaire == null)
            {
                return NotFound();
            }
            commentaire.Contenu = commentaireModel.Contenu;
            commentaire.userId = commentaireModel.userId;
            _db.Entry(commentaire).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var commentaire = await _db.commentaires.FindAsync(id);
            if(commentaire == null)
            {
                return NotFound();
            }
            _db.commentaires.Remove(commentaire);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet]
        [Route("Room/{id}")]
        public async Task<ActionResult<Commentaire>> GetByRoomId(int id)
        {
            var commentaire = await _db.commentaires.Where(x => x.chambreId == id).ToListAsync();
            if (commentaire == null)
            {
                return NotFound();
            }
            return Ok(commentaire);
        }
        [HttpGet]
        [Route("House/{id}")]
        public async Task<ActionResult<Commentaire>> GetByHouseId(int id)
        {
            var commentaire = await _db.commentaires.Where(x => x.maisonId == id).ToListAsync();
            if (commentaire == null)
            {
                return NotFound();
            }
            return Ok(commentaire);
        }

    }
}
