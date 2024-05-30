using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using NEMESYS.Areas.Identity.Pages.Investigations.Models;
using NEMESYS.Data;
using System.Threading.Tasks;

namespace NEMESYS.Areas.Identity.Pages.Investigations
{
    public class InvestigationDetailsModel : PageModel
    {
        private readonly AuthDbContext _context;

        public InvestigationDetailsModel(AuthDbContext context)
        {
            _context = context;
        }

        public Report Report { get; set; }
        public Investigation Investigation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Investigation = await _context.Investigations.FirstOrDefaultAsync(i => i.ReportId == id);
            if (Investigation == null)
            {
                return NotFound();
            }

            Report = await _context.Reports.FindAsync(id);
            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
