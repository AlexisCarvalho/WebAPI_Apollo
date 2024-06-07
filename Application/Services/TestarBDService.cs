using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Infraestrutura;

namespace WebAPI_Apollo.Application.Services
{
    public class TestarBDService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConfigService _configService;

        public TestarBDService(IServiceProvider serviceProvider, ConfigService configService)
        {
            _serviceProvider = serviceProvider;
            _configService = configService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RunDatabaseTestsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task RunDatabaseTestsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            bool read = true;
            bool write = true;
            bool delete = true;

            try
            {
                using AppDbContext TesteBanco = new();
                // A intenção aqui é gerar um erro caso não exista tabela correspondente,
                // ao mesmo tempo que verifica se existe um valor inicial já na tabela,
                // não existindo ele insere
                TestTable? tabelaExiste = TesteBanco.TestTable.FirstOrDefault();

                if (tabelaExiste is null)
                {
                    TestTable entradainicial = new();
                    TesteBanco.TestTable.Add(entradainicial);
                    TesteBanco.SaveChanges();
                }

                // Adiciona uma nova entrada ao banco para testar o WRITE
                TestTable entradaNova = new();
                TesteBanco.TestTable.Add(entradaNova);
                TesteBanco.SaveChanges();

                // Tenta recuperar a entrada recém adicionada para testar o READ
                TestTable? ultimaEntrada = TesteBanco.TestTable.OrderByDescending(t => t.CriadaEm).FirstOrDefault();

                if (ultimaEntrada is null)
                {
                    read = false;
                }
                else
                {
                    // Verifica se a ultima entrada encontrada no banco
                    // se trata da recém adicionada
                    if (ultimaEntrada.CriadaEm != entradaNova.CriadaEm)
                    {
                        write = false;
                    }

                    TestTable? entradaMaisAntiga = TesteBanco.TestTable.FirstOrDefault();

                    if (entradaMaisAntiga != null)
                    {
                        // Tenta remover a entrada mais antiga da tabela
                        TesteBanco.TestTable.Remove(entradaMaisAntiga);
                        TesteBanco.SaveChanges();

                        TestTable? novoValorLido = TesteBanco.TestTable.FirstOrDefault();

                        if (novoValorLido is null)
                        {
                            read = false;
                        }
                        else
                        {
                            // Compara os valores pra ver se a nova entrada mais antiga
                            // agora é a recém adicionada confirmando o DELETE
                            if (novoValorLido.CriadaEm != entradaNova.CriadaEm)
                            {
                                delete = false;
                            }
                        }
                    }
                }

                if (read && write && delete)
                {
                    ConfigService.DBFuncionando = false; // Trocar aqui pra forçar uso da RAM no inicio
                }

                // Se todos os testes passarem ativa o banco
                _configService.DBAtivado = ConfigService.DBFuncionando;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro nos Testes do Banco de Dados: {ex.Message}");
                _configService.DBAtivado = false;
            }
        }
    }
}
