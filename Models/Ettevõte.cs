namespace backend.Models
{
    public class Ettevõte
    {
        public int Id { get; set; }
        public string Nimi { get; set; }
        public string Aadress { get; set; }
        public string Registrikood { get; set; }
        public string SwedbankKonto { get; set; }
        public string SebKonto { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
    }
}
