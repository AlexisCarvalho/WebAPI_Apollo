using WebAPI_Apollo.Domain.DTOs;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IUsuarioRepository
    {
        Task Add(Usuario usuario);
        Task<Usuario?> Get(Guid id);
        Task<List<UsuarioDto>> GetAll();
        Task<Usuario?> GetSemelhanteEmail(string email);
        Task<Usuario?> GetSemelhanteUserName(string userName);
        Task<List<UsuarioDto>> GetUsuariosNome(string nome);
        Task<Usuario?> GetViaLogin(string email, string senha);
        Task Update(Usuario usuario);
        Task Delete(Usuario usuario);
        Task<Usuario?> RecuperarSenha(string email, string palavraRecuperacao);
        Task<bool> VerificarSeExisteEmailUsername(Usuario usuarioInformado);
        Task<bool> VerificaSeCadastrado(string email);
    }
}