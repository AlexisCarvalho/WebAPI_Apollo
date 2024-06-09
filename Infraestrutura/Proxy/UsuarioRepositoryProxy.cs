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

        public async Task Add(Usuario usuario)
        {
            await CurrentRepository.Add(usuario);
        }

        public async Task Delete(Usuario usuario)
        {
            await CurrentRepository.Delete(usuario);
        }

        public Task<Usuario?> Get(Guid id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<List<UsuarioDto>> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Task<Usuario?> GetSemelhanteEmail(string email)
        {
            return CurrentRepository.GetSemelhanteEmail(email);
        }

        public Task<Usuario?> GetSemelhanteUserName(string userName)
        {
            return CurrentRepository.GetSemelhanteUserName(userName);
        }

        public Task<Usuario?> GetViaLogin(string email, string senha)
        {
            return CurrentRepository.GetViaLogin(email, senha);
        }

        public Task<Usuario?> RecuperarSenha(string email, string palavraRecuperacao)
        {
            return CurrentRepository.RecuperarSenha(email, palavraRecuperacao);
        }

        public async Task Update(Usuario usuario)
        {
            await CurrentRepository.Update(usuario);
        }

        public Task<bool> VerificarSeExisteEmailUsername(Usuario usuarioInformado)
        {
            return CurrentRepository.VerificarSeExisteEmailUsername(usuarioInformado);
        }

        public Task<bool> VerificaSeCadastrado(string email)
        {
            return CurrentRepository.VerificaSeCadastrado(email);
        }

        public Task<List<UsuarioDto>> GetUsuariosNome(string nome)
        {
            return CurrentRepository.GetUsuariosNome(nome);
        }
    }
}
