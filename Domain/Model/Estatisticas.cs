using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model
{
    [Table("Estatisticas")]
    public class Estatisticas
    {
        int _id;
        double _imc;
        double _aguaDiaria;

        public Estatisticas()
        {
            _id = 0;
            _imc = 0;
            _aguaDiaria = 0;
        }

        public Estatisticas(double imc, double aguaDiaria)
        {
            _id = 0;
            _imc = imc;
            _aguaDiaria = aguaDiaria;
        }

        public Estatisticas(int id, double imc, double aguaDiaria)
        {
            _id = id;
            _imc = imc;
            _aguaDiaria = aguaDiaria;
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public double IMC
        {
            get { return _imc; }
            set { _imc = value; }
        }

        public double AguaDiaria
        {
            get { return _aguaDiaria; }
            set { _aguaDiaria = value; }
        }
    }
}
