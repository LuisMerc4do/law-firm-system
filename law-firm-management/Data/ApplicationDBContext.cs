using law_firm_management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace law_firm_management.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<CaseModel> Cases { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de las entidades y sus relaciones
            builder.Entity<CaseModel>()
                .HasKey(c => c.CaseId);

            builder.Entity<CaseModel>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .IsRequired(false) // If CreatedById can be null
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CaseModel>()
                .HasOne(c => c.AssignedTo)
                .WithMany()
                .HasForeignKey(c => c.AssignedToId)
                .IsRequired(false) // If AssignedToId can be null
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocumentModel>()
                .HasKey(d => d.DocumentId);

            builder.Entity<DocumentModel>()
                .HasOne(d => d.Case)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MessageModel>()
                .HasKey(m => m.MessageId);

            builder.Entity<MessageModel>()
                .HasOne(m => m.Case)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MessageModel>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NotificationModel>()
                .HasKey(n => n.NotificationId);

            builder.Entity<NotificationModel>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sembrado de roles
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Lawyer",
                    NormalizedName = "LAWYER"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
