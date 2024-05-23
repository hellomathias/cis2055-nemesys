using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using NEMESYS.Models;

namespace NEMESYS.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
    }
}
