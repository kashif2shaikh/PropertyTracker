using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyDtoToPropertyEntityTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            //Mapper.CreateMap<Dto.Models.Property, Entity.Models.Property>();
        }
    }
}