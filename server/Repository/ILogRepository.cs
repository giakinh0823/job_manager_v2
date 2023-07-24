using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ILogRepository : IRepository<Log>
    {
        List<Log> FindByJobId(int? jobId);
        void DeleteAll(List<Log> jobs);
    }
}
