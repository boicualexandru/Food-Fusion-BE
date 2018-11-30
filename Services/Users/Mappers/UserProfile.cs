using AutoMapper;
using DataAccess.Models;
using Services.Users.Models;

namespace Services.Users.Mappers
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserSimpleModel>();
            CreateMap<UserSimpleModel, User>()
                .ForMember(user => user.Id, opt => opt.Ignore());
        }
    }
}
