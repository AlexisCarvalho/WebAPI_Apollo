namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    public class Curtida : Interacao
    {
        public Curtida(Guid remetente, Guid destinatario) :
            base(remetente, destinatario)
        { }
    }
}
