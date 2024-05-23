using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NEMESYS.Data;

namespace NEMESYS.Areas.Identity.Pages.Reports
{
    public class ReportBoardModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportBoardModel(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Report> Reports { get; set; }

        public async Task OnGetAsync()
        {
            Reports = await _context.Reports.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (report.UserId != currentUser.Id)
            {
                return Forbid();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
