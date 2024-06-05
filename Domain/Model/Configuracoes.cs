namespace WebAPI_Apollo.Domain.Model
{
    public class ConfigUsuario
    {
        int _id;
        bool _modoEscuro;
        static string _nomeRedeSocial = "Apollo";


        public ConfigUsuario()
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

        public static string NomeRedeSocial
        {
            get { return _nomeRedeSocial; }
            set { _nomeRedeSocial = value; }
        }
    }

    public class ConfigService
    {
        private bool _dbAtivado;
        static bool _dbFuncionando = false;

        public bool DBAtivado
        {
            get => _dbAtivado;
            set
            {
                if (_dbAtivado != value)
                {
                    _dbAtivado = value;
                    OnDBAtivadoChanged();
                }
            }
        }

        public static bool DBFuncionando
        {
            get { return _dbFuncionando; }
            set { _dbFuncionando = value; }
        }

        public event Action? DBAtivadoChanged;

        protected virtual void OnDBAtivadoChanged()
        {
            DBAtivadoChanged?.Invoke();
        }
    }
}
