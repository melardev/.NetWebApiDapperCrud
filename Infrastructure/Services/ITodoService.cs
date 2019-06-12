using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDapperCrud.Entities;
using WebApiDapperCrud.Enums;

namespace WebApiDapperCrud.Infrastructure.Services
{
    public interface ITodoService
    {
       Task<List<Todo>> FetchMany(TodoShow show = TodoShow.All);
       Task<Todo> FetchById(int id);
       Task<Todo> FetchProxyById(int id);
       Task<Todo> Create(Todo todo);
       Task<Todo> Update(Todo todoFromDb, Todo todoInput);
       Task<int> DeleteById(int id);
       Task DeleteAll();
    }
}