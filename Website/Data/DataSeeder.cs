using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DataSeeder> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SeedData()
        {
            try
            {
                if (!_context.Users.Any())
                {
                    _logger.LogInformation("Creating roles");
                    // Create some roles:
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Owner" });
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

                    _logger.LogInformation("Creating users");
                    var user1 = new ApplicationUser { FirstName = "Marcus", LastName = "Stockton", Email = "marcus_stockton@hotmail.co.uk", UserName = "marcus_stockton@hotmail.co.uk", };
                    var user2 = new ApplicationUser { FirstName = "Becky", LastName = "Stockton", Email = "beckystockton84@hotmail.co.uk", UserName = "beckystockton84@hotmail.co.uk", };
                    await _userManager.CreateAsync(user1, "P@55w0rd1");
                    await _userManager.CreateAsync(user2, "P@55w0rd1");

                    _logger.LogInformation("Adding users to roles");
                    await _userManager.AddToRoleAsync(user1, "Admin");
                    await _userManager.AddToRoleAsync(user2, "Owner");
                    await _context.SaveChangesAsync();
                }
                if (!_context.DocumentTypes.Any())
                {
                    await _context.DocumentTypes.AddRangeAsync(new DocumentType
                    {
                        Description = "EPC",
                        CreatedDate = DateTime.Now,
                    }, new DocumentType
                    {
                        Description = "Gas Safety Certificate",
                        CreatedDate = DateTime.Now,
                    }, new DocumentType
                    {
                        Description = "Tenancy Agreement",
                        CreatedDate = DateTime.Now,
                    }, new DocumentType
                    {
                        Description = "Deposit Protection Scheme",
                        CreatedDate = DateTime.Now,
                    });
                    await _context.SaveChangesAsync();
                }
                if (!_context.Portfolios.Any())
                {
                    _logger.LogInformation("Creating portfolio and related data");
                    await _context.AddRangeAsync(new Portfolio
                    {
                        Owner = await _userManager.FindByEmailAsync("beckystockton84@hotmail.co.uk"),
                        Name = "Exeter",
                        CreatedDate = DateTime.Now,
                        Properties = new List<Property> {
                        new Property
                        {
                            PropertyValue = 120400,
                            PurchaseDate = new DateTime(2017, 05, 21),
                            CreatedDate = DateTime.Now,
                            Tenants = new List<Tenant>
                            {
                                new Tenant
                                {
                                    FirstName = "Dave",
                                    LastName = "Davidson",
                                    JobTitle = "Art Critic",
                                    PhoneNumber = "074625174385",
                                    Nationality = "British",
                                    TenancyStartDate = new DateTime(2019, 11, 23),
                                    TenantImage = @"\TenantImages\c9e768aa-9b8c-4e92-9b19-55282d1d2d04\dark-angels-codex-500x750.jpg",
                                    Notes = new List<Note>{ new Note {Description = "An overall good tenant. Keeps himself to himself, and looks after the property" } }
                                },
                                new Tenant
                                {
                                    FirstName = "Tony",
                                    LastName = "Montana",
                                    JobTitle = "Professional Cleaner",
                                    Nationality = "Cuban",
                                    PhoneNumber = "0192838456",
                                    TenancyStartDate = new DateTime(2018, 7, 17),
                                    TenancyEndDate = new DateTime(2019, 11, 12),
                                    TenantImage = @"\TenantImages\f421ff8c-019c-458c-719b-08d8493633d7\download.jpg",
                                    Notes = new List<Note>
                                    {
                                        new Note{Description = "Definitley not a professional cleaner."}
                                    }
                                }
                            },
                            Address = new Address
                            {
                                Line1 = "18B",
                                Line2 = "Wayside Crescent",
                                City = "Exeter",
                                Postcode = "EX2 2EX",
                                CreatedDate = DateTime.Now
                            },
                            Images = new List<PropertyImage>{
                                new PropertyImage{CreatedDate = DateTime.Now, FileName = "Picture 177.jpg", FilePath = "\\PropertyImages\\d7306acd-b089-45af-b4b7-f0dfb1126441\\Picture 177.jpg", FileType = ".jpg"}
                            }
                        }
                    }
                    });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}