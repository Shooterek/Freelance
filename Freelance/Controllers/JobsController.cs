using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Jobs;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Freelance.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page, decimal minWage = Decimal.One, decimal maxWage = Decimal.MaxValue,
            string[] availability = null, string localization = null, int? serviceType = null, string sort = null)
        {
            var result = await _jobsService.GetJobsAsync(page, PageSize, minWage, maxWage, availability, localization, serviceType, sort);
            return View(result);
        }

        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> Details(int id)
        {
            var job = await _jobsService.GetJobByIdAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> Add()
        {
            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));

            return View(new AddJobViewModel { ServiceTypes = servicesList });
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 1)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Exclude = "ServiceTypes")]AddJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var job = viewModel.Job;
                job.EmployerId = User.Identity.GetUserId();

                viewModel.Photos.Where(p => p != null).ForEach(p =>
                {
                    var photo = new Photo { Content = new byte[p.ContentLength] };
                    p.InputStream.Read(photo.Content, 0, p.ContentLength);

                    photo.ContentType = p.ContentType;
                    job.Photos.Add(photo);
                });

                var result = await _jobsService.AddJobAsync(job);

                return RedirectToAction("Details", new { id = result.JobId });
            }

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();

            var servicesList = new List<SelectListItem>();
            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name }));
            viewModel.ServiceTypes = servicesList;

            return View("Add", viewModel);
        }

        [ChildActionOnly]
        public ActionResult AddJobOffer(int jobId)
        {
            return PartialView(new JobOfferViewModel() { JobId = jobId });
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> SubmitOffer([Bind(Exclude = "SubmissionDate, OffererId")] JobOfferViewModel offer)
        {
            offer.OffererId = User.Identity.GetUserId();
            offer.SubmissionDate = DateTime.Now;
            var result = await _jobsService.AddOfferAsync(offer);

            if (result == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [HttpPost]
        public async Task<ActionResult> AcceptOffer(int id)
        {
            var result = await _jobsService.AcceptOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Details", new { id = result.JobId });
        }

        [HttpPost]
        public async Task<ActionResult> EndOffer(int id)
        {
            var result = await _jobsService.EndOfferAsync(id, User.Identity.GetUserId());
            return RedirectToAction("Details", new { id = result.JobId });
        }
    }
}