using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public bool verificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            var existente = _context.Usuarios.Any(usuario => usuario.UserName == usuarioInformado.UserName);
            existente = existente || _context.Usuarios.Any(usuario => usuario.Email == usuarioInformado.Email);
            return existente;
        }
        //System.Threading.Task<boll>

        public void Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario? Get(Guid id)
        {
            return _context.Usuarios.Find(id);
        }

        public Usuario? GetSemelhanteUserName(string userName)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.UserName == userName);
        }

        public void Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public bool VerificaSeCadastrado(string email)
        {
            if (_context.Usuarios.Any(usuario => usuario.Email == email))
            {
                return true;
            }
            return false;
        }

        public List<UsuarioDto> GetAll()
        {
            return _context.Usuarios
                    .Select(usuario => new UsuarioDto(usuario.Id, usuario.Idade, usuario.XP, usuario.Level, usuario.XP_ProximoNivel, usuario.Nome, usuario.Email, usuario.Senha, usuario.Esporte, usuario.Genero, usuario.UserName, usuario.PalavraRecuperacao, usuario.DataNascimento, usuario.Peso, usuario.Altura))
                    .ToList();
        }

        public Usuario? GetViaLogin(string email, string senha)
        {
            return _context.Usuarios.ToList().Find(usuario => usuario.Email == email && usuario.Senha == senha);
        }

        public void Delete(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public Usuario? RecuperarSenha(string email, string palavraRecuperacao)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.PalavraRecuperacao == palavraRecuperacao && usuario.Email == email);
        }

        public Usuario? GetSemelhanteEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email);
        }
    }
}
