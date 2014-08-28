using System.Collections.Generic;
using AutoMapper;
using PropertyTracker.Dto.Models;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyEntityListToPropertyDtoListTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<IEnumerable<Entity.Models.Property>, Dto.Models.PropertyList>()
                .ForMember(dm => dm.Properties, em => em.ResolveUsing<PropertyListResolver>())
                ; 
        }

        protected class PropertyListResolver : ValueResolver<IEnumerable<Entity.Models.Property>, IList<Dto.Models.Property>>
        {
            protected override IList<Property> ResolveCore(IEnumerable<Entity.Models.Property> source)
            {
                return Mapper.Map<IEnumerable<Entity.Models.Property>, IList<Dto.Models.Property>>(source);
            }
        }
    }
}