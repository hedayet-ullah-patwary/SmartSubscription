using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Users, LoginDTO>().ReverseMap();
            cfg.CreateMap<Users, UserDTO>().ReverseMap();
            cfg.CreateMap<Users, RegisterDTO>().ReverseMap();
            cfg.CreateMap<Users, UserResponseDTO>().ReverseMap();
            cfg.CreateMap<Features, FeatureDTO>().ReverseMap();
            cfg.CreateMap<Payments, PaymentDTO>().ReverseMap();
            cfg.CreateMap<Plans, PlanDTO>().ReverseMap();
            cfg.CreateMap<Subscriptions, SubscriptionDTO>().ReverseMap();
            cfg.CreateMap<Roles, RoleDTO>().ReverseMap();
            cfg.CreateMap<ApiUsage, ApiUsageDTO>().ReverseMap();
        });
        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}