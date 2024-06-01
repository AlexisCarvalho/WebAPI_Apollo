using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class MensagemRepository : IMensagemRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(Mensagem mensagem)
        {
            _context.Mensagens.Add(mensagem);
            _context.SaveChanges();
        }

        public Mensagem? Get(int id)
        {
            return _context.Mensagens.FirstOrDefault(m => m.Id == id);
        }

        public void Update(Mensagem mensagem)
        {
            _context.Mensagens.Update(mensagem);
            _context.SaveChanges();
        }

        public List<ChatDto> GetAll()
        {
            return _context.Mensagens
                .Select(m => new ChatDto
                (
                    m.Id,
                    m.Remetente, 
                    m.Destinatario, 
                    m.Conteudo, 
                    m.TimeStamp
                ))
                .ToList();
        }

        public void Delete(Mensagem mensagem)
        {
            _context.Mensagens.Remove(mensagem);
            _context.SaveChanges();
        }

        public List<ChatDto> EnviadasPor(Guid id)
        {
            return _context.Mensagens
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
                .ToList();
        }

        public List<ChatDto> RecebidasPor(Guid id)
        {
            return _context.Mensagens
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
                .ToList();
        }

        public List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return _context.Mensagens
                .Where(m => m.Remetente == remetente 
                            && m.Destinatario == destinatario)
                .OrderByDescending(m => m.Id)
                .Select(m => new ChatDto
                (
                    m.Id, 
                    m.Remetente, 
                    m.Destinatario, 
                    m.Conteudo, 
                    m.TimeStamp
                ))
                .ToList();
        }

        public Mensagem? GetLast()
        {
            return _context.Mensagens
                .OrderByDescending(m => m.Id)
                .FirstOrDefault();
        }
    }
}
