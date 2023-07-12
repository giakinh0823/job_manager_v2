using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class UserDAO
    {

        public static List<User> All ()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Users.Include(b => b.UserRoles).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<User>();
            }
        }

        public static User? FindById(int userId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Users.FirstOrDefault(u => u.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static User? FindByEmail(string? email)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.Users
                        .Include(u => u.UserRoles)
                        .ThenInclude(x => x.Role)
                        .FirstOrDefault(u => u.Email == email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static void Add(User user)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(User user)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Users.Update(user);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(User user)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Users.Remove(user);
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
