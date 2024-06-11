using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class UsuarioRepositoryRAM : IUsuarioRepository
    {

        public void Add(Usuario usuario)
        {
            VolatileContext.Usuarios.Add(usuario);
        }

        public Usuario? Get(Guid id)
        {
            return VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.Id == id);
        }

        public Usuario? GetSemelhanteUserName(string userName)
        {
            return VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.UserName == userName);
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
                ConfigUsuario.CurrentUser = usuario;
            }
        }

        public bool VerificaSeCadastrado(string email)
        {
            return VolatileContext.Usuarios.Any(usuario => usuario.Email == email);
        }

        public List<UsuarioDto> GetAll()
        {
            return VolatileContext.Usuarios
                .Select(usuario => new UsuarioDto
                (
                    usuario.Id, usuario.Idade,
                    usuario.XP, usuario.Level,
                    usuario.XP_ProximoNivel, usuario.Nome,
                    usuario.Email, usuario.Senha,
                    usuario.Esporte, usuario.Genero,
                    usuario.UserName, usuario.PalavraRecuperacao,
                    usuario.DataNascimento, usuario.Peso,
                    usuario.Altura, usuario.ImagemPerfil
                ))
                .ToList();
        }

        public Usuario? GetViaLogin(string email, string senha)
        {
            return VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.Email == email && usuario.Senha == senha);
        }

        public void Delete(Usuario usuario)
        {
            VolatileContext.Usuarios.Remove(usuario);
            ConfigUsuario.CurrentUser = null;
        }

        public Usuario? RecuperarSenha(string email, string palavraRecuperacao)
        {
            return VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.PalavraRecuperacao == palavraRecuperacao
                                           && usuario.Email == email);
        }

        public bool VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return VolatileContext.Usuarios
                .Any(usuario => usuario.UserName == usuarioInformado.UserName
                                || usuario.Email == usuarioInformado.Email);
        }

        public List<UsuarioDto> GetUsuariosNome(string nome)
        {
            var candidatos = VolatileContext.Usuarios
                .Where(usuario =>
                    usuario.Nome.Contains
                        (nome, StringComparison.CurrentCultureIgnoreCase))
                .Select(usuario => new UsuarioDto
                (
                    usuario.Id, usuario.Idade,
                    usuario.XP, usuario.Level,
                    usuario.XP_ProximoNivel, usuario.Nome,
                    usuario.Email, usuario.Senha,
                    usuario.Esporte, usuario.Genero,
                    usuario.UserName, usuario.PalavraRecuperacao,
                    usuario.DataNascimento, usuario.Peso,
                    usuario.Altura, usuario.ImagemPerfil
                ))
                .ToList();

            var resultados = candidatos
                .Where(usuario => AlgoritmosDePesquisa.SimilaridadeDeJaccard(usuario.nome, nome) > 0.6)
                .ToList();

            return resultados;
        }
    }
}
