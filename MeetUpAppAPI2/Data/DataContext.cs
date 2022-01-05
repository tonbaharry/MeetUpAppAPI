using MeetUpAppAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace MeetUpAppAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> Users {get;set;}
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLike>()
                .HasKey(x => new { x.SourceUserId, x.LikedUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(t => t.LikedUsers)
                .HasForeignKey(w => w.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(t => t.LikedByUsers)
                .HasForeignKey(w => w.LikedUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}