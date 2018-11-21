using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;

namespace Freelance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IServiceTypesService _serviceTypesService;

        public AdminController(IServiceTypesService serviceTypesService)
        {
            _serviceTypesService = serviceTypesService;
        }

        public async Task<ActionResult> ServiceTypes()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();
            return View(serviceTypes);
        }

        //Admin/ServiceTypes/Edit/id
        public async Task<ActionResult> EditServiceType(int id)
        {
            ViewBag.Method = "Edit";

            var serviceType = await _serviceTypesService.GetServiceTypeByIdAsync(id);
            if (serviceType == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("AddServiceType", serviceType);
        }

        //Admin/ServiceTypes/Edit/id
        [HttpPost]
        public async Task<ActionResult> EditServiceType(ServiceType serviceType)
        {
            ViewBag.Method = "Edit";

            if (ModelState.IsValid)
            {
                await _serviceTypesService.UpdateServiceTypeAsync(serviceType);
                return RedirectToAction("ServiceTypes");
            }

            return View("AddServiceType", serviceType);
        }

        //Admin/ServiceTypes/Add/id
        public ActionResult AddServiceType()
        {
            ViewBag.Method = "Add";

            return View("AddServiceType", new ServiceType());
        }

        //Admin/ServiceTypes/Add/id
        [HttpPost]
        public async Task<ActionResult> AddServiceType(ServiceType serviceType)
        {
            ViewBag.Method = "Add";

            if (ModelState.IsValid)
            {
                await _serviceTypesService.AddServiceTypeAsync(serviceType);
                return RedirectToAction("ServiceTypes");
            }

            return View("AddServiceType", serviceType);
        }
    }
}