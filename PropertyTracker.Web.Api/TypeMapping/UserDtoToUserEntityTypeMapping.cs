
using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class UserDtoToUserEntityTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Dto.Models.User, Entity.Models.User>()
                .ForMember(em => em.Photo, x => x.Ignore())
                //.ForMember(em => em.Password, x => x.Ignore()) Allow password to be updated from DTO
                .ForMember(em => em.Properties, x => x.Ignore())
                .ForMember(em => em.Company, x => x.Ignore());

        }
    }
}