namespace WebAPI_Apollo.Model.DTOs
{
    public record UsuarioDto(Guid id, int idade, int xp, int level, int XP_ProximoNivel, string nome, string email, string senha, string esporte, string genero, string userName, string palavraRecuperacao, DateTime dataNascimento, float peso, float altura);
    public record PerfilUsuarioDto(string nome, float altura, float peso, double imc, double aguaDiaria, int level, int xp);
    public record HomeUsuarioDto(string nomeRedeSocial, int numNotificacoes, int numAmigos);
}
