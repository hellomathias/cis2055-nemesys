using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NEMESYS.Data;
using System;
using System.ComponentModel.DataAnnotations;
using NEMESYS.Areas.Identity.Pages.Reports.Models;


namespace NEMESYS.Areas.Identity.Pages.Reports
{
    [Authorize]
    public class ReportPageModel : PageModel
    {
        private readonly AuthDbContext _context;

        public ReportPageModel(AuthDbContext context)
        {
            _context = context;
        }
        public Report CurrentReport { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CurrentReport.ReportId == 0)
            {
                // Assuming ReportId is your primary key and it's set to 0 for new entries
                _context.Reports.Add(CurrentReport);
            }
            else
            {
                _context.Reports.Update(CurrentReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index"); // or your redirect page after submission
        }

        public class Report
        {
            [Required]
            [StringLength(100, ErrorMessage = "Title is too long.")]
            public string Title { get; set; }

            [Required]
            [StringLength(1000, ErrorMessage = "Description is too long.")]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateOfReport { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Location is too long.")]
            public string Location { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateSpotted { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "Time is too long.")]
            public string TimeSpotted { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "Type of hazard is too long.")]
            public string TypeOfHazard { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "Status is too long.")]
            public string Status { get; set; }

            [Required]
            [EmailAddress]
            public string ReporterEmail { get; set; }

            [Phone]
            public string ReporterPhone { get; set; }

            public string OptionalPhotoPath { get; set; }
            public int ReportId { get; internal set; }
        }

        public void OnGet()
        {

            // Initialize CurrentReport with a default instance
            CurrentReport = new Report
            {
                Title = "Sample Report",
                Description = "Sample description",
                DateOfReport = DateTime.Now,
                Location = "Sample Location",
                DateSpotted = DateTime.Now,
                TimeSpotted = "12:00 PM",
                TypeOfHazard = "Sample Hazard",
                Status = "Open",
                ReporterEmail = "sample@example.com",
                ReporterPhone = "123-456-7890",
                OptionalPhotoPath = null
            };
        }
    }
}
