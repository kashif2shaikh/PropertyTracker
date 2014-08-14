using System.Net.Http.Headers;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Newtonsoft.Json;
using PropertyTracker.Web.Api.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Tracing;
using System.Web.Http.Dependencies;
using PropertyTracker.Web.Api.Security;

namespace PropertyTracker.Web.Api
{
    public static class WebApiConfig
    {
       

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //var constraintsResolver = new DefaultInlineConstraintResolver();
            //constraintsResolver.ConstraintMap.Add("apiVersionConstraint", typeof(ApiVersionConstraint));
            //config.MapHttpAttributeRoutes(constraintsResolver);

            // Remove the XML formatter
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Add JSON return type by default, even when text/html is specified
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            
            //config.Formatters.JsonFormatter.MaxDepth = 1;

            // Ignore null values - don't emit properties with Json Nulls
            //config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            /*
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
     = Newtonsoft.Json.ReferenceLoopHandling.Serialize;

            
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling
                 = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            
           */

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
            config.Services.Replace(typeof(ITraceWriter),
               new SimpleTraceWriter(WebContainerManager.Get<ILogManager>()));

            config.Services.Add(typeof(IExceptionLogger),
                new SimpleExceptionLogger(WebContainerManager.Get<ILogManager>()));

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
             */

            EntityFrameworkProfiler.Initialize();

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthenticationMessageHandler());
        }
        /// <summary>
        ///     Provides access to dependencies managed by the <see cref="IDependencyResolver" />. Useful where
        ///     access to the resolver is not convenient/possible.
        /// </summary>
        public static IDependencyResolver GetDependencyResolver()
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyResolver != null)
            {
                return dependencyResolver;
            }

            throw new InvalidOperationException("The dependency resolver has not been set.");
        }

        /// <summary>
        ///     Provides access to a specific type of dependency managed by the <see cref="IDependencyResolver" />. Use only
        ///     where access to the resolver is not convenient/possible.
        /// </summary>
        public static T ResolveType<T>()
        {
            var service = GetDependencyResolver().GetService(typeof(T));

            if (service == null)
                throw new NullReferenceException(string.Format("Requested service of type {0}, but null was found.",
                    typeof(T).FullName));

            return (T)service;
        }

        /// <summary>
        ///     Provides access to a specific type of dependency managed by the <see cref="IDependencyResolver" />. Use only
        ///     where access to the resolver is not convenient/possible.
        /// </summary>
        public static IEnumerable<T> ResolveAllTypes<T>()
        {
            var services = GetDependencyResolver().GetServices(typeof(T)).ToList();

            if (!services.Any())
                throw new NullReferenceException(string.Format("Requested services of type {0}, but none were found.",
                    typeof(T).FullName));

            return services.Cast<T>();
        }
    }
}
