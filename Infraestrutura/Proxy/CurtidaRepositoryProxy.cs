using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class CurtidaRepositoryProxy : ICurtidaRepository
    {
        private readonly ConfigService _configService;
        private ICurtidaRepository _currentRepository;
        private readonly ICurtidaRepository _dbRepository;
        private readonly ICurtidaRepository _ramRepository;

        public CurtidaRepositoryProxy(ConfigService configService, ICurtidaRepository dbRepository, ICurtidaRepository ramRepository)
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

        private ICurtidaRepository CurrentRepository => _currentRepository;

        public async Task Add(Curtida curtida)
        {
            await CurrentRepository.Add(curtida);
        }

        public async Task Delete(Curtida curtida)
        {
            await CurrentRepository.Delete(curtida);
        }

        public Task<Curtida?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<Curtida?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Task<Curtida?> JaCurtiu(Curtida curtida)
        {
            return CurrentRepository.JaCurtiu(curtida);
        }

        public async Task Update(Curtida curtida)
        {
            await CurrentRepository.Update(curtida);
        }
    }
}
