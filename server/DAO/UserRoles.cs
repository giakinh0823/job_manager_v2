using server.Entity;
using server.Repository;

namespace server.DAO
{
    public class UserRoleDAO
    {
        public static List<UserRole> All()
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.UserRoles.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return new List<UserRole>();
            }
        }

        public static UserRole? FindById(int roleId)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    return db.UserRoles.FirstOrDefault(u => u.RoleId == roleId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
                return null;
            }
        }

        public static void Add(UserRole userRole)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.UserRoles.Add(userRole);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Update(UserRole userRole)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.UserRoles.Update(userRole);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error excute database: " + ex.Message);
            }
        }

        public static void Delete(UserRole userRole)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.UserRoles.Remove(userRole);
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
