using dotNET_Project.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotNET_Project.Models;

namespace dotNET_Project.Data;

public class dotNET_ProjectContext : IdentityDbContext<dotNET_ProjectUser>
{
    public dotNET_ProjectContext(DbContextOptions<dotNET_ProjectContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    private class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<dotNET_ProjectUser> 
    { 
        public void Configure(EntityTypeBuilder<dotNET_ProjectUser> builder) 
        { 
            builder.Property(x => x.Nickname).HasMaxLength(255);
        } 
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<dotNET_ProjectUser> AllUsers { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
