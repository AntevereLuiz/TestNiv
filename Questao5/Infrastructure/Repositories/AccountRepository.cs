using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public AccountRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public Account? GetByAccount(string numero)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT * FROM contacorrente WHERE numero = @numero";

            return connection.Query<Account>(query, new { numero = numero }).FirstOrDefault();
        }

        public Account? GetById(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT * FROM contacorrente WHERE idcontacorrente = @Id";

            return connection.Query<Account>(query, new { Id = id }).FirstOrDefault();
        }
    }
}
