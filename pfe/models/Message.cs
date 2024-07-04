namespace pfe.models
{
    public class Message
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? text { get; set; }
        public DateTime? date { get; set; }
        public string? userId { get; set; }
        public virtual User? User { get; set; }
    }
}
