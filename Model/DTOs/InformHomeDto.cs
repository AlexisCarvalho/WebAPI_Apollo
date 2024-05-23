namespace WebAPI_Apollo.Model.DTOs
{
    public record InformHomeDto(Guid idUsuario, int numNotificacoesNaoLidas, int numSolicitacoesAmizade, int numAmigos, int numMensagensNaoLidas);
}
