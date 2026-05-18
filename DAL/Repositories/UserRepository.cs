using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public bool DeactivateUser(int userId)
        {
            var user = Find(userId);
            if (user == null)
                return false;

            user.IsActive = 0;
            return Update(user);
        }

        public User GetByEmail(string email)
        {
            return db.Users.FirstOrDefault(x => x.Email == email);
        }

        public List<User> GetUser(int IsActive)
        {
            return db.Users.Where(x => x.IsActive == IsActive).ToList();
        }

        public User UserLogin(string email, string password)
        {
            return db.Users.FirstOrDefault(x =>
                x.Email == email && x.Password == password && x.IsActive == 1);
        }

        public bool VerifyEmail(int userId)
        {
            var user = Find(userId);
            if (user == null)
                return false;

            user.IsEmailVerified = 1;
            return Update(user);
        }

    }
}
