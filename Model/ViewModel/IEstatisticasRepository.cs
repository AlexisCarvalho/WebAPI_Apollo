namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IEstatisticasRepository
    {
        void Add(Estatisticas estatisticas);
        Estatisticas? Get(int id);
        Estatisticas? GetLast();
        void Update(Estatisticas estatisticas);
        void Delete(Estatisticas id);
    }
}
