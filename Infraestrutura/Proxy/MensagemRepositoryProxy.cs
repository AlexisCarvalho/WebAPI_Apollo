using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class MensagemRepositoryProxy : IMensagemRepository
    {
        private readonly ConfigService _configService;
        private IMensagemRepository _currentRepository;
        private readonly IMensagemRepository _dbRepository;
        private readonly IMensagemRepository _ramRepository;

        public MensagemRepositoryProxy(ConfigService configService, IMensagemRepository dbRepository, IMensagemRepository ramRepository)
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

        private IMensagemRepository CurrentRepository => _currentRepository;

        public void Add(Mensagem mensagem)
        {
            CurrentRepository.Add(mensagem);
        }

        public void Delete(Mensagem mensagem)
        {
            CurrentRepository.Delete(mensagem);
        }

        public List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return CurrentRepository.EnviadasEntre(remetente, destinatario);
        }

        public List<ChatDto> EnviadasPor(Guid id)
        {
            return CurrentRepository.EnviadasPor(id);
        }

        public Mensagem? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public List<ChatDto> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Mensagem? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public List<ChatDto> RecebidasPor(Guid id)
        {
            return CurrentRepository.RecebidasPor(id);
        }

        public void Update(Mensagem mensagem)
        {
            CurrentRepository.Update(mensagem);
        }
    }
}
