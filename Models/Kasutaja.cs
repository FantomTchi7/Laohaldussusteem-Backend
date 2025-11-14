namespace backend.Models
{
    public abstract class Kasutaja
    {
        public int Id { get; set; }
        public string Nimi { get; set; }
        public KasutajaTüüp KasutajaTüüp { get; set; }
    }
}
