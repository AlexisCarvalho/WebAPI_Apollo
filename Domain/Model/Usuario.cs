using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Apollo.Domain.Model
{
    [Table("Usuarios")]
    public class Usuario
    {
        private Guid _id;
        private int _idade;
        private int _xp;
        private int _level;
        private int _XP_ProximoNivel;
        private string _nome;
        private string _email;
        private string _senha;
        private string _esporte;
        private string _genero;
        private string _userName;
        private string _palavraRecuperacao;
        private DateTime _dataNascimento;
        private float _peso;
        private float _altura;
        private int _idEstatisticas;
        private string _imagemPerfil;

        public Usuario()
        {
            _id = Guid.NewGuid();
            _xp = 0;
            _level = 1;
            _XP_ProximoNivel = 56;
            _idade = 0;
            _nome = string.Empty;
            _email = string.Empty;
            _senha = string.Empty;
            _esporte = string.Empty;
            _genero = string.Empty;
            _userName = string.Empty;
            _palavraRecuperacao = string.Empty;
            _dataNascimento = new DateTime();
            _peso = 0;
            _altura = 0;
            _idEstatisticas = 0;
        }

        public Usuario(string nome, string email, string senha, string userName, string palavraRecuperacao, DateTime dataNascimento)
        {
            Calculos calc = new Calculos();
            _id = Guid.NewGuid();
            _xp = 0;
            _level = 1;
            _XP_ProximoNivel = 56;
            _idade = calc.CalcularIdade(dataNascimento);
            _nome = nome;
            _email = email;
            _senha = senha;
            _esporte = string.Empty;
            _genero = string.Empty;
            _userName = userName;
            _palavraRecuperacao = palavraRecuperacao;
            _dataNascimento = dataNascimento;
            _peso = 0;
            _altura = 0;
            _idEstatisticas = 0;
            _imagemPerfil = string.Empty;
        }

        [Key]
        public Guid Id
        {
            get { return _id; }
            init { }
        }

        public int Idade
        {
            get { return _idade; }
            set { _idade = value; }
        }

        public int XP
        {
            get { return _xp; }
            set { _xp = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public int XP_ProximoNivel
        {
            get { return _XP_ProximoNivel; }
            set { _XP_ProximoNivel = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }

        public string Esporte
        {
            get { return _esporte; }
            set { _esporte = value; }
        }

        public string Genero
        {
            get { return _genero; }
            set { _genero = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string PalavraRecuperacao
        {
            get { return _palavraRecuperacao; }
            set { _palavraRecuperacao = value; }
        }

        public DateTime DataNascimento
        {
            get { return _dataNascimento; }
            set { _dataNascimento = value; }
        }

        public float Peso
        {
            get { return _peso; }
            set { _peso = value; }
        }

        public float Altura
        {
            get { return _altura; }
            set { _altura = value; }
        }

        public int IdEstatisticas
        {
            get { return _idEstatisticas; }
            set { _idEstatisticas = value; }
        }

        public string ImagemPerfil
        {
            get { return _imagemPerfil; }
            set { _imagemPerfil = value; }
        }
    }
}
