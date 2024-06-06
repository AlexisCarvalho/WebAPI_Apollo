using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    [Table("Mensagens")]
    public class Mensagem : Interacao
    {
        private string _conteudo;
        private DateTime _timeStamp;

        public Mensagem(Guid remetente, Guid destinatario, string conteudo) :
            base(remetente, destinatario)
        {
            _conteudo = conteudo;
            _timeStamp = DateTime.Now;
        }

        public string Conteudo
        {
            get { return _conteudo; }
            set { _conteudo = value; }
        }
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}