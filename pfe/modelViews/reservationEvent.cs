namespace pfe.modelViews
{
    public class reservationEvent
    {
        public int Id { get; set; }
        public string? username { get; set; }
        public int? maisonId { get; set; }
        public int? chambreId { get; set; }
        public string? maisonName { get; set; }
        public string? chambreName { get; set; }
        public string? userId { get; set; }
        public DateTime? date_debut { get; set; }
        public DateTime? date_fin { get; set; }
        public bool? isConfirmed { get; set; }
    }
}
