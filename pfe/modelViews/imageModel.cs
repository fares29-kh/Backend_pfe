using pfe.models;

namespace pfe.modelViews
{
    public class imageModel
    {
        public int Id { get; set; }
        public string? titre { get; set; }
        public string? data { get; set; }
        public int? maisonId { get; set; }
        public int? chambreId { get; set; }
    }
}
