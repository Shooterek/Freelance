using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Microsoft.AspNet.Identity;
using Constants = Freelance.Infrastructure.Utils.Constants;

namespace Freelance.Controllers
{
    [Authorize]
    public class OpinionsController : Controller
    {
        private IOpinionsService _opinionsService;

        public OpinionsController(IOpinionsService opinionsService)
        {
            _opinionsService = opinionsService;
        }

        public async Task<ActionResult> Add(int? announcementOfferId, int? jobOfferId, string evaluatedUserId)
        {
                
            if (evaluatedUserId == null || (announcementOfferId == null && jobOfferId == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var canAdd = await _opinionsService.CanAddOpinion(User.Identity.GetUserId(), evaluatedUserId,
                announcementOfferId ?? jobOfferId.Value,
                announcementOfferId == null ? Constants.JobOffer : Constants.AnnouncementOffer);

            if (!canAdd)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opinion = new Opinion()
            {
                EvaluatedUserId = evaluatedUserId,
                JobOfferId = jobOfferId,
                AnnouncementOfferId = announcementOfferId
            };

            return View(new AddOpinionViewModel {Opinion = opinion, PossibleGrades = GetPossibleGrades()});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Opinion opinion)
        {
            if (ModelState.IsValid)
            {
                var result = await _opinionsService.AddOpinionAsync(opinion);

                if (result == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                return RedirectToAction("Details", result.AnnouncementOfferId == null ? "Jobs" : "Announcements",
                    new {id = result.AnnouncementOfferId != null ? result.AnnouncementOffer.AnnouncementId : result.JobOffer.JobId});
            }

            return View(new AddOpinionViewModel {Opinion = opinion, PossibleGrades = GetPossibleGrades()});
        }

        public async Task<ActionResult> GetOpinions(string userId)
        {
            var opinions = await _opinionsService.GetOpinionsByEvaluatedUserId(userId);

            return PartialView("OpinionsList", opinions);
        }

        private IEnumerable<SelectListItem> GetPossibleGrades()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
            };
        }
    }
}