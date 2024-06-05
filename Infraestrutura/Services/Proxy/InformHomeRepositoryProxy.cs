using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interface;

namespace WebAPI_Apollo.Infraestrutura.Services.Proxy
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

        public void Add(InformHome informHome)
        {
            CurrentRepository.Add(informHome);
        }

        public void Delete(InformHome informHome)
        {
            CurrentRepository.Delete(informHome);
        }

        public InformHome? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public InformHome? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public InformHome? GetViaUsr(Guid idUsuario)
        {
            return CurrentRepository.GetViaUsr(idUsuario);
        }

        public InformHome? JaExiste(InformHome informHome)
        {
            return CurrentRepository.JaExiste(informHome);
        }

        public void Update(InformHome informHome)
        {
            CurrentRepository.Update(informHome);
        }
    }
}
