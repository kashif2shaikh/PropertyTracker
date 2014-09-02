
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AutoMapper;
using PropertyTracker.Web.Api.AutoMapping;
using PropertyTracker.Web.Entity.Models;
using User = PropertyTracker.Dto.Models.User;

namespace PropertyTracker.Web.Api.TypeMapping
{
    public class UserEntityToUserDtoTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            Mapper.CreateMap<Entity.Models.User, Dto.Models.User>()
                .ForMember(dm => dm.Password, x => x.Ignore()) // don't want to send password
                .ForMember(dm => dm.PhotoUrl, x => x.Ignore())
                .ForMember(dm => dm.Properties, em => em.ResolveUsing<PropertiesResolver>())
                //.ForMember(dm => dm.Properties, em => em.MapFrom(user => user.Properties.Select(p => p.Id).ToList())) // Method 2 - use MapFrom with Linq to create idlist
                //.ForMember(dm => dm.PhotoUrl, em => em.ResolveUsing<PhotoResolver>()) - not working, let controllers handle this           
                ; 
        }
       
        public class PropertiesResolver : ValueResolver<Entity.Models.User, IEnumerable<Dto.Models.Property>>  
        {
            protected override IEnumerable<Dto.Models.Property> ResolveCore(Entity.Models.User user)
            {
                var propertyList = user.Properties.Select(p => new Dto.Models.Property
                {
                    Id = p.Id, 
                    Name = p.Name, 
                    City = p.City,
                    StateProvince = p.StateProvince,
                    Country = p.Country,
                    SquareFeet = p.SquareFeet,
                    CompanyId = p.CompanyId
                });

                return propertyList;
            }
        }        
    }
}