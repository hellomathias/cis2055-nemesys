using NEMESYS.Areas.Identity.Pages.Reports.Models;

public class Investigation
{
    public int InvestigationId { get; set; }
    public string Description { get; set; }
    public DateTime DateOfAction { get; set; }
    public string InvestigatorEmail { get; set; }
    public string InvestigatorPhone { get; set; }
    public int ReportId { get; set; }  // Foreign key linking to the Report
    public Report RelatedReport { get; set; }  // Navigation property
}
