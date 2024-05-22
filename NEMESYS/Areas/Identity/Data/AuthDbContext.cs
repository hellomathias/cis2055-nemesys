using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NEMESYS.Areas.Identity.Data;
using System.Reflection.Emit;

namespace NEMESYS.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {

    }
    public DbSet<Areas.Identity.Pages.Reports.Models.Report> Reports { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Areas.Identity.Pages.Reports.Models.Report>(entity =>
        {
            entity.Property(e => e.Description).IsRequired();
        });
    }
}
