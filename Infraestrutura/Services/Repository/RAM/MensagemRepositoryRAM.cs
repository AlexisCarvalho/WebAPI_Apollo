using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class MensagemRepositoryRAM : IMensagemRepository
    {
        public void Add(Mensagem mensagem)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaMensagem = GetLast();

            if (ultimaMensagem != null)
            {
                mensagem.Id = ultimaMensagem.Id + 1;
            }
            else
            {
                mensagem.Id = 1;
            }
            // Visa manter o uso de id int ao invés de trocar pra Guid

            VolatileContext.Mensagens.Add(mensagem);
        }

        public Mensagem? Get(int id)
        {
            return VolatileContext.Mensagens
                .FirstOrDefault(mensagem => mensagem.Id == id);
        }

        public void Update(Mensagem mensagem)
        {
            var index = VolatileContext.Mensagens
                .FindIndex(m => m.Id == mensagem.Id);
            if (index != -1)
            {
                VolatileContext.Mensagens[index] = mensagem;
            }
        }

        public List<ChatDto> GetAll()
        {
            return VolatileContext.Mensagens
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id, 
                    mensagem.Remetente, 
                    mensagem.Destinatario, 
                    mensagem.Conteudo, 
                    mensagem.TimeStamp
                ))
                .ToList();
        }

        public void Delete(Mensagem mensagem)
        {
            VolatileContext.Mensagens.Remove(mensagem);
        }

        public List<ChatDto> EnviadasPor(Guid id)
        {
            return VolatileContext.Mensagens
                .OrderByDescending(msg => msg.Id)
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id, 
                    mensagem.Remetente, 
                    mensagem.Destinatario, 
                    mensagem.Conteudo, 
                    mensagem.TimeStamp
                ))
                .Where(mensagem => mensagem.Remetente == id)
                .ToList();
        }

        public List<ChatDto> RecebidasPor(Guid id)
        {
            return VolatileContext.Mensagens
                .OrderByDescending(msg => msg.Id)
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id, 
                    mensagem.Remetente, 
                    mensagem.Destinatario, 
                    mensagem.Conteudo, 
                    mensagem.TimeStamp
                ))
                .Where(mensagem => mensagem.Destinatario == id)
                .ToList();
        }

        public List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            return VolatileContext.Mensagens
                .OrderByDescending(msg => msg.Id)
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id, 
                    mensagem.Remetente, 
                    mensagem.Destinatario, 
                    mensagem.Conteudo, 
                    mensagem.TimeStamp
                ))
                .Where(mensagem => mensagem.Remetente == remetente 
                                   && mensagem.Destinatario == destinatario)
                .ToList();
        }

        public Mensagem? GetLast()
        {
            return VolatileContext.Mensagens
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }
    }
}
