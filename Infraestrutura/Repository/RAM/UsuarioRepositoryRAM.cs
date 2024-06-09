using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class UsuarioRepositoryRAM : IUsuarioRepository
    {

        public async Task Add(Usuario usuario)
        {
            await Task.Run(() =>
            {
                VolatileContext.Usuarios.Add(usuario);
            });
        }

        public Task<Usuario?> Get(Guid id)
        {
            var resultado = VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.Id == id);

            return Task.FromResult(resultado);
        }

        public Task<Usuario?> GetSemelhanteUserName(string userName)
        {
            var resultado = VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.UserName == userName);

            return Task.FromResult(resultado);
        }

        public Task<Usuario?> GetSemelhanteEmail(string email)
        {
            var resultado = VolatileContext.Usuarios.FirstOrDefault(usuario => usuario.Email == email);

            return Task.FromResult(resultado);
        }

        public async Task Update(Usuario usuario)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Usuarios.FindIndex(u => u.Id == usuario.Id);
                if (index != -1)
                {
                    VolatileContext.Usuarios[index] = usuario;
                    ConfigUsuario.CurrentUser = usuario;
                }
            });
        }

        public Task<bool> VerificaSeCadastrado(string email)
        {
            var resultado = VolatileContext.Usuarios.Any(usuario => usuario.Email == email);

            return Task.FromResult(resultado);
        }

        public Task<List<UsuarioDto>> GetAll()
        {
            var resultado = VolatileContext.Usuarios
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

            return Task.FromResult(resultado);
        }

        public Task<Usuario?> GetViaLogin(string email, string senha)
        {
            var resultado = VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.Email == email && usuario.Senha == senha);

            return Task.FromResult(resultado);
        }

        public async Task Delete(Usuario usuario)
        {
            await Task.Run(() =>
            {
                VolatileContext.Usuarios.Remove(usuario);
                ConfigUsuario.CurrentUser = null;
            });
        }

        public Task<Usuario?> RecuperarSenha(string email, string palavraRecuperacao)
        {
            var resultado = VolatileContext.Usuarios
                .FirstOrDefault(usuario => usuario.PalavraRecuperacao == palavraRecuperacao
                                           && usuario.Email == email);

            return Task.FromResult(resultado);
        }

        public Task<bool> VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            var resultado = VolatileContext.Usuarios
                .Any(usuario => usuario.UserName == usuarioInformado.UserName
                                || usuario.Email == usuarioInformado.Email);

            return Task.FromResult(resultado);
        }

        public Task<List<UsuarioDto>> GetUsuariosNome(string nome)
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

            return Task.FromResult(resultados);
        }
    }
}
