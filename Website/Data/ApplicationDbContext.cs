using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Portfolio>().HasKey(x => x.Id);
            builder.Entity<Property>().HasKey(x => x.Id);
            builder.Entity<Address>().HasKey(x => x.Id);
            builder.Entity<PropertyDocument>().HasKey(x => x.Id);
            builder.Entity<PropertyImage>().HasKey(x => x.Id);
            builder.Entity<Tenant>().HasKey(x => x.Id);
            builder.Entity<Note>().HasKey(x => x.Id);
            builder.Entity<DocumentType>().HasKey(x => x.Id);

            builder.Entity<Property>()
                .HasOne(b => b.Address)
                .WithOne(i => i.Property)
                .HasForeignKey<Property>(b => b.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Property>().Property(x => x.AddressId).ValueGeneratedOnAdd();

            builder.Entity<Property>()
                .HasMany(x => x.Images)
                .WithOne(x => x.Property)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Property>()
                .HasMany(x => x.Documents)
                .WithOne(x => x.Property)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}