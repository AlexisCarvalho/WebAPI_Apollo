namespace WebAPI_Apollo.Domain.DTOs
{
    public record PostPadraoDto(Guid id, Guid idUsuario, string titulo, string descricao, int numCurtidas, int numComentarios, DateTime timeStamp);
    //public record PostComImagemDto(Guid id, Guid idUsuario, string titulo, string imagemBase64, int numCurtidas, int numComentarios);
    public record PostCompletoDto(Guid id, Guid idUsuario, string titulo, string descricao, string imagemBase64, int numCurtidas, int numComentarios, DateTime timeStamp);
}
