namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IInformHomeRepository
    {
        Task Add(InformHome informHome);
        Task<InformHome?> Get(int id);
        Task<InformHome?> GetLast();
        Task<InformHome?> GetViaUsr(Guid idUsuario);
        Task Update(InformHome informHome);
        Task Delete(InformHome informHome);
        Task<InformHome?> JaExiste(InformHome informHome);
    }
}