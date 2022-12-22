using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public TransactionRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<int> MakeTransactionAsync(Transaction transaction)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            return await connection.ExecuteAsync("INSERT INTO movimento " +
                                                 "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, " +
                                                 "@TipoMovimento, @Valor);", transaction);
        }

        public async Task<GetBalanceQueryResponse?> GetByBalanceAccountAsync(string numero)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT " +
                            "c.numero AS AccountNumber, " +
                            "c.nome AS OwnerAccount, " +
                            "datetime() AS SearchDate, " +
                            "sum(m.valor) AS Balance " +
                            "FROM contacorrente AS c " +
                            "LEFT JOIN movimento AS m " +
                            "ON c.idcontacorrente = m.idcontacorrente " +
                            "WHERE c.numero = @numero; ";



            return await connection.QueryFirstOrDefaultAsync<GetBalanceQueryResponse>(query, new { numero = numero });
        }
    }
}
