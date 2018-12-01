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
        private IAnnouncementsService _announcementsService;
        private IEmailService _emailService;

        public AdminController(IServiceTypesService serviceTypesService, IAnnouncementsService announcementsService, IEmailService emailService)
        {
            _serviceTypesService = serviceTypesService;
            _announcementsService = announcementsService;
            _emailService = emailService;
        }

        public async Task<ActionResult> ServiceTypes()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();
            return View(serviceTypes);
        }
        
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
        
        public ActionResult AddServiceType()
        {
            ViewBag.Method = "Add";

            return View("AddServiceType", new ServiceType());
        }
        
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
        
        public async Task<ActionResult> SendNotifications()
        {
            var announcements = await _announcementsService.GetOldAnnouncementsAsync();

            foreach (var announcement in announcements)
            {
                await _emailService.SendNotification(announcement);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}