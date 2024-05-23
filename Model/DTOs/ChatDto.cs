namespace WebAPI_Apollo.Model.DTOs
{
    public record ChatDto(int Id, Guid Remetente, Guid Destinatario, string Conteudo, DateTime timeStamp);
}
