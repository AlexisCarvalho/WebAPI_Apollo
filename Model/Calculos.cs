namespace WebAPI_Apollo.Model
{
    public class Calculos
    {
        const int _mlDeAguaPorKilo = 35;
        const int _mlDeAguaPorKiloAtividadeDiaria = 42;

        public Calculos()
        {
        }

        public double CalcularIMC(Usuario usuario)
        {
            return Math.Round(usuario.Peso / Math.Pow(usuario.Altura, 2), 2);
        }

        // Jovens até os 17 anos	40 ml por cada kg
        // De 18 a 55 anos	35 ml por cada kg
        // De 55 a 65 anos	30 ml por cada kg
        // 66 anos em diante	25 ml por cada kg
        // Utilizei uma formula simplificada que gera um resultado semelhante
        public double CalcularAguaDiaria(bool AtiviFisicaDiaria, Usuario usuario)
        {
            if (usuario.Idade <= 55)
            {
                if (AtiviFisicaDiaria)
                {
                    return Math.Round(usuario.Peso * _mlDeAguaPorKiloAtividadeDiaria);
                }
                else
                {
                    return Math.Round(usuario.Peso * _mlDeAguaPorKilo);
                }
            }
            else
            {
                if (AtiviFisicaDiaria)
                {
                    return Math.Round(usuario.Peso * (_mlDeAguaPorKiloAtividadeDiaria - 5));
                }
                else
                {
                    return Math.Round(usuario.Peso * (_mlDeAguaPorKilo - 5));
                }
            }

        }

        // Calcula a idade utilizando a data atual e a data de nascimento
        // Sera executado sempre que o perfil for acessado para garantir
        // A precisão
        public int CalcularIdade(DateTime dataNascimento)
        {
            DateTime dataAtual = DateTime.Now;

            int idade = dataAtual.Year - dataNascimento.Year;

            if (dataAtual.Month < dataNascimento.Month)
            {
                idade--;
            }
            else if (dataAtual.Month == dataNascimento.Month)
            {
                if (dataNascimento.Day > dataAtual.Day)
                {
                    idade--;
                }
            }

            if (idade < 0)
            {
                return 0;
            }

            return idade;
        }

        public void GanharXP(int quantidadeExp, ref Usuario usuario)
        {
            usuario.XP += quantidadeExp;

            while (usuario.XP >= usuario.XP_ProximoNivel)
            {
                LevelUp(usuario);
            }
        }

        private void LevelUp(Usuario usuario)
        {
            usuario.Level++;
            usuario.XP_ProximoNivel = CalcularXP_ProximoNivel(usuario.Level);
        }

        private int CalcularXP_ProximoNivel(int level)
        {
            return (int)(50 * Math.Pow(1.5, level));
        }
    }
}
