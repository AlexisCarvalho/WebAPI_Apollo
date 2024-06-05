namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    public class Amizade : Interacao
    {
        public Amizade(Guid remetente, Guid destinatario) :
            base(remetente, destinatario)
        { }
        public Amizade(int id, Guid remetente, Guid destinatario) :
            base(id, remetente, destinatario)
        { }
    }
}
