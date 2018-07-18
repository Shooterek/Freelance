using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Controllers
{
    public class AnnouncementsController : Controller
    {
        private IAnnouncementsService _announcementService;
        private IServiceTypesService _serviceTypesService;
        private const int PageSize = 5;

        public AnnouncementsController(IAnnouncementsService announcementService, IServiceTypesService serviceTypesService)
        {
            _announcementService = announcementService;
            _serviceTypesService = serviceTypesService;
        }

        // GET: Announcements
        public async Task<ActionResult> Index()
        {
            var result = await _announcementService.GetAnnouncementsAsync(1, PageSize);
            return View(result.Announcements);
        }

        // GET: Announcements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = await _announcementService.GetAnnouncementByIdAsync(id.Value);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        public async Task<ActionResult> Add()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            return View(new AddAnnouncementViewModel {ServiceTypes = new List<ServiceType>(serviceTypes)});
        }
    }
}
