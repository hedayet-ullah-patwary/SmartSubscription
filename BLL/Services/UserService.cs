using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class UserService
    {
        DataAccessFactory factory;

        public UserService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public UserResponseDTO UserLogin(LoginDTO dto)
        {
            var user = factory.GetUserRepository().UserLogin(dto.Email, dto.Password);

            if (user == null)
                return null;

            var mapper = MapperConfig.GetMapper();
            var response = mapper.Map<UserResponseDTO>(user);

            response.Role = factory.GetUserRoleRepository().GetRoleNameByUserId(user.Id);

            return response;
        }

        public UserDTO RegisterUser(UserDTO dto)
        {
            var mapper = MapperConfig.GetMapper();
            var user = mapper.Map<User>(dto);

            var exists = factory.GetUserRepository().GetByEmail(user.Email);

            if (exists != null)
                return null;

            user.CreatedAt = DateTime.Now;
            user.IsActive = 1;
            user.IsEmailVerified = 0;

            var result = factory.GetUserRepository().Create(user);

            if (!result)
                return null;

            var data = mapper.Map<UserDTO>(user);
            return data;
        }

        public UserDTO GetUserById(int id)
        {
            var data = factory.GetUserRepository().Find(id);

            var mapper = MapperConfig.GetMapper();
            return mapper.Map<UserDTO>(data);
        }

        public UserDTO GetUserByEmail(string email)
        {
            var data = factory.GetUserRepository().GetByEmail(email);
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<UserDTO>(data);
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = factory.GetUserRepository().GetAll();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<UserDTO>>(users);
        }

        public User GetUserEntityById(int id)
        {
            return factory.GetUserRepository().Find(id);
        }

        public bool UpdateUserProfile(UserDTO dto)
        {
            var user = factory.GetUserRepository().Find(dto.Id);

            if (user == null)
                return false;

            user.Name = dto.Name;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password;

            return factory.GetUserRepository().Update(user);
        }

        public bool UpdateUserAdmin(UserDTO dto)
        {
            var user = factory.GetUserRepository().Find(dto.Id);

            if (user == null)
                return false;

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.IsActive = dto.IsActive;
            user.IsEmailVerified = dto.IsEmailVerified;

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password;

            return factory.GetUserRepository().Update(user);
        }

        public bool SetUserActive(int userId, int isActive)
        {
            var user = factory.GetUserRepository().Find(userId);
            if (user == null)
                return false;

            user.IsActive = isActive;
            return factory.GetUserRepository().Update(user);
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            var user = factory.GetUserRepository().Find(userId);
            if (user == null)
                return false;

            user.Password = newPassword;
            return factory.GetUserRepository().Update(user);
        }

        public bool VerifyEmail(int userId)
        {
            return factory.GetUserRepository().VerifyEmail(userId);
        }

        public bool DeleteUser(int id)
        {
            return factory.GetUserRepository().Delete(id);
        }

        public List<PaymentDTO> GetUserPayments(int userId)
        {
            var data = factory.GetPaymentRepository().GetUserPayments(userId);

            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<PaymentDTO>>(data);
        }

        public List<SubscriptionDTO> GetUserSubscriptions(int userId)
        {
            var data = factory.GetSubscriptionRepository().GetUserSubscriptions(userId);

            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<SubscriptionDTO>>(data);
        }

    }
}