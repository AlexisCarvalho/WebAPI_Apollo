using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model.Interacoes
{
    [Table("Curtidas")]
    public class Curtida : Interacao
    {
        public Curtida(Guid remetente, Guid destinatario) :
            base(remetente, destinatario)
        { }
    }
}
