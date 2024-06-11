using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IMensagemRepository
    {
        void Add(Mensagem mensagem);
        void Delete(Mensagem mensagem);
        List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario);
        List<ChatDto> EnviadasPor(Guid id);
        Mensagem? Get(int id);
        List<ChatDto> GetAll();
        Mensagem? GetLast();
        List<ChatDto> RecebidasPor(Guid id);
        void Update(Mensagem mensagem);
    }
}