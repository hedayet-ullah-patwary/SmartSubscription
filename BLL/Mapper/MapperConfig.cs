using AutoMapper;
using AutoMapper.Features;
using BLL.DTOs;
using DAL.EF.Tables;
using Microsoft.Extensions.Logging;

public class MapperConfig
{
    public static MapperConfiguration config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<User, LoginDTO>().ReverseMap();
        cfg.CreateMap<User, UserDTO>().ReverseMap();
        cfg.CreateMap<User, RegisterDTO>().ReverseMap();
        cfg.CreateMap<User, UserResponseDTO>().ReverseMap();
        cfg.CreateMap<Feature, FeatureDTO>().ReverseMap();
        cfg.CreateMap<Payment, PaymentDTO>().ReverseMap();
        cfg.CreateMap<Plan, PlanDTO>().ReverseMap();
        cfg.CreateMap<Subcription, SubscriptionDTO>().ReverseMap();
        cfg.CreateMap<Role, RoleDTO>().ReverseMap();

    }, null);

    public static IMapper GetMapper()
    {
        return config.CreateMapper();
    }
}
