namespace WebAPI_Apollo.Model
{
    public class Esportes
    {
        private int _id;
        private string _tipoEsporte;
        // Referencia ao icone aqui

        public Esportes()
        {
            _id = 0;
            _tipoEsporte = string.Empty;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string TipoEsporte
        {
            get { return _tipoEsporte; }
            set { _tipoEsporte = value; }
        }
    }
}
