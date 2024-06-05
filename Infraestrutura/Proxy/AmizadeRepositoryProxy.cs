using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
{
    public class AmizadeRepositoryProxy : IAmizadeRepository
    {
        private readonly ConfigService _configService;
        private IAmizadeRepository _currentRepository;
        private readonly IAmizadeRepository _dbRepository;
        private readonly IAmizadeRepository _ramRepository;

        public AmizadeRepositoryProxy(ConfigService configService, IAmizadeRepository dbRepository, IAmizadeRepository ramRepository)
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

        private IAmizadeRepository CurrentRepository => _currentRepository;

        public void Add(Amizade amizade)
        {
            CurrentRepository.Add(amizade);
        }

        public void DeletarReferencias(Guid idUsuario)
        {
            CurrentRepository.DeletarReferencias(idUsuario);
        }

        public void Delete(Amizade amizade)
        {
            CurrentRepository.Delete(amizade);
        }

        public Amizade? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public List<Amizade> GetAllUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllUsr(idUsuario);
        }

        public Amizade? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public void Update(Amizade amizade)
        {
            CurrentRepository.Update(amizade);
        }

        public Amizade? VerificarAmizade(Amizade amizade)
        {
            return CurrentRepository.VerificarAmizade(amizade);
        }
    }
}
