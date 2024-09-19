using Microsoft.EntityFrameworkCore;

namespace WebApplicationDemo.Model
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {
        }
        public DbSet<UserDataModel> userData { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDataModel>().ToTable("tUser");

            modelBuilder.Entity<UserDataModel>()
                .HasKey(u => u.username);

            modelBuilder.Entity<UserDataModel>()
                .Property(u => u.username)
                .HasColumnName("username")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<UserDataModel>()
                .Property(u => u.password)
                .HasColumnName("password")
                .HasMaxLength(400)
                .IsRequired();
        }
    }
}
