using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interface;

namespace WebAPI_Apollo.Infraestrutura.Services.Proxy
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

        public void Add(Estatisticas estatisticas)
        {
            CurrentRepository.Add(estatisticas);
        }

        public void DeletarReferencias(int idEstatisticas)
        {
            CurrentRepository.DeletarReferencias(idEstatisticas);
        }

        public void Delete(Estatisticas est)
        {
            CurrentRepository.Delete(est);
        }

        public Estatisticas? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Estatisticas? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public void Update(Estatisticas estatisticas)
        {
            CurrentRepository.Update(estatisticas);
        }
    }
}
