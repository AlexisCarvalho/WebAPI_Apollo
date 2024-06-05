namespace WebAPI_Apollo.Domain.DTOs
{
    public record AmizadeDto(Guid remetente, Guid destinatario);

    // Para uso especifico, fora das consultas
    public record AmizadeListaAmigos(Guid idAmigo, string nome, string imagemPerfil);
}
