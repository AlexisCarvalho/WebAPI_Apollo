namespace WebAPI_Apollo.Domain.Model
{
    public class InformHome
    {
        int _id;
        Guid _idUsuario;
        int _numNotificacoesNaoLidas;
        int _numSolicitacoesAmizade;
        int _numAmigos;
        int _numMensagensNaoLidas;

        public InformHome(Guid idUsuario, int numNotificacoesNaoLidas, int numSolicitacoesAmizade, int numAmigos, int numMensagensNaoLidas)
        {
            _idUsuario = idUsuario;
            _numNotificacoesNaoLidas = numNotificacoesNaoLidas;
            _numSolicitacoesAmizade = numSolicitacoesAmizade;
            _numAmigos = numAmigos;
            _numMensagensNaoLidas = numMensagensNaoLidas;
        }

        public InformHome(Guid idUsuario)
        {
            _idUsuario = idUsuario;
            _numNotificacoesNaoLidas = 0;
            _numSolicitacoesAmizade = 0;
            _numAmigos = 0;
            _numMensagensNaoLidas = 0;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public Guid IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }
        public int NumNotificacoesNaoLidas
        {
            get { return _numNotificacoesNaoLidas; }
            set { _numNotificacoesNaoLidas = value; }
        }
        public int NumSolicitacoesAmizade
        {
            get { return _numSolicitacoesAmizade; }
            set { _numSolicitacoesAmizade = value; }
        }
        public int NumAmigos
        {
            get { return _numAmigos; }
            set { _numAmigos = value; }
        }
        public int NumMensagensNaoLidas
        {
            get { return _numMensagensNaoLidas; }
            set { _numMensagensNaoLidas = value; }
        }
    }
}
