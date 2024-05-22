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
            // Initialization or loading of CurrentReport for display/editing
            CurrentReport = new Report
            {
                DateOfReport = DateTime.Today,
                Location = "Initial Location",
                DateSpotted = DateTime.Today,
                TimeSpotted = DateTime.Now.ToString("HH:mm"),
                TypeOfHazard = "Unsafe Condition",
                Description = "Initial description",
                Status = "Open", // Assuming status is also managed
                ReporterEmail = "reporter@example.com"
            };
        }

        // Example of a method that might be called by an investigator to update the status
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page if validation fails
            }

            // Here you would save the CurrentReport to the database

            return RedirectToPage("./Index"); // Redirect to another page after successful post
        }
    }
}
