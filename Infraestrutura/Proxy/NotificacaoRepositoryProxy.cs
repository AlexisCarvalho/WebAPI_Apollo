using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class NotificacaoRepositoryProxy : INotificacaoRepository
    {
        private readonly ConfigService _configService;
        private INotificacaoRepository _currentRepository;
        private readonly INotificacaoRepository _dbRepository;
        private readonly INotificacaoRepository _ramRepository;

        public NotificacaoRepositoryProxy(ConfigService configService, INotificacaoRepository dbRepository, INotificacaoRepository ramRepository)
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

        private INotificacaoRepository CurrentRepository => _currentRepository;

        public async Task Add(Notificacao notificacao)
        {
            await CurrentRepository.Add(notificacao);
        }

        public async Task DeletarReferencias(Guid idUsuario)
        {
            await CurrentRepository.DeletarReferencias(idUsuario);
        }

        public async Task Delete(Notificacao notificacao)
        {
            await CurrentRepository.Delete(notificacao);
        }

        public Task<Notificacao?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<List<NotificacoesDaRedeDto>> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Task<List<NotificacoesDaRedeDto>> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllEnviadasNotiAmizadeUsr(idUsuario);
        }

        public Task<List<NotificacoesDaRedeDto>> GetAllNotiAmizadeUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllNotiAmizadeUsr(idUsuario);
        }

        public Task<List<NotificacoesDaRedeDto>> GetAllUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllUsr(idUsuario);
        }

        public Task<Notificacao?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Task<Notificacao?> JaFoiNotificado(Notificacao notificacao)
        {
            return CurrentRepository.JaFoiNotificado(notificacao);
        }

        public async Task Update(Notificacao notificacao)
        {
            await CurrentRepository.Update(notificacao);
        }
    }
}
