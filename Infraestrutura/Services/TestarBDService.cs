using WebAPI_Apollo.Model;

namespace WebAPI_Apollo.Infraestrutura.Services
{
    public class TestarBDService
    {
        public static void ExecutarTeste()
        {
            try
            {
                using AppDbContext TesteBanco = new();
                // A intenção aqui é gerar um erro causo não exista tabela correspondente,
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
                    ConfSistema.DBAtivado = false;
                }
                else
                {
                    // Verifica se a ultima entrada encontrada no banco
                    // se trata da recém adicionada
                    if (ultimaEntrada.CriadaEm != entradaNova.CriadaEm)
                    {
                        ConfSistema.DBAtivado = false;
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
                            ConfSistema.DBAtivado = false;
                        }
                        else
                        {
                            // Compara os valores pra ver se a nova entrada mais antiga
                            // agora é a recém adicionada confirmando o DELETE
                            if (novoValorLido.CriadaEm != entradaNova.CriadaEm)
                            {
                                ConfSistema.DBAtivado = false;
                            }
                        }
                    }
                }
            }
            catch (Microsoft.Data.Sqlite.SqliteException)
            {
                ConfSistema.DBAtivado = false;
            }
        }
    }
}
