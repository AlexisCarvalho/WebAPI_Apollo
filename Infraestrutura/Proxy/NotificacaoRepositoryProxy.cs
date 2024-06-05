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

        public void Add(Notificacao notificacao)
        {
            CurrentRepository.Add(notificacao);
        }

        public void DeletarReferencias(Guid idUsuario)
        {
            CurrentRepository.DeletarReferencias(idUsuario);
        }

        public void Delete(Notificacao notificacao)
        {
            CurrentRepository.Delete(notificacao);
        }

        public Notificacao? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public List<NotificacoesDaRedeDto> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public List<NotificacoesDaRedeDto> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllEnviadasNotiAmizadeUsr(idUsuario);
        }

        public List<NotificacoesDaRedeDto> GetAllNotiAmizadeUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllNotiAmizadeUsr(idUsuario);
        }

        public List<NotificacoesDaRedeDto> GetAllUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllUsr(idUsuario);
        }

        public Notificacao? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Notificacao? JaFoiNotificado(Notificacao notificacao)
        {
            return CurrentRepository.JaFoiNotificado(notificacao);
        }

        public void Update(Notificacao notificacao)
        {
            CurrentRepository.Update(notificacao);
        }
    }
}
