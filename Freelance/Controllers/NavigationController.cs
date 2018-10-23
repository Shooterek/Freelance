using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Controllers
{
    public class NavigationController : Controller
    {
        public ActionResult SearchBar()
        {
            return PartialView();
        }
    }
}