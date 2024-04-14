using AutoMapper;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO.ChatDTOs;
using MyMessenger.MApplication.DTO.MessagesDTOs;
using MyMessenger.MApplication.DTO.UserDTOs;

namespace MyMessenger.Options
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Chat, ChatDTO>().ReverseMap();
        }
    }
}
