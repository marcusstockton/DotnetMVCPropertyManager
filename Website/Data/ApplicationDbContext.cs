using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base( options )
        {
        }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyDocument> PropertyDocuments { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Property>()
                .HasOne( b => b.Address )
                .WithOne( i => i.Property )
                .HasForeignKey<Property>( b => b.AddressId ).IsRequired();

            base.OnModelCreating( builder );
        }
    }
}
