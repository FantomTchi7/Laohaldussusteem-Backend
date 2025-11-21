namespace backend.Models
{
    public abstract class Kasutaja
    {
        public int Id { get; set; }
        public string Nimi { get; set; }
        public KasutajaTüüp KasutajaTüüp { get; set; }
        public string Email { get; set; }
        public string Parool { get; set; }
    }
}
