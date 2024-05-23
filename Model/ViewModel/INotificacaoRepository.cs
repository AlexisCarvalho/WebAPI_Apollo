using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface INotificacaoRepository
    {
        void Add(Notificacao notificacao);
        Notificacao? Get(int id);
        Notificacao? GetLast();
        void Update(Notificacao notificacao);
        Notificacao? JaFoiNotificado(Notificacao notificacao);
        void Delete(Notificacao notificacao);
    }
}
