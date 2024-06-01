using WebAPI_Apollo.Model.DTOs;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IUsuarioRepository
    {
        void Add(Usuario usuario);
        void Delete(Usuario usuario);
        Usuario? Get(Guid id);
        List<UsuarioDto> GetAll();
        Usuario? GetSemelhanteEmail(string email);
        Usuario? GetSemelhanteUserName(string userName);
        Usuario? GetViaLogin(string email, string senha);
        Usuario? RecuperarSenha(string email, string palavraRecuperacao);
        void Update(Usuario usuario);
        bool VerificarSeExisteEmailUsername(Usuario usuarioInformado);
        bool VerificaSeCadastrado(string email);
    }
}