using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PaymentInfoRepository : IPaymentInfoRepository
    {
        public List<PaymentInfo> All() => PaymentInfoDAO.All();

        public PaymentInfo? FindById(int id) => PaymentInfoDAO.FindById(id);

        public void Add(PaymentInfo entity) => PaymentInfoDAO.Add(entity);

        public void Delete(PaymentInfo entity) => PaymentInfoDAO.Delete(entity);

        public void Update(PaymentInfo entity) => PaymentInfoDAO.Update(entity);

        public List<PaymentInfo> FindByUserId(int? userId) => PaymentInfoDAO.FindByUserId(userId);

        public PaymentInfo? FindOneByUserId(int? userId) => PaymentInfoDAO.FindOneByUserId(userId);
    }
}
