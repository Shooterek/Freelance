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

namespace Freelance.Controllers
{
    public class AnnouncementsController : Controller
    {
        private IAnnouncementService _announcementService;
        private const int PageSize = 5;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
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

            Announcement announcement = await _announcementService.GetAnnouncementById(id.Value);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }
    }
}
