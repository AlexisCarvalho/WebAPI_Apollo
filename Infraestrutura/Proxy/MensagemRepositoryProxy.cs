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

        public async Task Add(Mensagem mensagem)
        {
            await CurrentRepository.Add(mensagem);
        }

        public async Task Delete(Mensagem mensagem)
        {
            await CurrentRepository.Delete(mensagem);
        }

        public Task<List<ChatDto>> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return CurrentRepository.EnviadasEntre(remetente, destinatario);
        }

        public Task<List<ChatDto>> EnviadasPor(Guid id)
        {
            return CurrentRepository.EnviadasPor(id);
        }

        public Task<Mensagem?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<List<ChatDto>> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Task<Mensagem?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Task<List<ChatDto>> RecebidasPor(Guid id)
        {
            return CurrentRepository.RecebidasPor(id);
        }

        public async Task Update(Mensagem mensagem)
        {
            await CurrentRepository.Update(mensagem);
        }
    }
}
