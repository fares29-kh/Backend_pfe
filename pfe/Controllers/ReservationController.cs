using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;
using System.Security.Cryptography.Xml;

namespace pfe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly DBContext _db;
        public ReservationController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<List<Reservation>> GetReservations()
        {
            return await _db.reservations.ToListAsync();
        }
        //[HttpGet]
        //[Route("owner/{id}")]
        //public async Task<List<reservationEvent>> GetReservationByOwnerId(string id)
        //{
        //    List<reservationEvent> reservations = new List<reservationEvent>();
        //    var maisons = await _db.maisons.Where(x=>x.userId == id)
        //        .Include(x=>x.Reservations)
        //        .ToListAsync();
        //    foreach(var maison in maisons)
        //    {
        //        List<Reservation> lst = await _db.reservations.Where(x => x.maisonId == maison.Id)
        //            .Include(x=>x.User)
        //            .Include(x=>x.Maison)
        //            .Include(x=>x.Chambre)
        //            .ToListAsync();
        //        foreach(var reservation in lst)
        //        {
        //            reservationEvent model = new reservationEvent
        //            {
        //                Id = reservation.Id,
        //                date_debut = reservation.date_debut,
        //                date_fin = reservation.date_fin,
        //                maisonId = reservation.maisonId,
        //                chambreId = reservation.chambreId,
        //                username = reservation.User?.UserName,
        //                maisonName = reservation.Maison?.NomMaison,
        //                chambreName = reservation.Chambre?.NomChambre,
        //                isConfirmed = reservation.isConfirmed,
        //                userId = reservation.userId,
        //            };
        //            reservations.Add(model);
        //        }
        //    }
        //    return reservations;
        //}
        [HttpGet]
        [Route("owner/{id}")]
        public async Task<List<reservationEvent>> GetReservationByOwnerId(string id)
        {
            List<reservationEvent> reservations = new List<reservationEvent>();

            var maisons = await _db.maisons
                .Where(x => x.userId == id)
                .Include(x => x.Reservations)
                .ToListAsync();

            var chambres = await _db.chambres
                .Where(x => x.userId == id)
                .Include(x => x.Reservations)
                .ToListAsync();

            foreach (var maison in maisons)
            {
                foreach (var reservation in maison.Reservations)
                {
                    reservationEvent model = new reservationEvent
                    {
                        Id = reservation.Id,
                        date_debut = reservation.date_debut,
                        date_fin = reservation.date_fin,
                        maisonId = reservation.maisonId,
                        chambreId = reservation.chambreId,
                        username = reservation.User?.UserName,
                        maisonName = reservation.Maison?.NomMaison,
                        chambreName = reservation.Chambre?.NomChambre,
                        isConfirmed = reservation.isConfirmed,
                        userId = reservation.userId,
                    };

                    reservations.Add(model);
                }
            }

            foreach (var chambre in chambres)
            {
                foreach (var reservation in chambre.Reservations)
                {
                    reservationEvent model = new reservationEvent
                    {
                        Id = reservation.Id,
                        date_debut = reservation.date_debut,
                        date_fin = reservation.date_fin,
                        maisonId = reservation.maisonId,
                        chambreId = reservation.chambreId,
                        username = reservation.User?.UserName,
                        maisonName = reservation.Maison?.NomMaison,
                        chambreName = reservation.Chambre?.NomChambre,
                        isConfirmed = reservation.isConfirmed,
                        userId = reservation.userId,
                    };

                    reservations.Add(model);
                }
            }

            return reservations;
        }


        [HttpGet]
        [Route("client/{id}")]
        public async Task<ActionResult<Maison>> GetReservationByclientId(string id)
        {
            List<reservationEvent> list = new List<reservationEvent>();

            List<Reservation> reservations = await _db.reservations
                .Where(x => x.userId == id)
                .Include(x => x.User)
                .Include(x => x.Maison)
                .Include(x => x.Chambre)
                .ToListAsync();
            foreach (var reservation in reservations)
            {
                reservationEvent model = new reservationEvent
                {
                    Id = reservation.Id,
                    date_debut = reservation.date_debut,
                    date_fin = reservation.date_fin,
                    maisonId = reservation.maisonId,
                    chambreId = reservation.chambreId,
                    username = reservation.User?.UserName,
                    maisonName = reservation.Maison?.NomMaison,
                    chambreName = reservation.Chambre?.NomChambre,
                    isConfirmed = reservation.isConfirmed,
                    userId = reservation.userId,
                };
                list.Add(model);
            }

            return Ok(list);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Reservation>> GetById(int id)
        {
            var model = await _db.reservations.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Reservation>> AddReservation(reservationModel reservationModel)
        {
            var reservation = new Reservation
            {
                userId = reservationModel.userId,
                maisonId = reservationModel?.maisonId,
                chambreId = reservationModel?.chambreId,
                date_debut = reservationModel?.date_debut,
                date_fin = reservationModel?.date_fin,
                isConfirmed = reservationModel?.isConfirmed
            };
            _db.reservations.Add(reservation);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }



        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutReservation(int id, reservationModel reservationModel)
        {
            Reservation? result = await _db.reservations.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            result.userId = reservationModel.userId;
            result.maisonId = reservationModel?.maisonId;
            result.chambreId = reservationModel?.chambreId;
            result.date_fin = reservationModel?.date_fin;
            result.date_debut = reservationModel?.date_debut;
            result.isConfirmed = reservationModel?.isConfirmed;
            _db.Entry(result).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var ch = await _db.reservations.FindAsync(id);
            if (ch == null)
            {
                return NotFound();
            }
            _db.reservations.Remove(ch);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
