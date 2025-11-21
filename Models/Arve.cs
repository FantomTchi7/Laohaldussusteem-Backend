namespace backend.Models
{
    public class Arve
    {
        public int Id { get; set; }
        public DateTime KoostatudKuupäev { get; set; }
        public DateTime Maksetähtaeg { get; set; }

        public int KoostajaId { get; set; }
        public virtual Koostaja? Koostaja { get; set; }

        public int TellijaId { get; set; }
        public virtual Tellija? Tellija { get; set; }

        public int EttevõteId { get; set; }
        public virtual Ettevõte? Ettevõte { get; set; }

        public virtual ICollection<Toode> Tooted { get; set; }

        public int SummaKäibemaksuta { get; set; }
        public int Käibemaksumäär { get; set; }
        public int SummaKäibemaksuga { get; set; }

        public Arve()
        {
            Tooted = new HashSet<Toode>();
        }
    }
}