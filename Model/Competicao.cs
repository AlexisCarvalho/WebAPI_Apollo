namespace WebAPI_Apollo.Model
{
    public class Competicao // Herdara do Post no futuro
    {
        private string _local;
        private string _esporte;
        private DateTime _data;

        public Competicao()
        {
            _local = string.Empty;
            _esporte = string.Empty;
            _data = new DateTime();
        }

        public string Local
        {
            get { return _local; }
            set { _local = value; }
        }

        public string Esporte
        {
            get { return _esporte; }
            set { _esporte = value; }
        }

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
