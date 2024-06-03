using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(Notificacao notificacao)
        {
            var ultimaNotificacao = GetLast();

            if (ultimaNotificacao != null)
            {
                notificacao.Id = ultimaNotificacao.Id + 1;
            }
            else
            {
                notificacao.Id = 1;
            }

            _context.Notificacoes.Add(notificacao);
            _context.SaveChanges();
        }

        public Notificacao? JaFoiNotificado(Notificacao notificacao)
        {
            return _context.Notificacoes
                .FirstOrDefault(n => n.Remetente == notificacao.Remetente
                                    && n.Destinatario == notificacao.Destinatario
                                    && n.TipoDeNotificacao == notificacao.TipoDeNotificacao);
        }

        public Notificacao? Get(int id)
        {
            return _context.Notificacoes.FirstOrDefault(n => n.Id == id);
        }

        public List<NotificacoesDaRedeDto> GetAll()
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = _context.Notificacoes
                .OrderByDescending(n => n.Id)
                .Select(n => new Notificacao
                (
                    n.Id,
                    n.Remetente,
                    n.Destinatario,
                    n.TipoDeNotificacao,
                    n.TimeStamp,
                    n.MensagemDaNotificacao
                ))
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = _context.Usuarios
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
            var notificacoes = _context.Notificacoes
                .Where(n => n.Destinatario == idUsuario && n.TipoDeNotificacao != 1)
                .OrderByDescending(n => n.Id)
                .Select(n => new Notificacao
                (
                    n.Id,
                    n.Remetente,
                    n.Destinatario,
                    n.TipoDeNotificacao,
                    n.TimeStamp,
                    n.MensagemDaNotificacao
                ))
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = _context.Usuarios
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
            var notificacoes = _context.Notificacoes
                .Where(n => n.Destinatario == idUsuario && n.TipoDeNotificacao == 1)
                .OrderByDescending(n => n.Id)
                .Select(n => new Notificacao
                (
                    n.Id,
                    n.Remetente,
                    n.Destinatario,
                    n.TipoDeNotificacao,
                    n.TimeStamp,
                    n.MensagemDaNotificacao
                ))
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = _context.Usuarios
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
            var notificacoes = _context.Notificacoes
                .Where(n => n.Remetente == idUsuario && n.TipoDeNotificacao == 1)
                .OrderByDescending(n => n.Id)
                .Select(n => new Notificacao
                (
                    n.Id,
                    n.Remetente,
                    n.Destinatario,
                    n.TipoDeNotificacao,
                    n.TimeStamp,
                    n.MensagemDaNotificacao
                ))
                .ToList();

            foreach (var noti in notificacoes)
            {
                var usuarioQueEnviou = _context.Usuarios
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
            return _context.Notificacoes
                .OrderByDescending(n => n.Id)
                .FirstOrDefault();
        }

        public void Update(Notificacao notificacao)
        {
            _context.Notificacoes.Update(notificacao);
            _context.SaveChanges();
        }

        public void Delete(Notificacao notificacao)
        {
            _context.Notificacoes.Remove(notificacao);
            _context.SaveChanges();
        }

        public void DeletarReferencias(Guid idUsuario)
        {
            var notificacoesDoUsr = _context.Notificacoes
                .Where(n => n.Destinatario == idUsuario
                            || n.Remetente == idUsuario)
                .ToList();

            _context.Notificacoes.RemoveRange(notificacoesDoUsr);
            _context.SaveChanges();
        }
    }
}
