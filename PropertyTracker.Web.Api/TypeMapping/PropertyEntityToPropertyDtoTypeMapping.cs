using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyEntityToPropertyDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Entity.Models.Property, Dto.Models.Property>();
            //.ForMember(dm => dm.Users, em => em.MapFrom(property => property.Users.Select(p => p.Id).ToList()))


        }
    }
}