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
        private readonly IAnnouncementsService _announcementService;
        private readonly IServiceTypesService _serviceTypesService;
        private const int PageSize = 6;

        public AnnouncementsController(IAnnouncementsService announcementService, IServiceTypesService serviceTypesService)
        {
            _announcementService = announcementService;
            _serviceTypesService = serviceTypesService;
        }

        // GET: Announcements
        public async Task<ActionResult> Index(int page, decimal minWage = Decimal.One, decimal maxWage = Decimal.MaxValue,
            string[] availability = null, string localization = null, int? serviceType = null, string sort = null)
        {
            var result = await _announcementService.GetAnnouncementsAsync(page, PageSize, minWage, maxWage, availability, localization, serviceType, sort);
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

        [Authorize]
        public async Task<ActionResult> Add()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() {Value = s.ServiceTypeId.ToString(), Text = s.Name}));

            return View(new AddAnnouncementViewModel {ServiceTypes = servicesList});
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Exclude = "ServiceTypes")]AddAnnouncementViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var announcement = viewModel.Announcement;
                announcement.AdvertiserId = User.Identity.GetUserId();

                viewModel.Photos.Where(p => p != null).ForEach(p =>
                {
                    var photo = new Photo {Content = new byte[p.ContentLength]};
                    p.InputStream.Read(photo.Content, 0, p.ContentLength);

                    photo.ContentType = p.ContentType;
                    announcement.Photos.Add(photo);
                });

                var result = await _announcementService.AddAnnouncementAsync(announcement);

                return RedirectToAction("Details", new {id = result.AnnouncementId});
            }

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));
            viewModel.ServiceTypes = servicesList;

            return View("Add", viewModel);
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult AddOffer(int announcementId)
        {
            return PartialView(
                new AnnouncementOffer() {AnnouncementId = announcementId, OffererId = User.Identity.GetUserId()});
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SubmitOffer([Bind(Exclude = "SubmissionDate, OffererId")] AnnouncementOffer offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Now;
            var result = await _announcementService.AddOfferAsync(offer);
            return RedirectToAction("Details", new {id = offer.AnnouncementId});
        }

        [Authorize]
        public async Task<ActionResult> AcceptOffer(int id)
        {
            await _announcementService.AcceptOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Offers", "Account");
        }
        
        [Authorize]
        public async Task<ActionResult> DeclineOffer(int id)
        {
            await _announcementService.DeclineOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Offers", "Account");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> EndOffer(int id)
        {
            await _announcementService.EndOfferAsync(id, User.Identity.GetUserId());

            return RedirectToAction("Index", "Home");
        }
    }
}
