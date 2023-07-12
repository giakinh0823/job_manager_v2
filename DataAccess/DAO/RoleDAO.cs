using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class RoleDAO
    {
        public static List<Role> All()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Roles.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<Role>();
            }
        }

        public static Role? FindById(int roleId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Roles.FirstOrDefault(u => u.RoleId == roleId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static Role? FindByName(string? name)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Roles.FirstOrDefault(u => name != null && u.Name.ToLower() == name.ToLower());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static void Add(Role role)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(Role role)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Roles.Update(role);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(Role user)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Roles.Remove(user);
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
