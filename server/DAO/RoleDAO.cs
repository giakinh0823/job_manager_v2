using server.Entity;

namespace server.DAO
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

        public static void Delete(Role role)
        {
            try
            {
                using (var db = new JobManagerContext())
                {
                    db.Roles.Remove(role);
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
