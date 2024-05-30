using Microsoft.AspNetCore.Mvc;
using NEMESYS.Areas.Identity.Pages.HallOfFame.Models;
using NEMESYS.Data;
using System.Linq;
using System;

namespace NEMESYS.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class HallOfFameController : Controller
    {
        private readonly AuthDbContext _context;

        public HallOfFameController(AuthDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var currentYear = DateTime.Now.Year;
            var reports = _context.Reports
                .Where(r => r.DateOfReport.Year == currentYear)
                .ToList();

            var reporterRankings = reports
                .GroupBy(report => report.ReporterEmail)
                .Select(group => new ReporterRanking
                {
                    ReporterName = group.Key,
                    ReportCount = group.Count()
                })
                .OrderByDescending(r => r.ReportCount)
                .ToList();

            return View("~/Areas/Identity/Pages/HallOfFame/Index.cshtml", reporterRankings);
        }
    }
}
