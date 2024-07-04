using Microsoft.AspNetCore.Identity;

namespace pfe.models
{
    public class Commentaire
    {
        public int Id { get; set; }
        public string? Contenu { get; set; }
        public DateTime? date { get; set; }
        public string? userId { get; set; }
        public int? maisonId { get; set; }
        public virtual Maison? Maison { get; set; }
        public int? chambreId { get; set; }
        public virtual Chambre? Chambre { get; set; }
        //public virtual User? User { get; set; }

    }
}
