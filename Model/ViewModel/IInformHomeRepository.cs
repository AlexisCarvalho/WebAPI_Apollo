using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IInformHomeRepository
    {
        void Add(InformHome informHome);
        InformHome? Get(int id);
        InformHome? GetLast();
        InformHome? GetViaUsr(Guid idUsuario);
        void Update(InformHome informHome);
        InformHome? JaExiste(InformHome informHome);
        void Delete(InformHome informHome);
    }
}
