using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface INotificacaoRepository
    {
        void Add(Notificacao notificacao);
        void DeletarReferencias(Guid idUsuario);
        void Delete(Notificacao notificacao);
        Notificacao? Get(int id);
        List<NotificacoesDaRedeDto> GetAll();
        List<NotificacoesDaRedeDto> GetAllUsr(Guid idUsuario);
        List<NotificacoesDaRedeDto> GetAllNotiAmizadeUsr(Guid idUsuario);
        List<NotificacoesDaRedeDto> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario);
        Notificacao? GetLast();
        Notificacao? JaFoiNotificado(Notificacao notificacao);
        void Update(Notificacao notificacao);
    }
}