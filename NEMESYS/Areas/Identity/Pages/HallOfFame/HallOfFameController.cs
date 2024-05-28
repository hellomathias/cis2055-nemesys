using Microsoft.AspNetCore.Mvc;
using NEMESYS.Areas.Identity.Pages.HallOfFame.Models;

namespace NEMESYS.Areas.Identity.Pages.HallOfFame
{
	public class HallOfFameController : Controller
	{
		private readonly IReportRepository _reportRepository;

		public HallOfFameController(IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
		}

		public IActionResult Index()
		{
			var reports = _reportRepository.GetAllReports();
			var reporterRankings = reports
				.GroupBy(report => report.ReporterName)
				.Select(group => new ReporterRanking
				{
					ReporterName = group.Key,
					ReportCount = group.Count()
				})
				.OrderByDescending(r => r.ReportCount)
				.ToList();

			return View(reporterRankings);
		}
	}

}
