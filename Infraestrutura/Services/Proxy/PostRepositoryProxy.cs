using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interface;

namespace WebAPI_Apollo.Infraestrutura.Services.Proxy
{
    public class PostRepositoryProxy : IPostRepository
    {
        private readonly ConfigService _configService;
        private IPostRepository _currentRepository;
        private readonly IPostRepository _dbRepository;
        private readonly IPostRepository _ramRepository;

        public PostRepositoryProxy(ConfigService configService, IPostRepository dbRepository, IPostRepository ramRepository)
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

        private IPostRepository CurrentRepository => _currentRepository;

        public void Add(Post post)
        {
            CurrentRepository.Add(post);
        }

        public void Delete(Post post)
        {
            CurrentRepository.Delete(post);
        }

        public Post? Get(Guid id)
        {
            return CurrentRepository.Get(id);
        }

        public List<PostCompletoDto> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public List<Amizade> GetAllAmz(Guid idUsuario)
        {
            return CurrentRepository.GetAllAmz(idUsuario);
        }

        public List<PostCompletoDto> GetFeedUsr(Guid idUsuario)
        {
            return CurrentRepository.GetFeedUsr(idUsuario);
        }

        public Post? GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public List<PostCompletoDto> GetPostsPesquisa(string pesquisa)
        {
            return CurrentRepository.GetPostsPesquisa(pesquisa);
        }

        public List<PostCompletoDto> PostadosPor(Guid idUsuario)
        {
            return CurrentRepository.PostadosPor(idUsuario);
        }

        public void Update(Post post)
        {
            CurrentRepository.Update(post);
        }
    }
}
