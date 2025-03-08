using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PropertyDocument> PropertyDocuments { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Portfolio>(b =>
            {
                b.HasKey(x => x.Id);
            });

            builder.Entity<Portfolio>(x =>
                x.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .IsRequired());

            builder.Entity<Property>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(b => b.Address)
                .WithOne(i => i.Property)
                .HasForeignKey<Property>(b => b.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(x => x.Images)
                .WithOne(x => x.Property)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(x => x.Documents)
                .WithOne(x => x.Property)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Address>().HasKey(x => x.Id);
            builder.Entity<Address>().Property(x => x.Longitude).HasColumnType("decimal(18, 9)");
            builder.Entity<Address>().Property(x => x.Latitude).HasColumnType("decimal(18, 9)");

            builder.Entity<PropertyDocument>().HasKey(x => x.Id);
            builder.Entity<PropertyImage>().HasKey(x => x.Id);
            builder.Entity<PropertyImage>().Property(x=>x.Description).HasMaxLength(200).IsRequired(false);
            builder.Entity<Tenant>().HasKey(x => x.Id);
            builder.Entity<Nationality>().HasKey(x => x.Id);
            builder.Entity<Note>().HasKey(x => x.Id);
            builder.Entity<DocumentType>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId);
            });

            builder.Entity<PropertyDocument>(b =>
            {
                b.HasOne(x => x.DocumentType)
                .WithMany()
                .HasForeignKey(x => x.DocumentTypeId);
            });

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is Base && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Added)
                {
                    ((Base)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is Base && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((Base)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}