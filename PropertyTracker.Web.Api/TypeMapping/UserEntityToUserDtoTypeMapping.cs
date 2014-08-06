
using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class UserEntityToUserDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Entity.Models.User, Dto.Models.User>();
        }
    }
}