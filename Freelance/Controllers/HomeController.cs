using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Repositories;

namespace Freelance.Controllers
{
    public class HomeController : Controller
    {
        private IServiceTypeRepository _serviceTypeRepository;

        public HomeController(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        public ActionResult Index()
        {
            _serviceTypeRepository.GetAllAsync();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}