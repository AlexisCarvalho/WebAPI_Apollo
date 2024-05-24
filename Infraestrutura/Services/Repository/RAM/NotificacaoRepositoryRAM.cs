using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class NotificacaoRepositoryRAM : INotificacaoRepository
    {
        public void Add(Notificacao notificacao)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaNotificacao = GetLast();

            if (ultimaNotificacao != null)
            {
                notificacao.Id = ultimaNotificacao.Id + 1;
            }
            else
            {
                notificacao.Id = 1;
            }
            // Visa manter o uso de id int ao invés de trocar pra Guid

            VolatileContext.Notificacoes.Add(notificacao);
        }

        public Notificacao? JaFoiNotificado(Notificacao notificacao)
        {
            return VolatileContext.Notificacoes
                .FirstOrDefault(notificacoesNoBanco =>
                notificacoesNoBanco.Remetente == notificacao.Remetente &&
                notificacoesNoBanco.Destinatario == notificacao.Destinatario &&
                notificacoesNoBanco.TipoDeNotificacao == notificacao.TipoDeNotificacao); 
        }

        public Notificacao? Get(int id)
        {
            return VolatileContext.Notificacoes.FirstOrDefault(e => e.Id == id);
        }

        public List<NotificacoesDaRedeDto> GetAll()
        {
            return VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new NotificacoesDaRedeDto(noti.Remetente, noti.Destinatario, noti.TipoDeNotificacao, noti.MensagemDaNotificacao))
                .ToList();
        }

        public List<NotificacoesDaRedeDto> GetAllUsr(Guid idUsuario)
        {
            return VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new NotificacoesDaRedeDto(noti.Remetente, noti.Destinatario, noti.TipoDeNotificacao, noti.MensagemDaNotificacao))
                .Where(noti => noti.destinatario == idUsuario)
                .ToList();
        }

        public Notificacao? GetLast()
        {
            return VolatileContext.Notificacoes.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Notificacao notificacao)
        {
            var index = VolatileContext.Notificacoes.FindIndex(e => e.Id == notificacao.Id);
            if (index != -1)
            {
                VolatileContext.Notificacoes[index] = notificacao;
            }
        }

        public void Delete(Notificacao notificacao)
        {
            VolatileContext.Notificacoes.Remove(notificacao);
        }

        public void DeletarReferencias(Guid idUsuario)
        {
            var notificacoesDoUsr = VolatileContext.Notificacoes
                .Select(ntf => new Notificacao(ntf.Id,ntf.Remetente,ntf.Destinatario,ntf.TipoDeNotificacao,ntf.MensagemDaNotificacao))
                .Where(ntf => ntf.Destinatario == idUsuario || ntf.Remetente == idUsuario)
                .ToList();

            foreach (var notificacoes in notificacoesDoUsr)
            {
                VolatileContext.Notificacoes.Remove(notificacoes);
            }
        }
    }
}
