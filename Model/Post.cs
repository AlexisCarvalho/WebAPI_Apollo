namespace WebAPI_Apollo.Model
{
    public class Post
    {
        private Guid _id;
        private Guid _idUsuario;
        private int _numCurtidas;
        private int _numComentarios;
        private string _titulo;
        private string _descricao;
        private string _caminhoImagem; // Para memória mudar depois
        private DateTime _timeStamp;

        /*
        public Post(string titulo, string caminhoImagem, Guid idUsuario)
        {
            _id = Guid.NewGuid();
            _idUsuario = idUsuario;
            _numCurtidas = 0;
            _numComentarios = 0;
            _titulo = titulo;
            _descricao = "";
            _caminhoImagem = caminhoImagem;
        }
        */

        public Post(Guid idUsuario, string titulo, string descricao)
        {
            _id = Guid.NewGuid();
            _timeStamp = DateTime.Now;
            _idUsuario = idUsuario;
            _numCurtidas = 0;
            _numComentarios = 0;
            _titulo = titulo;
            _descricao = descricao;
            _caminhoImagem = "";
        }

        public Post(Guid idUsuario, string titulo, string descricao, string caminhoImagem)
        {
            _id = Guid.NewGuid();
            _timeStamp = DateTime.Now;
            _idUsuario = idUsuario;
            _numCurtidas = 0;
            _numComentarios = 0;
            _titulo = titulo;
            _descricao = descricao;
            _caminhoImagem = caminhoImagem;
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Guid IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        public int NumCurtidas
        {
            get { return _numCurtidas; }
            set { _numCurtidas = value; }
        }

        public int NumComentarios
        {
            get { return _numComentarios; }
            set { _numComentarios = value; }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }

        public string Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }

        public string CaminhoImagem
        {
            get { return _caminhoImagem; }
            set { _caminhoImagem = value; }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}

