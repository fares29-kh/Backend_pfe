using pfe.models;

namespace pfe.modelViews
{
    public class reservationModel
    {
        public string? userId { get; set; }
        public int? maisonId { get; set; }
        public int? chambreId { get; set; }
        public DateTime? date_debut { get; set; }
        public DateTime? date_fin { get; set; }
        public bool? isConfirmed { get; set; }

    }
}
