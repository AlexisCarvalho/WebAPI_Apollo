namespace WebAPI_Apollo.Model.DTOs
{
    public record NotificacoesDaRedeDto(Guid remetente, Guid destinatario, int tipoDeNotificacao, string mensagemDaNotificacao);
    public record NotificacoesDoUsuarioDto(Guid remetente, int tipoDeNotificacao, string mensagemDaNotificacao);
}

