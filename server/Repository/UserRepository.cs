using server.DAO;
using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public List<User> All() => UserDAO.All();

        public User? FindById(int id) => UserDAO.FindById(id);
        public User? FindByEmail(string? email) => UserDAO.FindByEmail(email);

        public void Add(User entity) => UserDAO.Add(entity);

        public void Delete(User entity) => UserDAO.Delete(entity);

        public void Update(User entity) => UserDAO.Update(entity);
    }
}
