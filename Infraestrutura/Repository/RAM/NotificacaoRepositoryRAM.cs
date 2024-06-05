using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
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
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new Notificacao
                (
                    noti.Id,
                    noti.Remetente,
                    noti.Destinatario,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                ))
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = VolatileContext.Usuarios
                    .FirstOrDefault(usr => usr.Id == noti.Remetente);

                var notiSaida = new NotificacoesDaRedeDto
                    (
                        noti.Remetente,
                        noti.Destinatario,
                        usuarioQueEnviou.ImagemPerfil,
                        usuarioQueEnviou.Nome,
                        noti.TipoDeNotificacao,
                        noti.TimeStamp,
                        noti.MensagemDaNotificacao
                    );
                notificacoesSaida.Add(notiSaida);
            }

            return notificacoesSaida;
        }

        public List<NotificacoesDaRedeDto> GetAllUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new Notificacao
                (
                    noti.Id,
                    noti.Remetente,
                    noti.Destinatario,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                ))
                .Where(noti => noti.Destinatario == idUsuario && noti.TipoDeNotificacao != 1)
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = VolatileContext.Usuarios
                    .FirstOrDefault(usr => usr.Id == noti.Remetente);

                var notiSaida = new NotificacoesDaRedeDto
                    (
                        noti.Remetente,
                        noti.Destinatario,
                        usuarioQueEnviou.ImagemPerfil,
                        usuarioQueEnviou.Nome,
                        noti.TipoDeNotificacao,
                        noti.TimeStamp,
                        noti.MensagemDaNotificacao
                    );
                notificacoesSaida.Add(notiSaida);
            }

            return notificacoesSaida;
        }

        public List<NotificacoesDaRedeDto> GetAllNotiAmizadeUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new Notificacao
                (
                    noti.Id,
                    noti.Remetente,
                    noti.Destinatario,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                ))
                .Where(noti => noti.Destinatario == idUsuario && noti.TipoDeNotificacao == 1)
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = VolatileContext.Usuarios
                    .FirstOrDefault(usr => usr.Id == noti.Remetente);

                var notiSaida = new NotificacoesDaRedeDto
                    (
                        noti.Remetente,
                        noti.Destinatario,
                        usuarioQueEnviou.ImagemPerfil,
                        usuarioQueEnviou.Nome,
                        noti.TipoDeNotificacao,
                        noti.TimeStamp,
                        noti.MensagemDaNotificacao
                    );
                notificacoesSaida.Add(notiSaida);
            }

            return notificacoesSaida;
        }

        public List<NotificacoesDaRedeDto> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = VolatileContext.Notificacoes
                .OrderByDescending(noti => noti.Id)
                .Select(noti => new Notificacao
                (
                    noti.Id,
                    noti.Remetente,
                    noti.Destinatario,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                ))
                .Where(noti => noti.Remetente == idUsuario && noti.TipoDeNotificacao == 1)
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = VolatileContext.Usuarios
                    .FirstOrDefault(usr => usr.Id == noti.Remetente);

                var notiSaida = new NotificacoesDaRedeDto
                    (
                        noti.Remetente,
                        noti.Destinatario,
                        usuarioQueEnviou.ImagemPerfil,
                        usuarioQueEnviou.Nome,
                        noti.TipoDeNotificacao,
                        noti.TimeStamp,
                        noti.MensagemDaNotificacao
                    );
                notificacoesSaida.Add(notiSaida);
            }

            return notificacoesSaida;
        }

        public Notificacao? GetLast()
        {
            return VolatileContext.Notificacoes
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }

        public void Update(Notificacao notificacao)
        {
            var index = VolatileContext.Notificacoes
                .FindIndex(e => e.Id == notificacao.Id);
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
                .Select(noti => new Notificacao
                (
                    noti.Id,
                    noti.Remetente,
                    noti.Destinatario,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                ))
                .Where(ntf => ntf.Destinatario == idUsuario
                              || ntf.Remetente == idUsuario)
                .ToList();

            foreach (var notificacoes in notificacoesDoUsr)
            {
                VolatileContext.Notificacoes.Remove(notificacoes);
            }
        }
    }
}
