using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;

namespace sebts_web_tools
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            RegisterFormatters();
        }

        static void RegisterFormatters()
        {
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var jsonpFormatter = new JsonpMediaTypeFormatter(jsonFormatter);
            GlobalConfiguration.Configuration.Formatters.Insert(0, jsonpFormatter);

            jsonFormatter.AddQueryStringMapping("format", "json", "application/json");            
            jsonpFormatter.AddQueryStringMapping("format", "jsonp", "application/jsonp");            
        }
    }
}
