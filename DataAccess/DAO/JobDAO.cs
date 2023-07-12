using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
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
