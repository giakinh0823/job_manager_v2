using Microsoft.EntityFrameworkCore;
using server.Entity;
using server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.DAO
{
    public class JobDAO
    {
        public static List<Job> All()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Jobs.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<Job>();
            }
        }

        public static Job? FindById(int id)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Jobs.FirstOrDefault(u => u.JobId == id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }


        public static List<Job> FindByUserId(int? userId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Jobs.Where(u => u.UserId == userId).OrderByDescending(job => job.CreatedAt).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<Job>();
            }
        }

        public static Job? FindByUserIdAndJobId(int? userId, int? jobId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Jobs.Include(job => job.Logs.OrderByDescending(log => log.StartTime)).Where(u => u.UserId == userId && u.JobId == jobId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new Job();
            }
        }


        public static void Add(Job job)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Jobs.Add(job);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(Job job)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Jobs.Update(job);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(Job job)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Jobs.Remove(job);
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
