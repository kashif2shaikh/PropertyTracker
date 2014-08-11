using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyEntityToPropertyDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            //Mapper.CreateMap<Entity.Models.Property, Dto.Models.Property>();
        }
    }
}