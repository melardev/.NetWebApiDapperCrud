using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace WebApiDapperCrud
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            /*
             *  
             var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
              formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
             */


            config.EnableCors();

            // var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            // config.EnableCors(cors);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
