using WebAPI_Apollo.Domain.Model;

namespace WebAPI_Apollo.Domain.Model.Interface
{
    public interface IInformHomeRepository
    {
        void Add(InformHome informHome);
        void Delete(InformHome informHome);
        InformHome? Get(int id);
        InformHome? GetLast();
        InformHome? GetViaUsr(Guid idUsuario);
        InformHome? JaExiste(InformHome informHome);
        void Update(InformHome informHome);
    }
}