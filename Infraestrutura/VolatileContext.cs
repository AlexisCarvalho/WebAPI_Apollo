using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Infraestrutura
{
    public class VolatileContext
    {
        /* --- Entidades ---  */

        // Usuario
        static List<Usuario> usuarios = [];
        static List<Estatisticas> estatisticas = [];
        static List<InformHome> informHome = [];

        // Rede
        static List<Post> posts = [];
        static List<Mensagem> mensagens = [];
        static List<Notificacao> notificacoes = [];

        /* --- Interações entre IDs ---  */

        static List<Curtida> curtidas = [];
        static List<Amizade> amizades = [];

        public static List<Usuario> Usuarios
        {
            get { return usuarios; }
            set { usuarios = value; }
        }

        public static List<Mensagem> Mensagens
        {
            get { return mensagens; }
            set { mensagens = value; }
        }

        public static List<Estatisticas> Estatisticas
        {
            get { return estatisticas; }
            set { estatisticas = value; }
        }

        public static List<Post> Posts
        {
            get { return posts; }
            set { posts = value; }
        }

        public static List<Curtida> Curtidas
        {
            get { return curtidas; }
            set { curtidas = value; }
        }

        public static List<Amizade> Amizades
        {
            get { return amizades; }
            set { amizades = value; }
        }

        public static List<Notificacao> Notificacoes
        {
            get { return notificacoes; }
            set { notificacoes = value; }
        }

        public static List<InformHome> InformHome
        {
            get { return informHome; }
            set { informHome = value; }
        }
    }
}
