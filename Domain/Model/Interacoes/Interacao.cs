namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    public class Interacao
    {
        protected int _id;
        protected Guid _idRemetente;
        protected Guid _idDestinatario;

        public Interacao(Guid remetente, Guid destinatario)
        {
            Remetente = remetente;
            Destinatario = destinatario;
        }

        public Interacao(int id, Guid remetente, Guid destinatario)
        {
            Id = id;
            Remetente = remetente;
            Destinatario = destinatario;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Guid Remetente
        {
            get { return _idRemetente; }
            set { _idRemetente = value; }
        }

        public Guid Destinatario
        {
            get { return _idDestinatario; }
            set { _idDestinatario = value; }
        }
    }
}
