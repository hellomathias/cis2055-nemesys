using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NEMESYS.Controllers
{
    public class ReportsController : Controller
    {
        [Authorize]
        public IActionResult ReportPage()
        {
            return RedirectToPage("/Reports/ReportPage", new { area = "Identity" });
        }

        public IActionResult RedirectToReport()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                TempData["ReturnMessage"] = "Please log in to continue to report an incident.";
                var returnUrl = Url.Page("/Reports/ReportPage", new { area = "Identity" });
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
            }
            return RedirectToPage("/Reports/ReportPage", new { area = "Identity" });
        }
    }
}
