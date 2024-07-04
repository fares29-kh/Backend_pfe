namespace pfe.models
{
    public class Image
    {
        public int Id { get; set; }
        public string? titre { get; set; }
        public string? data { get; set; }
        public int? maisonId { get; set; }
        public virtual Maison? Maison { get; set; }
        public int? chambreId { get; set; }
        public virtual Chambre? Chambre { get;set; }
    }
}
