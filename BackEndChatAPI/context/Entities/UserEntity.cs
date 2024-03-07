using AutoMapper;
using BackEndChatAPI.Models;

namespace BackEndChatAPI.context.Entities
{
    public class UserEntity : User, IEntityBase
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class UserEntityMappingProfile : Profile
    {
        public UserEntityMappingProfile() 
        {
            CreateMap<User, UserEntity>().ReverseMap();
        }
    }
}
