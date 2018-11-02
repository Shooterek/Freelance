using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Freelance.Controllers
{
    public class JobsController : Controller
    {
        private IJobsService _jobsService;
        private IServiceTypesService _serviceTypesService;
        private const int PageSize = 5;

        public JobsController(IJobsService jobsService, IServiceTypesService serviceTypesService)
        {
            _jobsService = jobsService;
            _serviceTypesService = serviceTypesService;
        }

        // GET: Announcements
        public async Task<ActionResult> Index(int page, string[] availability = null)
        {
            var result = await _jobsService.GetJobsAsync(page, PageSize, availability);
            return View(result);
        }

        // GET: Announcements/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Job job = await _jobsService.GetJobByIdAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        public async Task<ActionResult> Add()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));

            return View(new AddAnnouncementViewModel { ServiceTypes = servicesList });
        }

        [HttpPost]
        public async Task<ActionResult> Add(Job job)
        {
            if (ModelState.IsValid)
            {
                job.EmployerId = User.Identity.GetUserId();
                var result = await _jobsService.AddJobAsync(job);

                return View("Details", result);
            }

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));

            return View("Add", new AddJobViewModel { Job = job, ServiceTypes = servicesList });
        }

        [ChildActionOnly]
        public ActionResult AddOffer(int jobId)
        {
            return PartialView(
                new JobOffer() { JobId = jobId, OffererId = User.Identity.GetUserId() });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitOffer(JobOffer offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Today;
            var result = await _jobsService.AddOfferAsync(offer);
            return RedirectToAction("Details", new { id = offer.JobId });
        }
        
        public async Task<ActionResult> AcceptOffer(int id)
        {
            var userId = User.Identity.GetUserId();
            await _jobsService.AcceptOfferAsync(id, userId);
            return RedirectToAction("Offers", "Account");
        }

        public async Task<ActionResult> Decline(int id)
        {
            var userId = User.Identity.GetUserId();
            await _jobsService.DeclineOfferAsync(id, userId);
            return RedirectToAction("Offers", "Account");
        }
    }
}