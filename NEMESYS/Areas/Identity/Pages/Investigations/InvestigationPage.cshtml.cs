using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NEMESYS.Data;

namespace NEMESYS.Areas.Identity.Pages.Investigations
{
    public class InvestigationPageModel : PageModel
    {
        private readonly AuthDbContext _context;

        public InvestigationPageModel(AuthDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Investigation CurrentInvestigation { get; set; }

        public void OnGet(int reportId)
        {
            CurrentInvestigation = new Investigation { ReportId = reportId };
        }

    }

}
