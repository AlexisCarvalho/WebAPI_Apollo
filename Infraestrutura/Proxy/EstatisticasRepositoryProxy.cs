using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class EstatisticasRepositoryProxy : IEstatisticasRepository
    {
        private readonly ConfigService _configService;
        private IEstatisticasRepository _currentRepository;
        private readonly IEstatisticasRepository _dbRepository;
        private readonly IEstatisticasRepository _ramRepository;

        public EstatisticasRepositoryProxy(ConfigService configService, IEstatisticasRepository dbRepository, IEstatisticasRepository ramRepository)
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

        private IEstatisticasRepository CurrentRepository => _currentRepository;

        public async Task Add(Estatisticas estatisticas)
        {
            await CurrentRepository.Add(estatisticas);
        }

        public async Task DeletarReferencias(int idEstatisticas)
        {
            await CurrentRepository.DeletarReferencias(idEstatisticas);
        }

        public async Task Delete(Estatisticas est)
        {
            await CurrentRepository.Delete(est);
        }

        public Task<Estatisticas?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<Estatisticas?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public async Task Update(Estatisticas estatisticas)
        {
            await CurrentRepository.Update(estatisticas);
        }
    }
}
