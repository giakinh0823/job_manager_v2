using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IJobRepository : IRepository<Job>
    {
        List<Job> FindByUserId(int? userId);

        Job FindByUserIdAndJobId(int? userId, int? jobId);
    }
}
