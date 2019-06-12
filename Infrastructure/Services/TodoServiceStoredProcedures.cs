using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using Dapper;
using WebApiDapperCrud.Entities;
using WebApiDapperCrud.Enums;

namespace WebApiDapperCrud.Infrastructure.Services
{
    public class TodoServiceStoredProcedures : ITodoService
    {
        private readonly string _connectionString;


        public TodoServiceStoredProcedures()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;
        }

        public async Task<List<Todo>> FetchMany(TodoShow show = TodoShow.All)
        {
            List<Todo> todos;

            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                if (show == TodoShow.Completed)
                {
                    todos = (await con.QueryAsync<Todo>("GetCompleted")).ToList();
                }
                else if (show == TodoShow.Pending)
                {
                    todos = (await con.QueryAsync<Todo>("GetPending")).ToList();
                }
                else
                {
                    todos = (await con.QueryAsync<Todo>("GetAllTodos")).ToList();
                }
            }

            return todos;
        }

        public async Task<Todo> FetchById(int id)
        {
            Todo todo = new Todo();

            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Id", id);

                todo = (await con.QueryAsync<Todo>("GetTodoById", new {Id = id},
                        commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();
            }

            return todo;
        }

        public async Task<Todo> FetchProxyById(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Id", id);

                return (await con.QueryAsync<Todo>("GetTodoProxyById", parameter,
                        commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();
            }
        }

        public async Task<Todo> Create(Todo todo)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Title", todo.Title);
                parameters.Add("@Description", todo.Description);
                parameters.Add("@Completed", todo.Completed);

                object result = await con.ExecuteScalarAsync("CreateTodo", parameters,
                    commandType: CommandType.StoredProcedure);
                int todoId = int.Parse(result.ToString());
                todo.Id = todoId;
            }

            return todo;
        }

        public async Task<Todo> Update(Todo todoFromDb, Todo todoInput)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DateTime now = DateTime.UtcNow;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", todoFromDb.Id);
                parameters.Add("@Title", todoInput.Title);
                parameters.Add("@Description", todoInput.Description);
                parameters.Add("@Completed", todoInput.Completed);
                parameters.Add("@UpdatedAt", now);

                int rowAffected =
                    await con.ExecuteAsync("UpdateTodo", parameters, commandType: CommandType.StoredProcedure);

                todoInput.Id = todoFromDb.Id;
                todoInput.UpdatedAt = now;
            }

            return todoInput;
        }

        public async Task<int> DeleteById(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                rowAffected =
                    await con.ExecuteAsync("DeleteTodo", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }

        public async Task DeleteAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                await connection.ExecuteAsync("DeleteAllTodos", null, commandType: CommandType.StoredProcedure);
            }
        }
    }
}