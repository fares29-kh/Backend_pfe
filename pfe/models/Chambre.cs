namespace pfe.models
{
    public class Chambre
    {
        public int Id { get; set; }
        public string? NomChambre { get; set; }
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public string? Description { get; set; }
        public string? LienCarte { get; set; }
        public string? LienVideo { get; set; }
        public string? Categorie { get; set; }
        public int? NbrAdulte { get; set; }
        public int? NbrEnfant { get; set; }
        public int? Prix { get; set; }
        public string? userId { get; set; }
        public virtual User? User { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Image>? Images { get; set; }
    }
}
