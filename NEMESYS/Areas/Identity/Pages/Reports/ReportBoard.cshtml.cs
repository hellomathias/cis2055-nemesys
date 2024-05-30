using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Investigations.Models;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using NEMESYS.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IList<Investigation> Investigations { get; set; }
        public IList<Upvote> Upvotes { get; set; }
        public Dictionary<int, bool> UserUpvotes { get; set; } = new Dictionary<int, bool>();

        public async Task OnGetAsync()
        {
            Reports = await _context.Reports.ToListAsync();
            Investigations = await _context.Investigations.ToListAsync();
            Upvotes = await _context.Upvotes.ToListAsync();

            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                UserUpvotes = await _context.Upvotes
                    .Where(u => u.UserId == userId)
                    .ToDictionaryAsync(u => u.ReportId, u => true);
            }
        }

        public async Task<IActionResult> OnPostToggleUpvoteAsync(int reportId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Forbid();
            }

            var existingUpvote = await _context.Upvotes.FirstOrDefaultAsync(u => u.ReportId == reportId && u.UserId == userId);
            var report = await _context.Reports.FindAsync(reportId);
            if (existingUpvote != null)
            {
                _context.Upvotes.Remove(existingUpvote);
                if (report != null)
                {
                    report.Upvotes--;
                    _context.Reports.Update(report);
                }
            }
            else
            {
                var upvote = new Upvote
                {
                    ReportId = reportId,
                    UserId = userId
                };
                _context.Upvotes.Add(upvote);
                if (report != null)
                {
                    report.Upvotes++;
                    _context.Reports.Update(report);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
