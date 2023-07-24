using server.DAO;
using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRoleRepository : IRepository<UserRole>
    {
        public List<UserRole> All() => UserRoleDAO.All();

        public UserRole? FindById(int id) => UserRoleDAO.FindById(id);

        public void Add(UserRole entity) => UserRoleDAO.Add(entity);

        public void Delete(UserRole entity) => UserRoleDAO.Delete(entity);

        public void Update(UserRole entity) => UserRoleDAO.Update(entity);
    }
}
