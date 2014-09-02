using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using PropertyTracker.Dto.Models;
using PropertyTracker.Web.Api.AutoMapping;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class PropertyEntityToPropertyDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Entity.Models.Property, Dto.Models.Property>()                                
                .ForMember(dm => dm.Users, em => em.ResolveUsing<UserListResolver>())
                ;            
        }

        protected class UserListResolver : ValueResolver<Entity.Models.Property, IEnumerable<User>>
        {
            protected override IEnumerable<User> ResolveCore(Entity.Models.Property property)
            {
                var userList = property.Users.Select(u => new User{Id=u.Id,Fullname=u.Fullname,Username=u.Username});
                return userList;
            }
        }
    }
}