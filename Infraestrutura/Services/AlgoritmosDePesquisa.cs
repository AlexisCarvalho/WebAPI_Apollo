namespace WebAPI_Apollo.Infraestrutura.Services
{
    public class AlgoritmosDePesquisa
    {
        public static double SimilaridadeDeJaccard(string s1, string s2)
        {
            var intersection = s1.Intersect(s2).Count();
            var union = s1.Union(s2).Count();

            return (double)intersection / union;
        }
    }
}
