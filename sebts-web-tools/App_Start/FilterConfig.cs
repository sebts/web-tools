﻿using System.Web;
using System.Web.Mvc;

namespace sebts_web_tools
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}