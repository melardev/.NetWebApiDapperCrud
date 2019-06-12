using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Configuration;
using Bogus;
using Dapper;
using WebApiDapperCrud.Entities;

namespace WebApiDapperCrud.Seeds
{
    public class DbSeeder
    {
        private static string _connectionString;

        public static void Seed()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;
            SeedTodos();
            // SeedEntity2();
            // SeedEntity3();
            // ....
        }


        public static async Task SeedTodos()
        { var todosCount = await GetTodoCount();
            var todosToSeed = 32;
            todosToSeed -= todosCount;
            if (todosToSeed > 0)
            {
                Console.WriteLine($"[+] Seeding {todosToSeed} Todos");
                var faker = new Faker<Todo>()
                    .RuleFor(a => a.Title, f => string.Join(" ", f.Lorem.Words(f.Random.Int(2, 5))))
                    .RuleFor(a => a.Description, f => f.Lorem.Sentences(f.Random.Int(1, 10)))
                    .RuleFor(t => t.Completed, f => f.Random.Bool(0.4f))
                    .RuleFor(a => a.CreatedAt,
                        f => f.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now.AddDays(-1)))
                    .FinishWith((f, todoInstance) =>
                    {
                        todoInstance.UpdatedAt =
                            f.Date.Between(todoInstance.CreatedAt, DateTime.Now);
                    });

                var todos = faker.Generate(todosToSeed);
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var sql = "Insert Into Todo (Title, Description, Completed, CreatedAt, UpdatedAt) Values " +
                              "(@Title, @Description, @Completed, @CreatedAt, @UpdatedAt)";

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var todo in todos)
                            await connection.ExecuteAsync(sql, new
                            {
                                todo.Title,
                                todo.Description,
                                todo.Completed,
                                todo.CreatedAt,
                                todo.UpdatedAt
                            }, transaction);

                        transaction.Commit();
                    }

                    connection.Close();
                }
            }
        }

        private static async Task<int> GetTodoCount()
        {
           
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var rowcount = (int) await connection.ExecuteScalarAsync("Select COUNT(*) from Todo");
                return rowcount;
            }
        }
    }
}