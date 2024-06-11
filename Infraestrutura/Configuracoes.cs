using WebAPI_Apollo.Domain.Model;
namespace WebAPI_Apollo.Infraestrutura
{
    public class ConfigUsuario
    {
        static Usuario? _currentUser = null;
        static string _nomeRedeSocial = "Apollo";

        public static Usuario? CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static string NomeRedeSocial
        {
            get { return _nomeRedeSocial; }
            set { _nomeRedeSocial = value; }
        }
    }

    public class ConfigService
    {
    }
}
