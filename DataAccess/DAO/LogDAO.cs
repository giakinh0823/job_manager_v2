using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LogDAO
    {
        public static List<Log> All()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Logs.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<Log>();
            }
        }

        public static Log? FindById(int id)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Logs.FirstOrDefault(u => u.LogId == id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }


        public static void Add(Log log)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Logs.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(Log log)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Logs.Update(log);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(Log log)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Logs.Remove(log);
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
