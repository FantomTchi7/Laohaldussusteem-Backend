namespace backend.Models
{
    public class Toode : Produkt
    {
        public int ArveId { get; set; }
        public virtual Arve Arve { get; set; }
        public int Kogus { get; set; }
        public int Hind { get; set; }
    }
}