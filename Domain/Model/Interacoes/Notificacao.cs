using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    [Table("Notificacoes")]
    public class Notificacao : Interacao
    {
        private int _tipoDeNotificacao;
        private string _mensagemDaNotificacao;
        private DateTime _timeStamp;

        // TIPOS:
        // 1 - Solicitação de Amizade
        // 2 - Postagem de Amigo (Página)
        // 3 - Resposta de Solicitação

        public Notificacao(Guid remetente, Guid destinatario, int tipoDeNotificacao, string mensagemDaNotificacao) :
            base(remetente, destinatario)
        {
            _timeStamp = DateTime.Now;
            _mensagemDaNotificacao = mensagemDaNotificacao;
            _tipoDeNotificacao = tipoDeNotificacao;
        }

        public Notificacao(int id, Guid remetente, Guid destinatario, int tipoDeNotificacao, DateTime timeStamp, string mensagemDaNotificacao) :
            base(id, remetente, destinatario)
        {
            _timeStamp = timeStamp;
            _mensagemDaNotificacao = mensagemDaNotificacao;
            _tipoDeNotificacao = tipoDeNotificacao;
        }

        public int TipoDeNotificacao
        {
            get { return _tipoDeNotificacao; }
            set { _tipoDeNotificacao = value; }
        }

        public string MensagemDaNotificacao
        {
            get { return _mensagemDaNotificacao; }
            set { _mensagemDaNotificacao = value; }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}
