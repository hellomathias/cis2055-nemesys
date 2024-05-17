namespace NEMESYS.Areas.Identity.Pages.Reports.Models
{

    public class Report
    {
        public DateTime DateOfReport { get; set; }
        public string Location { get; set; }
        public DateTime DateSpotted { get; set; }
        public string TimeSpotted { get; set; } // Assuming TimeSpotted is a string representing time.
        public string TypeOfHazard { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Open"; // Default to "Open"
        public string ReporterEmail { get; set; }
        public string ReporterPhone { get; set; } // Optional
        public string OptionalPhotoPath { get; set; } // Path to the optional photo
        public int Upvotes { get; set; }
    }

}