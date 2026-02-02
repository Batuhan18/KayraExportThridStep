using KayraExportThridStep.Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KayraExportThirdStep.Auth.Infrastructure.Data
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tablo isimlerini açıkça belirt
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("AspNetRoles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AspNetUserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AspNetUserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AspNetUserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AspNetUserTokens");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AspNetRoleClaims");
            });
        }
    }
}