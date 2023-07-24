using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        List<T> All();
        T? FindById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
