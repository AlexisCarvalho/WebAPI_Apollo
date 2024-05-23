using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IMensagemRepository
    {
        void Add(Mensagem mensagem);
        List<ChatDto> GetAll();
        Mensagem? Get(int id);
        List<ChatDto> EnviadasPor(Guid id);
        List<ChatDto> RecebidasPor(Guid id);
        List<ChatDto> EnviadasEntre(Guid remetente, Guid destinatario);
        void Update(Mensagem mensagem);
        void Delete(Mensagem mensagem);
    }
}
