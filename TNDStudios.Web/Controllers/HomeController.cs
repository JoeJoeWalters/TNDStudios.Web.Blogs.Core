using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.Web.Blogs.Core.ViewModels;
using TNDStudios.Web.Models;

namespace TNDStudios.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            BlogViewDisplaySettings displaySettings = new BlogViewDisplaySettings() { };
            displaySettings.ViewPorts[BlogViewSize.ExtraSmall].Columns =
                displaySettings.ViewPorts[BlogViewSize.Small].Columns = 1;
            displaySettings.ViewPorts[BlogViewSize.Medium].Columns = 2;
            displaySettings.ViewPorts[BlogViewSize.Large].Columns = 3;
            ViewBag.WidgetDisplaySettings = displaySettings;
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
