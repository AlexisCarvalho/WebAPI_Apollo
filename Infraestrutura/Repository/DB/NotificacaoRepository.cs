using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Notificacao notificacao)
        {
            await _context.Notificacoes.AddAsync(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task<Notificacao?> Get(int id)
        {
            return await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Notificacao?> GetLast()
        {
            return await _context.Notificacoes
                .OrderByDescending(n => n.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<NotificacoesDaRedeDto>> GetAll()
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = await _context.Notificacoes
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
                .ToListAsync();

            var tasks = notificacoes.Select(async noti =>
            {
                var usuarioQueEnviou = await _context.Usuarios
                    .FirstOrDefaultAsync(usr => usr.Id == noti.Remetente);

                return new NotificacoesDaRedeDto
                (
                    noti.Remetente,
                    noti.Destinatario,
                    usuarioQueEnviou.ImagemPerfil,
                    usuarioQueEnviou.Nome,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                );
            }).ToArray();

            notificacoesSaida = (await Task.WhenAll(tasks)).ToList();

            return notificacoesSaida;
        }

        public async Task<List<NotificacoesDaRedeDto>> GetAllUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();
            var notificacoes = await _context.Notificacoes
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
                .ToListAsync();

            var tasks = notificacoes.Select(async noti =>
            {
                var usuarioQueEnviou = await _context.Usuarios
                    .FirstOrDefaultAsync(usr => usr.Id == noti.Remetente);

                return new NotificacoesDaRedeDto
                (
                    noti.Remetente,
                    noti.Destinatario,
                    usuarioQueEnviou.ImagemPerfil,
                    usuarioQueEnviou.Nome,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                );
            }).ToArray();

            notificacoesSaida = (await Task.WhenAll(tasks)).ToList();

            return notificacoesSaida;
        }

        public async Task<List<NotificacoesDaRedeDto>> GetAllNotiAmizadeUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();

            var notificacoes = await _context.Notificacoes
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
                .ToListAsync();

            var tasks = notificacoes.Select(async noti =>
            {
                var usuarioQueEnviou = await _context.Usuarios
                    .FirstOrDefaultAsync(usr => usr.Id == noti.Remetente);

                return new NotificacoesDaRedeDto
                (
                    noti.Remetente,
                    noti.Destinatario,
                    usuarioQueEnviou.ImagemPerfil,
                    usuarioQueEnviou.Nome,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                );
            }).ToArray();

            notificacoesSaida = (await Task.WhenAll(tasks)).ToList();

            return notificacoesSaida;
        }


        public async Task<List<NotificacoesDaRedeDto>> GetAllEnviadasNotiAmizadeUsr(Guid idUsuario)
        {
            var notificacoesSaida = new List<NotificacoesDaRedeDto>();

            var notificacoes = await _context.Notificacoes
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
                .ToListAsync();

            var tasks = notificacoes.Select(async noti =>
            {
                var usuarioQueEnviou = await _context.Usuarios
                    .FirstOrDefaultAsync(usr => usr.Id == noti.Remetente);

                return new NotificacoesDaRedeDto
                (
                    noti.Remetente,
                    noti.Destinatario,
                    usuarioQueEnviou.ImagemPerfil,
                    usuarioQueEnviou.Nome,
                    noti.TipoDeNotificacao,
                    noti.TimeStamp,
                    noti.MensagemDaNotificacao
                );
            }).ToArray();

            notificacoesSaida = (await Task.WhenAll(tasks)).ToList();

            return notificacoesSaida;
        }

        public async Task Update(Notificacao notificacao)
        {
            _context.Notificacoes.Update(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Notificacao notificacao)
        {
            _context.Notificacoes.Remove(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarReferencias(Guid idUsuario)
        {
            var notificacoesDoUsr = await _context.Notificacoes
                .Where(n => n.Destinatario == idUsuario
                            || n.Remetente == idUsuario)
                .ToListAsync();

            _context.Notificacoes.RemoveRange(notificacoesDoUsr);
            await _context.SaveChangesAsync();
        }

        public async Task<Notificacao?> JaFoiNotificado(Notificacao notificacao)
        {
            return await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Remetente == notificacao.Remetente
                                    && n.Destinatario == notificacao.Destinatario
                                    && n.TipoDeNotificacao == notificacao.TipoDeNotificacao);
        }
    }
}
