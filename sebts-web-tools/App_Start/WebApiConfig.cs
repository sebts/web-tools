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
            var jsonpFormatter = new JsonpMediaTypeFormatter(GlobalConfiguration.Configuration.Formatters.JsonFormatter);
            GlobalConfiguration.Configuration.Formatters.Insert(0, jsonpFormatter);
            jsonpFormatter.AddQueryStringMapping("media", "json", JsonMediaTypeFormatter.DefaultMediaType);
            jsonpFormatter.AddQueryStringMapping("media", "javascript", "application/javascript");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.AddQueryStringMapping(
                "media", "xml", XmlMediaTypeFormatter.DefaultMediaType);
        }
    }
}
