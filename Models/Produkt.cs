namespace backend.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        public string Nimetus { get; set; }
        public string Ühik { get; set; }
        public int BaasHind { get; set; }
        public int LaduId { get; set; }
        public virtual Ladu? Ladu { get; set; }
    }
}
