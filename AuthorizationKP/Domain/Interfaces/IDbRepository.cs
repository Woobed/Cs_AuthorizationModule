using Microsoft.EntityFrameworkCore;
using AuthorizationKP.Domain.Entity;
using AuthorizationKP.Domain.Response;

namespace AuthorizationKP.Domain.Interfaces
{
    public interface IDbRepository
    {
        Task<bool> Add(Users entity);

        Task<IEnumerable<Users>> GetAll();
        Task<bool> Delete(Users entity);
        Task<Users> GetById(int id);
        Task<Users> GetByLogin(string log);
        Task<Users> Update(Users entity);


        //Task<List<Users>> Select(); 
    }
}
