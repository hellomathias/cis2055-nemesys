using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using NEMESYS.Data;
using System.Threading.Tasks;

namespace NEMESYS.Areas.Identity.Pages.Reports
{
    public class ReportDetailsModel : PageModel
    {
        private readonly AuthDbContext _context;

        public ReportDetailsModel(AuthDbContext context)
        {
            _context = context;
        }

        public Report Report { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Report = await _context.Reports.FirstOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
