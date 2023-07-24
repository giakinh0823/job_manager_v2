using server.DAO;
using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class JobRepository : IJobRepository
    {
        public void Add(Job entity) => JobDAO.Add(entity);

        public List<Job> All() => JobDAO.All();

        public void Delete(Job entity) => JobDAO.Delete(entity);

        public Job? FindById(int id) => JobDAO.FindById(id);

        public List<Job> FindByUserId(int? userId) => JobDAO.FindByUserId(userId);

        public Job? FindByUserIdAndJobId(int? userId, int? jobId) => JobDAO.FindByUserIdAndJobId(userId, jobId);

        public void Update(Job entity) => JobDAO.Update(entity);
    }
}
