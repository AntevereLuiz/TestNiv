using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotencyRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }
        public async Task<int> CreateIdempotencyKeyAsync(Idempotency idempotency)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            return await connection.ExecuteAsync("INSERT INTO idempotencia " +
                                                 "VALUES (@Chave_Idempotencia, @Requisicao, @Resultado);",
                                                 idempotency);
        }

        public Idempotency? GetById(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT * FROM idempotencia WHERE Chave_Idempotencia = @Id";

            return connection.Query<Idempotency>(query, new { Id = id }).FirstOrDefault();
        }
    }
}
