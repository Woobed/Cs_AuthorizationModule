using AuthorizationKP.Domain.Data;
using AuthorizationKP.Domain.Entity;
using AuthorizationKP.Domain.Interfaces;
using AuthorizationKP.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationKP.Domain.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly AppDbContext _appDbContext;

        public DbRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Add(Users user)
        {
           
            await _appDbContext.Users.AddAsync(user);
            _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Users> GetById(int id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Users> GetByLogin(string log)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Login == log);
        }

        public async Task<IEnumerable<Users>> GetAll() 
        {

            return await _appDbContext.Users.ToListAsync();
        }

        public async Task<Users> Update (Users entity)
        {
            _appDbContext.Users.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }



        public async Task <bool> Delete(Users entity)
        {
            _appDbContext.Users.Remove(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
