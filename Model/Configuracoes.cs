namespace WebAPI_Apollo.Model
{
    public class ConfUsuario
    {
        int _id;
        bool _modoEscuro;


        public ConfUsuario()
        {
            _id = 0;
            _modoEscuro = false;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool ModoEscuro
        {
            get { return _modoEscuro; }
            set { _modoEscuro = value; }
        }
    }

    public class ConfSistema
    {
        static bool _BDAtivado = false;
        static string _nomeRedeSocial = "Apollo";

        public static bool DBAtivado
        {
            get { return _BDAtivado; }
            set { _BDAtivado = value; }
        }

        public static string NomeRedeSocial
        {
            get { return _nomeRedeSocial; }
            set { _nomeRedeSocial = value; }
        }
    }
}
