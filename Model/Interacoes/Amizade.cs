namespace WebAPI_Apollo.Model.Interacoes
{
    public class Amizade : Interacao
    {
        public Amizade(Guid remetente, Guid destinatario) :
            base(remetente, destinatario)
        { }
    }
}
