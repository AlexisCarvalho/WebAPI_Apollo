using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IMensagemRepository
    {
        Task Add(Mensagem mensagem);
        Task<Mensagem?> Get(int id);
        Task<List<ChatDto>> GetAll();
        Task<Mensagem?> GetLast();
        Task<List<ChatDto>> EnviadasPor(Guid id);
        Task<List<ChatDto>> RecebidasPor(Guid id);
        Task<List<ChatDto>> EnviadasEntre(Guid remetente, Guid destinatario);
        Task Update(Mensagem mensagem);
        Task Delete(Mensagem mensagem);
    }
}