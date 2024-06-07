﻿using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario? Get(Guid id)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public Usuario? GetSemelhanteUserName(string userName)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.UserName == userName);
        }

        public Usuario? GetSemelhanteEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email);
        }

        public void Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public bool VerificaSeCadastrado(string email)
        {
            return _context.Usuarios.Any(usuario => usuario.Email == email);
        }

        public List<UsuarioDto> GetAll()
        {
            return _context.Usuarios
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
            return _context.Usuarios
                .FirstOrDefault(usuario => usuario.Email == email
                                           && usuario.Senha == senha);
        }

        public void Delete(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public Usuario? RecuperarSenha(string email, string palavraRecuperacao)
        {
            return _context.Usuarios
                .FirstOrDefault(usuario => usuario.PalavraRecuperacao == palavraRecuperacao
                                           && usuario.Email == email);
        }

        public bool VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return _context.Usuarios
                .Any(usuario => usuario.UserName == usuarioInformado.UserName
                                || usuario.Email == usuarioInformado.Email);
        }
    }
}