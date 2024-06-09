namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IEstatisticasRepository
    {
        Task Add(Estatisticas estatisticas);
        Task<Estatisticas?> Get(int id);
        Task<Estatisticas?> GetLast();
        Task Update(Estatisticas estatisticas);
        Task Delete(Estatisticas est);
        Task DeletarReferencias(int idEstatisticas);
    }
}