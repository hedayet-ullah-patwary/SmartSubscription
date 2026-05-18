using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public class MapperConfig
{
    public static MapperConfiguration config = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<User, LoginDTO>().ReverseMap();
            cfg.CreateMap<User, UserDTO>().ReverseMap();
            cfg.CreateMap<User, RegisterDTO>().ReverseMap();
            cfg.CreateMap<User, UserResponseDTO>().ReverseMap();
            cfg.CreateMap<Feature, FeatureDTO>().ReverseMap();
            cfg.CreateMap<Plan, PlanDTO>().ReverseMap();
            cfg.CreateMap<Subcription, SubscriptionDTO>().ReverseMap();
            cfg.CreateMap<Role, RoleDTO>().ReverseMap();

            // Payment: entity uses SubcriptionId, DTO uses SubscriptionId — map both ways
            cfg.CreateMap<Payment, PaymentDTO>()
               .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.SubcriptionId))
               .ReverseMap()
               .ForMember(dest => dest.SubcriptionId, opt => opt.MapFrom(src => src.SubscriptionId));
        },
        NullLoggerFactory.Instance  // required in AutoMapper 15.x
    );

    public static IMapper GetMapper()
    {
        return config.CreateMapper();
    }
}
