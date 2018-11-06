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
using Freelance.Utilities;
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
        public async Task<ActionResult> Index(int page, decimal minWage = Decimal.One, decimal maxWage = Decimal.MaxValue, string[] availability = null, string localization = null)
        {
            var result = await _announcementService.GetAnnouncementsAsync(page, PageSize, minWage, maxWage, availability, localization);
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

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));
            return View("Add", new AddAnnouncementViewModel() {Announcement = announcement, ServiceTypes = servicesList});
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
            return RedirectToAction("Details", new {id = offer.AnnouncementId});
        }

        public async Task<ActionResult> AcceptOffer(int id)
        {
            await _announcementService.AcceptOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Offers", "Account");
        }
        
        public async Task<ActionResult> DeclineOffer(int id)
        {
            await _announcementService.DeclineOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Offers", "Account");
        }
    }
}
