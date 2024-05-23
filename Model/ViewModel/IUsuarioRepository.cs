using WebAPI_Apollo.Model.DTOs;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IUsuarioRepository
    {
        void Add(Usuario usuario);
        List<UsuarioDto> GetAll();
        Usuario? Get(Guid id);
        public bool verificarSeExisteEmailUsername(Usuario usuarioInformado);
        bool VerificaSeCadastrado(string email);
        Usuario? GetViaLogin(string email, string senha);
        Usuario? RecuperarSenha(string email, string palavraRecuperacao);
        Usuario? GetSemelhanteUserName(string userName);
        Usuario? GetSemelhanteEmail(string email);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
    }
}
