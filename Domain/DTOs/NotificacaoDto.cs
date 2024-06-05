namespace WebAPI_Apollo.Domain.DTOs
{
    public record NotificacoesDaRedeDto(Guid remetente, Guid destinatario, string imagemPerfil, string nomeQuemMandou, int tipoDeNotificacao, DateTime timeStamp, string mensagemDaNotificacao);
    public record NotificacoesDoUsuarioDto(Guid remetente, int tipoDeNotificacao, string mensagemDaNotificacao);
}

