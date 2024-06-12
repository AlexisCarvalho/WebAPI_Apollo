using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class MensagemRepositoryRAM : IMensagemRepository
    {
        public async Task Add(Mensagem mensagem)
        {
            await Task.Run(() =>
            {
                // Código pra substituir o autoIncrement do Banco
                var ultimaMensagem = GetLast().Result;

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
            });
        }

        public Task<Mensagem?> Get(int id)
        {
            var resultado = VolatileContext.Mensagens
                .FirstOrDefault(mensagem => mensagem.Id == id);

            return Task.FromResult(resultado);
        }

        public async Task Update(Mensagem mensagem)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Mensagens
                .FindIndex(m => m.Id == mensagem.Id);
                if (index != -1)
                {
                    VolatileContext.Mensagens[index] = mensagem;
                }
            });
        }

        public Task<List<ChatDto>> GetAll()
        {
            var resultado = VolatileContext.Mensagens
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id,
                    mensagem.Remetente,
                    mensagem.Destinatario,
                    mensagem.Conteudo,
                    mensagem.TimeStamp
                ))
                .ToList();

            return Task.FromResult(resultado);
        }

        public async Task Delete(Mensagem mensagem)
        {
            await Task.Run(() =>
            {
                VolatileContext.Mensagens.Remove(mensagem);
            });
        }

        public Task<List<ChatDto>> EnviadasPor(Guid id)
        {
            var resultado = VolatileContext.Mensagens
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

            return Task.FromResult(resultado);
        }

        public Task<List<ChatDto>> RecebidasPor(Guid id)
        {
            var resultado = VolatileContext.Mensagens
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

            return Task.FromResult(resultado);
        }

        public Task<List<ChatDto>> EnviadasEntre(Guid remetente, Guid destinatario)
        {
            var resultado = VolatileContext.Mensagens
                .Select(mensagem => new ChatDto
                (
                    mensagem.Id,
                    mensagem.Remetente,
                    mensagem.Destinatario,
                    mensagem.Conteudo,
                    mensagem.TimeStamp
                ))
                .Where(mensagem => mensagem.Remetente == remetente
                                   && mensagem.Destinatario == destinatario
                                   || mensagem.Remetente == destinatario
                                   && mensagem.Destinatario == remetente)
                .ToList();

            return Task.FromResult(resultado);
        }

        public Task<Mensagem?> GetLast()
        {
            var resultado = VolatileContext.Mensagens
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }
    }
}
