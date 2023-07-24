using server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IPaymentInfoRepository : IRepository<PaymentInfo>
    {
        List<PaymentInfo> FindByUserId(int? userId);

        PaymentInfo? FindOneByUserId(int? userId);
    }
}
