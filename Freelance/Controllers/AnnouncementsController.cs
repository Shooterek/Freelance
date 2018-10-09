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
using Microsoft.AspNet.Identity;

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
        public async Task<ActionResult> Index(int page)
        {   
            var result = await _announcementService.GetAnnouncementsAsync(page, PageSize);
            return View(result);
        }

        // GET: Announcements/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Announcement announcement = await _announcementService.GetAnnouncementByIdAsync(id);
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

        [ChildActionOnly]
        public ActionResult AddOffer(int id)
        {
            return PartialView(
                new AnnouncementOffer() {AnnouncementId = id, OffererId = User.Identity.GetUserId()});
        }

        [HttpPost]
        public async Task<ActionResult> AddOffer(AnnouncementOffer offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Today;
            var result = await _announcementService.AddOfferAsync(offer);
            return View("Index", "Home");
        }
    }
}
