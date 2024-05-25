namespace WebAPI_Apollo.Model.ViewModel
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