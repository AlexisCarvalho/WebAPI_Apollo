using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface INotificacaoRepository
    {
        Task Add(Notificacao notificacao);
        Task<Notificacao?> Get(int id);
        Task<Notificacao?> GetLast();
        Task<List<NotificacoesDaRedeDto>> GetAll();
        Task<List<NotificacoesDaRedeDto>> GetAllUsr(Guid idUsuario);
        Task<List<NotificacoesDaRedeDto>> GetAllNotiAmizadeUsr(Guid idUsuario);
        Task<List<NotificacoesDaRedeDto>> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario);
        Task Update(Notificacao notificacao);
        Task Delete(Notificacao notificacao);
        Task DeletarReferencias(Guid idUsuario);
        Task<Notificacao?> JaFoiNotificado(Notificacao notificacao);
    }
}