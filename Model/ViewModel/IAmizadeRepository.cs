using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IAmizadeRepository
    {
        void Add(Amizade amizade);
        void DeletarReferencias(Guid idUsuario);
        void Delete(Amizade amizade);
        Amizade? Get(int id);
        List<Amizade> GetAllUsr(Guid idUsuario);
        Amizade? GetLast();
        void Update(Amizade amizade);
        Amizade? VerificarAmizade(Amizade amizade);
    }
}