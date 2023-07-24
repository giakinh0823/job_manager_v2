using server.DAO;
using server.Entity;

namespace DataAccess.Repository
{
    public class LogRepository : ILogRepository
    {
        public List<Log> All() => LogDAO.All();

        public Log? FindById(int id) => LogDAO.FindById(id);

        public void Add(Log entity) => LogDAO.Add(entity);
        public void Delete(Log entity) => LogDAO.Delete(entity);

        public void Update(Log entity) => LogDAO.Update(entity);

        public List<Log> FindByJobId(int? jobId) => LogDAO.FindByJobId(jobId);

        public void DeleteAll(List<Log> logs) => LogDAO.DeleteAll(logs);
    }
}
