using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class MensagemRepository : IMensagemRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void Add(Mensagem mensagem)
        {
            _context.Mensagens.Add(mensagem);
            _context.SaveChanges();
        }

        public Mensagem? Get(int id)
        {
            return _context.Mensagens.Find(id);
        }

        public void Update(Mensagem mensagem)
        {
            _context.Mensagens.Update(mensagem);
            _context.SaveChanges();
        }

        public List<ChatDto> GetAll()
        {
            return _context.Mensagens
                .Select(mensagem => new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp))
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
                           .OrderByDescending(msg => msg.Id)
                           .Select(mensagem => new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp))
                           .Where(mensagem => mensagem.Remetente == id)
                           .ToList();
        }

        public List<ChatDto> RecebidasPor(Guid id)
        {
            return _context.Mensagens
                           .OrderByDescending(msg => msg.Id)
                           .Select(mensagem => new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp))
                           .Where(mensagem => mensagem.Destinatario == id)
                           .ToList();
        }

        public List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return _context.Mensagens
                    .OrderByDescending(msg => msg.Id)
                    .Select(mensagem => new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp))
                    .Where(mensagem => mensagem.Remetente == remetente && mensagem.Destinatario == destinatario)
                    .ToList();
        }

        public Mensagem? GetLast()
        {
            return _context.Mensagens.OrderByDescending(e => e.Id).FirstOrDefault();
        }
    }
}
