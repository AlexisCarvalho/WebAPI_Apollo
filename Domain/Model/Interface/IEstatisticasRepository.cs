using WebAPI_Apollo.Domain.Model;

namespace WebAPI_Apollo.Domain.Model.Interface
{
    public interface IEstatisticasRepository
    {
        void Add(Estatisticas estatisticas);
        void DeletarReferencias(int idEstatisticas);
        void Delete(Estatisticas est);
        Estatisticas? Get(int id);
        Estatisticas? GetLast();
        void Update(Estatisticas estatisticas);
    }
}