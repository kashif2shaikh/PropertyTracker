using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyDtoToPropertyEntityTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Dto.Models.Property, Entity.Models.Property>()
                .ForMember(em => em.Users, x => x.Ignore())                
                //.ForMember(em => em.Company, x => x.Ignore())
                ;
        }
    }
}