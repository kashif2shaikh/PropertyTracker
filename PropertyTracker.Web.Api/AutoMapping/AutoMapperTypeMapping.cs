// AutoMapperConfigurator.cs
// Copyright Jamie Kurtz, Brian Wortman 2014.

using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace PropertyTracker.Web.Api.AutoMapping
{
    public class AutoMapperTypeMapping : IAutoMapperTypeMapping
    {
        public void Configure()
        {
            // Configures all DI-registered automapper type mappings
            Configure(WebApiConfig.ResolveAllTypes<IAutoMapperTypeMapping>());
        }
        
        public void Configure(IEnumerable<IAutoMapperTypeMapping> autoMapperTypeConfigurations)
        {
            autoMapperTypeConfigurations.ToList().ForEach(x => x.Configure());

            Mapper.AssertConfigurationIsValid();
        }
    }
}