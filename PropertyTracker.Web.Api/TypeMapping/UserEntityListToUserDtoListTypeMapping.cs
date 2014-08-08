using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PropertyTracker.Dto.Models;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class UserEntityListToUserDtoListTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<IEnumerable<Entity.Models.User>, Dto.Models.UserList>()
                .ForMember(dm => dm.Users, em => em.ResolveUsing<UserListResolver>())
                ; 
        }

        protected class UserListResolver : ValueResolver<IEnumerable<Entity.Models.User>, IList<Dto.Models.User>>
        {
            protected override IList<User> ResolveCore(IEnumerable<Entity.Models.User> source)
            {
                return Mapper.Map<IEnumerable<Entity.Models.User>, IList<Dto.Models.User>>(source);
            }
        }
    }
}