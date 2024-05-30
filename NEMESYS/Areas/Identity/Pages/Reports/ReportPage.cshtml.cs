using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using System;

namespace NEMESYS.Areas.Pages.Reports
{
    public class ReportPageModel : PageModel
    {
        [BindProperty]
        public Report CurrentReport { get; set; } 

        public void OnGet()
        {

            DateTime dateTime = DateTime.MaxValue; 

            // Initialization or loading of CurrentReport for display/editing
            CurrentReport = new Report
            {
                DateOfReport = dateTime,
                DateSpotted = DateTime.MaxValue,  
                TimeSpotted = DateTime.Now.ToString("HH:mm"),
                TypeOfHazard = "Unsafe Condition",
                Description = "Initial description",
                Status = "Closed", 
                ReporterEmail = "reporter@example.com"
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            

            return RedirectToPage("./Index"); 
        }
    }
}