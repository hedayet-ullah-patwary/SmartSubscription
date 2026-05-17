using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User UserLogin(string email, string password);  // Authenticate user

        User GetByEmail(string email); // Get user by email
        List<User> GetUser(int IsActive); // Get active and inactive users
        bool VerifyEmail(int userId);  // Mark email as verified
        bool DeactivateUser(int userId);  // Deactivate user account


    }
}
