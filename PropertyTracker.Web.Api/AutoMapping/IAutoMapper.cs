// IAutoMapper.cs
// Copyright Jamie Kurtz, Brian Wortman 2014.

namespace PropertyTracker.Web.Api.AutoMapping
{
    public interface IAutoMapper
    {
        T Map<T>(object objectToMap);
    }
}