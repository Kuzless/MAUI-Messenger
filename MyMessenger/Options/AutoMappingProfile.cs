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
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Chat, ChatDTO>().ReverseMap();
        }
    }
}
