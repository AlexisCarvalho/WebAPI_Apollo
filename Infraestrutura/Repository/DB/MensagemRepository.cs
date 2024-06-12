using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class MensagemRepository : IMensagemRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Mensagem mensagem)
        {
            await _context.Mensagens.AddAsync(mensagem);
            await _context.SaveChangesAsync();
        }

        public async Task<Mensagem?> Get(int id)
        {
            return await _context.Mensagens.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<ChatDto>> GetAll()
        {
            return await _context.Mensagens
                .Select(m => new ChatDto
                (
                    m.Id,
                    m.Remetente,
                    m.Destinatario,
                    m.Conteudo,
                    m.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task<Mensagem?> GetLast()
        {
            return await _context.Mensagens
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatDto>> EnviadasPor(Guid id)
        {
            return await _context.Mensagens
                .Where(m => m.Remetente == id)
                .OrderByDescending(m => m.Id)
                .Select(m => new ChatDto
                (
                    m.Id,
                    m.Remetente,
                    m.Destinatario,
                    m.Conteudo,
                    m.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task<List<ChatDto>> RecebidasPor(Guid id)
        {
            return await _context.Mensagens
                .Where(m => m.Destinatario == id)
                .OrderByDescending(m => m.Id)
                .Select(m => new ChatDto
                (
                    m.Id,
                    m.Remetente,
                    m.Destinatario,
                    m.Conteudo,
                    m.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task<List<ChatDto>> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return await _context.Mensagens
                .Where(m => m.Remetente == remetente
                                && m.Destinatario == destinatario
                                || m.Remetente == destinatario
                                && m.Destinatario == remetente)
                .Select(m => new ChatDto
                (
                    m.Id,
                    m.Remetente,
                    m.Destinatario,
                    m.Conteudo,
                    m.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task Update(Mensagem mensagem)
        {
            _context.Mensagens.Update(mensagem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Mensagem mensagem)
        {
            _context.Mensagens.Remove(mensagem);
            await _context.SaveChangesAsync();
        }
    }
}
