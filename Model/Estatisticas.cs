namespace WebAPI_Apollo.Model
{
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
