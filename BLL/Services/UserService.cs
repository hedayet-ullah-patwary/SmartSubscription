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
        IMapper mapper;

        public UserService(DataAccessFactory factory, IMapper mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }

        public UserResponseDTO UserLogin(LoginDTO dto)
        {
            var user = factory.GetUserRepository().UserLogin(dto.Email, dto.Password);

            if (user == null)
                return null;

            var response = mapper.Map<UserResponseDTO>(user);

            response.Role = factory.GetUserRoleRepository().GetRoleNameByUserId(user.Id);

            return response;
        }

        public UserDTO RegisterUser(RegisterDTO dto)
        {
            var user = mapper.Map<User>(dto);

            var exists = factory.GetUserRepository().GetByEmail(user.Email);

            if (exists != null)
                return null;

            var result = factory.GetUserRepository().Create(user);

            if (!result)
                return null;

            var data = mapper.Map<UserDTO>(user);
            return data;
        }

        public UserDTO GetUserById(int id)
        {
            var data = factory.GetUserRepository().Find(id);

            return mapper.Map<UserDTO>(data);
        }

        public bool UpdateUser(UserDTO dto)
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

        public bool DeleteUser(int id)
        {
            return factory.GetUserRepository().Delete(id);
        }

        public List<PaymentDTO> GetUserPayments(int userId)
        {
            var data = factory.GetPaymentRepository().GetUserPayments(userId);
            return mapper.Map<List<PaymentDTO>>(data);
        }

        public List<SubscriptionDTO> GetUserSubscriptions(int userId)
        {
            var data = factory.GetSubscriptionRepository().GetUserSubscriptions(userId);
            return mapper.Map<List<SubscriptionDTO>>(data);
        }

    }
}