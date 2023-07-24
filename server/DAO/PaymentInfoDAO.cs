using server.Entity;
using server.Repository;

namespace server.DAO
{
    public class PaymentInfoDAO
    {
        public static List<PaymentInfo> All()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.PaymentInfos.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<PaymentInfo>();
            }
        }

        public static PaymentInfo? FindById(int paymentId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.PaymentInfos.FirstOrDefault(u => u.PaymentId == paymentId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static List<PaymentInfo> FindByUserId(int? userId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.PaymentInfos.Where(p => p.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<PaymentInfo>();
            }
        }

        public static PaymentInfo? FindOneByUserId(int? userId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.PaymentInfos.OrderByDescending(p => p.PaymentDate).FirstOrDefault(p => p.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static void Add(PaymentInfo paymentInfo)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.PaymentInfos.Add(paymentInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(PaymentInfo paymentInfo)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.PaymentInfos.Update(paymentInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(PaymentInfo paymentInfo)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.PaymentInfos.Remove(paymentInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }
    }
}
