using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class UsuarioRepositoryRAM : IUsuarioRepository
    {
        public UsuarioRepositoryRAM() { }

        /*
          
        |//////////////////////////////////////////////|
        | Metodos relacionados com a memoria principal |
        |______________________________________________|
        
         */

        public bool verificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return VolatileContext.Usuarios.Any(usuario => usuario.UserName == usuarioInformado.UserName || usuario.Email == usuarioInformado.Email);
        }

        public void Add(Usuario usuario)
        {
            VolatileContext.Usuarios.Add(usuario);
        }

        public Usuario? Get(Guid id)
        {
            return VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public Usuario? GetSemelhanteUserName(string userName)
        {
            return VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.UserName == userName);
        }

        public Usuario? GetSemelhanteEmail(string email)
        {
            return VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.Email == email);
        }

        public void Update(Usuario usuario)
        {
            var index = VolatileContext.Usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index != -1)
            {
                VolatileContext.Usuarios[index] = usuario;
            }
        }

        public bool VerificaSeCadastrado(string email)
        {
            return VolatileContext.Usuarios.Any(usuario => usuario.Email == email);
        }

        public List<UsuarioDto> GetAll()
        {
            return VolatileContext.Usuarios.Select(usuario => new UsuarioDto(usuario.Id, usuario.Idade, usuario.XP, usuario.Level, usuario.XP_ProximoNivel, usuario.Nome, usuario.Email, usuario.Senha, usuario.Esporte, usuario.Genero, usuario.UserName, usuario.PalavraRecuperacao, usuario.DataNascimento, usuario.Peso, usuario.Altura)).ToList();
        }

        public Usuario? GetViaLogin(string email, string senha)
        {
            return VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.Email == email && usuario.Senha == senha);
        }

        public void Delete(Usuario usuario)
        {
            VolatileContext.Usuarios.Remove(usuario);
        }

        public Usuario? RecuperarSenha(string email, string palavraRecuperacao)
        {
            return VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.PalavraRecuperacao == palavraRecuperacao && usuario.Email == email);
        }
    }
}
