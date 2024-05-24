using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IAmizadeRepository
    {
        void Add(Amizade amizade);
        Amizade? Get(int id);
        Amizade? GetLast();
        void Update(Amizade amizade);
        Amizade? JaEAmigo(Amizade amizade);
        void Delete(Amizade amizade);
        void DeletarReferencias(Guid idUsuario);
    }
}
