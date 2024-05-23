namespace WebAPI_Apollo.Model.Interacoes
{
    public class Curtida : Interacao
    {
        public Curtida(Guid remetente, Guid destinatario) :
            base(remetente, destinatario)
        { }
    }
}
