using System.Text.RegularExpressions;

namespace WebAPI_Apollo.Infraestrutura.Services
{
    public class RegexVerifier
    {
        public RegexVerifier() { }

        public static DateTime dataNoPadrão(string dataNascimento)
        {
            string datePattern = @"^\d{4}-\d{2}-\d{2}$";
            Regex regex = new(datePattern);
            DateTime dataConvertida;

            if (regex.IsMatch(dataNascimento))
            {
                string[] dataDividida = dataNascimento.Split("-");
                dataConvertida = new DateTime(
                    int.Parse(dataDividida[0]),
                    int.Parse(dataDividida[1]),
                    int.Parse(dataDividida[2])
                );
                return dataConvertida;
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}
