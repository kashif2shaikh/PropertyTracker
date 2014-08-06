// AutoMapperAdapter.cs
// Copyright Jamie Kurtz, Brian Wortman 2014.

using AutoMapper;

namespace PropertyTracker.Web.Api.AutoMapping
{
    public class AutoMapperAdapter : IAutoMapper
    {
        public T Map<T>(object objectToMap)
        {
            return Mapper.Map<T>(objectToMap);
        }
    }
}