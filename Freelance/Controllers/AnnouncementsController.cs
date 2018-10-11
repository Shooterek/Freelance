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
using Freelance.Infrastructure;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

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

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() {Value = s.ServiceTypeId.ToString(), Text = s.Name}));

            return View(new AddAnnouncementViewModel {ServiceTypes = servicesList});
        }

        [HttpPost]
        public async Task<ActionResult> Add(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.AdvertiserId = User.Identity.GetUserId();
                var result = await _announcementService.AddAnnouncementAsync(announcement);

                return View("Details", result);
            }

            return View("Add", announcement);
        }

        [ChildActionOnly]
        public ActionResult AddOffer(int announcementId)
        {
            return PartialView(
                new AnnouncementOffer() {AnnouncementId = announcementId, OffererId = User.Identity.GetUserId()});
        }

        [HttpPost]
        public async Task<ActionResult> SubmitOffer(AnnouncementOffer offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Today;
            var result = await _announcementService.AddOfferAsync(offer);
            return View("Details");
        }
    }
}
