using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> Get(Guid id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<List<UsuarioDto>> GetAll()
        {
            return await _context.Usuarios
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
                .ToListAsync();
        }

        public async Task<Usuario?> GetSemelhanteEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Email == email);
        }

        public async Task<Usuario?> GetSemelhanteUserName(string userName)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.UserName == userName);
        }

        public async Task<List<UsuarioDto>> GetUsuariosNome(string nome)
        {
            var candidatos = await _context.Usuarios
                .Where(usuario => 
                    usuario.Nome.ToLower().Contains
                        (nome.ToLower()))
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
                .ToListAsync();

            var resultados = candidatos
                .Where(usuarios => AlgoritmosDePesquisa.SimilaridadeDeJaccard(usuarios.nome, nome) > 0.4)
                .ToList();

            return resultados;
        }

        public async Task<Usuario?> GetViaLogin(string email, string senha)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Email == email
                                           && usuario.Senha == senha);
        }

        public async Task Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            ConfigUsuario.CurrentUser = usuario;
        }

        public async Task Delete(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            ConfigUsuario.CurrentUser = null;
        }

        public async Task<Usuario?> RecuperarSenha(string email, string palavraRecuperacao)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.PalavraRecuperacao == palavraRecuperacao
                                           && usuario.Email == email);
        }

        public async Task<bool> VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return await _context.Usuarios
                .AnyAsync(usuario => usuario.UserName == usuarioInformado.UserName
                                || usuario.Email == usuarioInformado.Email);
        }

        public async Task<bool> VerificaSeCadastrado(string email)
        {
            return await _context.Usuarios
                .AnyAsync(usuario => usuario.Email == email);
        }
    }
}
