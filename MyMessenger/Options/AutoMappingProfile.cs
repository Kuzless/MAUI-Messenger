using AutoMapper;
using MyMessenger.Domain.Entities;
using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.DTO.UserDTOs;

namespace MyMessenger.Options
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name)).ReverseMap();
            CreateMap<Chat, ChatDTO>().ReverseMap();
        }
    }
}
