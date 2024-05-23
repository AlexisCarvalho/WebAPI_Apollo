namespace WebAPI_Apollo.Model
{
    public class Feed
    {
        private int _id;
        private int _idUsuario;
        private List<Post> _posts;
        private List<Competicao> _competicoes;

        public Feed()
        {
            _id = 0;
            _idUsuario = 0;
            _posts = [];
            _competicoes = [];
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        public List<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public List<Competicao> Competicoes
        {
            get { return _competicoes; }
            set { _competicoes = value; }
        }
    }
}