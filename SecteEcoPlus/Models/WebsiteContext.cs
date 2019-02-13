 using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Models
{
    public class WebsiteContext : IdentityDbContext<SecteUser>
    {
        public WebsiteContext(DbContextOptions<WebsiteContext> options)
            : base(options)
        {
            
        }
        public DbSet<Message> WebsiteReviews { get; set; }
        public DbSet<PublicProfile> PublicProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PublicProfile>()
                .HasMany(u => u.ReviewMessages)
                .WithOne(m => m.Author)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<SecteUser>()
                .HasOne( p=> p.PublicProfile)
                .WithOne(p => p.SecteUser)
                .HasForeignKey<SecteUser>(u => u.PublicProfileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
