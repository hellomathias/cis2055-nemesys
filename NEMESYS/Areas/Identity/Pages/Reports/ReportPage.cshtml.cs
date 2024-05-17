using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NEMESYS.Areas.Identity.Pages.Reports.Models;

namespace NEMESYS.Areas.Pages.Reports
{
    public class ReportPageModel : PageModel
    {
        [BindProperty]
        public Report CurrentReport { get; set; } // The report being displayed or edited

        public void OnGet()
        {
            // Assuming you load report data from a database or another source here
            CurrentReport = new Report
            {
                DateOfReport = DateTime.Today,
                Location = "Initial Location",
                DateSpotted = DateTime.Today,
                TimeSpotted = DateTime.Now.ToString("HH:mm"),
                TypeOfHazard = "Unsafe Condition",
                Description = "Initial description",
                ReporterEmail = "reporter@example.com"
            };
        }

        // Example of a method that might be called by an investigator to update the status
        public IActionResult OnPostUpdateStatus(string newStatus)
        {
            if (User.IsInRole("Investigator")) // Ensure only authorized users can change status
            {
                CurrentReport.Status = newStatus;
                // Save changes to database
                return RedirectToPage("./Index"); // Redirect after post
            }
            return Page();
        }
    }
}
