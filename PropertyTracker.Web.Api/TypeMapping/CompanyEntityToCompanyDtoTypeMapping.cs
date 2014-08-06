
using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class CompanyEntityToCompanyDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Entity.Models.Company, Dto.Models.Company>();
        }
    }
}