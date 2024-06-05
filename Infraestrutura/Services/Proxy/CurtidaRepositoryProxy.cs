using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interface;

namespace WebAPI_Apollo.Infraestrutura.Services.Proxy
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

        public void Add(Curtida curtida, ref Post postCurtido)
        {
            CurrentRepository.Add(curtida, ref postCurtido);
        }

        public void Delete(Curtida curtida, ref Post postDescurtido)
        {
            CurrentRepository.Delete(curtida, ref postDescurtido);
        }

        public Curtida? Get(int id)
        {
            return CurrentRepository.Get(id);
        }

        public Curtida? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Curtida? JaCurtiu(Curtida curtida)
        {
            return CurrentRepository.JaCurtiu(curtida);
        }

        public void Update(Curtida curtida)
        {
            CurrentRepository.Update(curtida);
        }
    }
}
