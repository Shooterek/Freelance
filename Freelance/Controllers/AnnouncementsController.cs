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
using Freelance.Infrastructure.ViewModels.Announcements;
using Freelance.Utilities;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Freelance.Controllers
{
    [Authorize]
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
        
        [AllowAnonymous]
        public async Task<ActionResult> Index(int page, decimal minWage = Decimal.Zero, decimal maxWage = Decimal.MaxValue,
            string[] availability = null, string localization = null, int? serviceType = null, string sort = null)
        {
            var result = await _announcementService.GetAnnouncementsAsync(page, PageSize, minWage, maxWage, availability, localization, serviceType, sort);
            return View(result);
        }
        
        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> Details(int id)
        {
            var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }
        
        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> Add()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() {Value = s.ServiceTypeId.ToString(), Text = s.Name}));

            return View(new AddAnnouncementViewModel {ServiceTypes = servicesList});
        }
        
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 1)]
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
        
        [ChildActionOnly]
        public ActionResult AddAnnouncementOffer(int announcementId)
        {
            return PartialView(
                new AnnouncementOfferViewModel() {AnnouncementId = announcementId});
        }
        
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> SubmitOffer([Bind(Exclude = "SubmissionDate, OffererId")] AnnouncementOfferViewModel offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Now;
            var result = await _announcementService.AddOfferAsync(offer);
            return RedirectToAction("Details", new {id = offer.AnnouncementId});
        }
        
        [HttpPost]
        public async Task<ActionResult> AcceptOffer(int id)
        {
            var result = await _announcementService.AcceptOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Details", new {id = result.AnnouncementId});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EndOffer(int id)
        {
            var result = await _announcementService.EndOfferAsync(id, User.Identity.GetUserId());

            return RedirectToAction("Details", new { id = result.AnnouncementId });
        }
    }
}
