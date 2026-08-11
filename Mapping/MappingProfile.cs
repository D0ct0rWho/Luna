using AutoMapper;
using Luna.Dtos;
using Luna.Models;

namespace Luna.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Products

            // Из сущности в DTO для ответа
            CreateMap<Product, ProductDto>();
            // Из DTO создания в сущность
            CreateMap<CreateProductDto, Product>();
            // Из DTO обновления в сущность
            CreateMap<UpdateProductDto, Product>();

            // Contacts
            CreateMap<Contact, ContactDto>()
                .ForMember(dst => dst.ContactUsername, opt => opt.MapFrom(src => src.ContactUser.UserName));

            // Chats
            CreateMap<Chat, ChatDto>()
                .ForMember(dst => dst.ParticipantUsernames, opt => opt.MapFrom(src => src.Participants.Select(p => p.User.UserName)));

            // Messages
            CreateMap<Message, MessageDto>()
                .ForMember(dst => dst.SenderUsername, opt => opt.MapFrom(src => src.Sender.UserName));
        }
    }
}