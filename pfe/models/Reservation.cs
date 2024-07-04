namespace pfe.models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? userId { get; set; }
        public virtual User? User { get; set; }
        public int? maisonId { get; set; }
        public virtual Maison? Maison { get; set; }
        public int? chambreId { get; set; }
        public virtual Chambre? Chambre { get; set; }
        public DateTime? date_debut { get; set; }
        public DateTime? date_fin { get; set; }
        public bool? isConfirmed { get; set; } = false;

    }
}
