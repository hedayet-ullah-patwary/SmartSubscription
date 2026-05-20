using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User UserLogin(string email, string password);  

        User GetByEmail(string email); 
        List<User> GetUser(int IsActive);
        bool VerifyEmail(int userId);  
        bool DeactivateUser(int userId);  


    }
}
