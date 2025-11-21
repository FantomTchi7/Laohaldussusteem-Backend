using backend.Models;

namespace backend.Helpers
{
    public static class ArveHelper
    {
        public static void CalculateTotals(Arve arve)
        {
            if (arve == null) return;

            if (arve.Tooted == null || !arve.Tooted.Any())
            {
                arve.SummaKäibemaksuta = 0;
                arve.SummaKäibemaksuga = 0;
                return;
            }

            long sumNoVat = arve.Tooted.Sum(t => (long)t.Hind * t.Kogus);
            arve.SummaKäibemaksuta = (int)sumNoVat;

            double taxAmount = arve.SummaKäibemaksuta * (arve.Käibemaksumäär / 100.0);

            int roundedTax = (int)Math.Round(taxAmount, MidpointRounding.AwayFromZero);

            arve.SummaKäibemaksuga = arve.SummaKäibemaksuta + roundedTax;
        }
    }
}