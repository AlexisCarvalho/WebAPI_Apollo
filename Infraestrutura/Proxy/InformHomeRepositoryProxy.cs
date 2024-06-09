using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class InformHomeRepositoryProxy : IInformHomeRepository
    {
        private readonly ConfigService _configService;
        private IInformHomeRepository _currentRepository;
        private readonly IInformHomeRepository _dbRepository;
        private readonly IInformHomeRepository _ramRepository;

        public InformHomeRepositoryProxy(ConfigService configService, IInformHomeRepository dbRepository, IInformHomeRepository ramRepository)
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

        private IInformHomeRepository CurrentRepository => _currentRepository;

        public async Task Add(InformHome informHome)
        {
            await CurrentRepository.Add(informHome);
        }

        public async Task Delete(InformHome informHome)
        {
            await CurrentRepository.Delete(informHome);
        }

        public Task<InformHome?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<InformHome?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Task<InformHome?> GetViaUsr(Guid idUsuario)
        {
            return CurrentRepository.GetViaUsr(idUsuario);
        }

        public Task<InformHome?> JaExiste(InformHome informHome)
        {
            return CurrentRepository.JaExiste(informHome);
        }

        public async Task Update(InformHome informHome)
        {
            await CurrentRepository.Update(informHome);
        }
    }
}
