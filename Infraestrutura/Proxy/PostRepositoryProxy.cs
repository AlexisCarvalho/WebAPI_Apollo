using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Proxy
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

        public async Task Add(Post post)
        {
            await CurrentRepository.Add(post);
        }

        public async Task Delete(Post post)
        {
            await CurrentRepository.Delete(post);
        }

        public Task<Post?> Get(Guid id)
        {
            return CurrentRepository.Get(id);
        }

        public Task<List<PostCompletoDto>> GetAll()
        {
            return CurrentRepository.GetAll();
        }

        public Task<List<Amizade>> GetAllAmz(Guid idUsuario)
        {
            return CurrentRepository.GetAllAmz(idUsuario);
        }

        public Task<List<PostCompletoDto>> GetFeedUsr(Guid idUsuario)
        {
            return CurrentRepository.GetFeedUsr(idUsuario);
        }

        public Task<Post?> GetLast()
        {
            return CurrentRepository.GetLast();
        }

        public Task<List<PostCompletoDto>> GetPostsPesquisa(string pesquisa)
        {
            return CurrentRepository.GetPostsPesquisa(pesquisa);
        }

        public Task<List<PostCompletoDto>> PostadosPor(Guid idUsuario)
        {
            return CurrentRepository.PostadosPor(idUsuario);
        }

        public async Task Update(Post post)
        {
            await CurrentRepository.Update(post);
        }
    }
}
