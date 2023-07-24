using server.DAO;
using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public List<Role> All() => RoleDAO.All();

        public Role? FindById(int id) => RoleDAO.FindById(id);

        public Role? findByName(string? name) => RoleDAO.FindByName(name);

        public void Add(Role entity) => RoleDAO.Add(entity);

        public void Delete(Role entity) => RoleDAO.Delete(entity);

        public void Update(Role entity) => RoleDAO.Update(entity);

    }
}
