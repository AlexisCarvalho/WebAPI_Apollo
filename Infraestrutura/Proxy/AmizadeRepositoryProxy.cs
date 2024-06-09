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

        public async Task Add(Amizade amizade)
        {
            await CurrentRepository.Add(amizade);
        }

        public async Task DeletarReferencias(Guid idUsuario)
        {
            await CurrentRepository.DeletarReferencias(idUsuario);
        }

        public async Task Delete(Amizade amizade)
        {
            await CurrentRepository.Delete(amizade);
        }

        public Task<Amizade?> Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<List<Amizade>> GetAllUsr(Guid idUsuario)
        {
            return CurrentRepository.GetAllUsr(idUsuario);
        }

        public Task<Amizade?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public async Task Update(Amizade amizade)
        {
            await CurrentRepository.Update(amizade);
        }

        public Task<Amizade?> VerificarAmizade(Amizade amizade)
        {
            return CurrentRepository.VerificarAmizade(amizade);
        }
    }
}
