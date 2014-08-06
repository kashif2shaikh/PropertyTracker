using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class CompanyDtoToCompanyEntityTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Dto.Models.Company, Entity.Models.Company>()
                .ForMember(em => em.Properties, x => x.Ignore())
                .ForMember(em => em.Users, x => x.Ignore());
        }
    }
}