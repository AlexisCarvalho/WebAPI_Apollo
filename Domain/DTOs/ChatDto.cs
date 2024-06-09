namespace WebAPI_Apollo.Domain.DTOs
{
    public record ChatDtoRetornoPost(Guid Remetente, Guid Destinatario, string Conteudo, DateTime TimeStamp);
    public record ChatDto(int Id, Guid Remetente, Guid Destinatario, string Conteudo, DateTime TimeStamp);
    public record ComentariosDto(Guid Remetente, Guid Destinatario, string ImagemPerfil, string Nome, string Conteudo, DateTime TimeStamp);
}
