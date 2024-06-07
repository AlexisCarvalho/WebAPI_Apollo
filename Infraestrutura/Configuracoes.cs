﻿namespace WebAPI_Apollo.Infraestrutura
{
    public class ConfigUsuario
    {
        static Guid _currentUserId;
        bool _modoEscuro;
        static string _nomeRedeSocial = "Apollo";

        public static Guid CurrentUserId
        {
            get { return _currentUserId; }
            set { _currentUserId = value; }
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
