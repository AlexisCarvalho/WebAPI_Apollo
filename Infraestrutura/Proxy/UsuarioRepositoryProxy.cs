using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class UsuarioRepositoryProxy : IUsuarioRepository
    {
        private readonly ConfigService _configService;
        private IUsuarioRepository _currentRepository;
        private readonly IUsuarioRepository _dbRepository;
        private readonly IUsuarioRepository _ramRepository;

        public UsuarioRepositoryProxy(ConfigService configService, IUsuarioRepository dbRepository, IUsuarioRepository ramRepository)
        {
            _configService = configService;
            _dbRepository = dbRepository;
            _ramRepository = ramRepository;

            _currentRepository = _configService.DBAtivado ? _dbRepository : _ramRepository;

            _configService.DBAtivadoChanged += OnDBAtivadoChanged;
        }

        private void OnDBAtivadoChanged()
        {
            _currentRepository = _configService.DBAtivado ? _dbRepository : _ramRepository;
        }

        private IUsuarioRepository CurrentRepository => _currentRepository;

        public void Add(Usuario usuario)
        {
            CurrentRepository.Add(usuario);
        }

        public void Delete(Usuario usuario)
        {
            CurrentRepository.Delete(usuario);
        }

        public Usuario? Get(Guid id)
        {
            return CurrentRepository.Get(id);
        }

        public List<UsuarioDto> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Usuario? GetSemelhanteEmail(string email)
        {
            return CurrentRepository.GetSemelhanteEmail(email);
        }

        public Usuario? GetSemelhanteUserName(string userName)
        {
            return CurrentRepository.GetSemelhanteUserName(userName);
        }

        public Usuario? GetViaLogin(string email, string senha)
        {
            return CurrentRepository.GetViaLogin(email, senha);
        }

        public Usuario? RecuperarSenha(string email, string palavraRecuperacao)
        {
            return CurrentRepository.RecuperarSenha(email, palavraRecuperacao);
        }

        public void Update(Usuario usuario)
        {
            CurrentRepository.Update(usuario);
        }

        public bool VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return CurrentRepository.VerificarSeExisteEmailUsername(usuarioInformado);
        }

        public bool VerificaSeCadastrado(string email)
        {
            return CurrentRepository.VerificaSeCadastrado(email);
        }

        public List<UsuarioDto> GetUsuariosNome(string nome)
        {
            return CurrentRepository.GetUsuariosNome(nome);
        }
    }
}
