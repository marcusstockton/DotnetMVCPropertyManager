using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task SeedData(bool reseedDatabase = false)
        {
            try
            {
                if (reseedDatabase)
                {
                    _logger.LogInformation("Blatting the database and reseeding...");
                    _context.Database.EnsureDeleted();
                    _context.Database.Migrate();
                    // Delete Images....leaving examples
                }

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
                    }, new DocumentType
                    {
                        Description = "Electrical Inspection Documentation",
                        CreatedDate = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                }
                if (!_context.Portfolios.Any())
                {
                    _logger.LogInformation("Creating portfolio and related data");
                    await _context.AddRangeAsync(
                        new Portfolio
                        {
                            Owner = await _userManager.FindByEmailAsync("beckystockton84@hotmail.co.uk"),
                            Name = "South West",
                            CreatedDate = DateTime.Now,
                            Properties = new List<Property> {
                            new Property
                            {
                                PropertyValue = 120400,
                                PurchaseDate = new DateTime(2017, 05, 21),
                                CreatedDate = DateTime.Now,
                                Description = "A well presented 1 bed apartment with views",
                                NoOfRooms = 1,
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
                                        TenantImage = @"\TenantImages\Example\Male1\download.jpg",
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
                                        TenantImage = @"\TenantImages\Example\Male2\index.jpg",
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
                                    Latitude = 50.72704m,
                                    Longitude = -3.49282m,
                                    CreatedDate = DateTime.Now
                                },
                                Documents = new List<PropertyDocument>
                                {
                                    new PropertyDocument{ CreatedDate = DateTime.Now, DocumentType = _context.DocumentTypes.First(x=>x.Description == "Tenancy Agreement"), FileName = "Tracked_Returns_label_NT350067989GB.pdf", FilePath = @"\PropertyDocuments\Example\Tracked_Returns_label_NT350067989GB.pdf", FileType = ".pdf" }
                                },
                                Images = new List<PropertyImage>{
                                    new PropertyImage{CreatedDate = DateTime.Now, FileName = "assets.simpleview-europe.com.jpg", FilePath = @"\PropertyImages\Example\Property1\assets.simpleview-europe.com.jpg", FileType = ".jpg"},
                                    new PropertyImage{CreatedDate = DateTime.Now, FileName = "26C69756-012B-4C50-B431-88F5CE279CD2.jpeg", FilePath = @"\PropertyImages\Example\Property1\26C69756-012B-4C50-B431-88F5CE279CD2.jpeg", FileType = ".jpeg"},
                                    new PropertyImage{CreatedDate = DateTime.Now, FileName = "1684d6d66862064eb404b0a7a70d7283.jpg", FilePath = @"\PropertyImages\Example\Property1\1684d6d66862064eb404b0a7a70d7283.jpg", FileType = ".jpeg"}
                                }
                            }, new Property
                            {
                                PropertyValue = 189000,
                                CreatedDate = DateTime.Now,
                                Address = new Address
                                {
                                    City = "Plymouth",
                                    Postcode = "PL34 2RT",
                                    CreatedDate = DateTime.Now,
                                    Line1 = "12",
                                    Line2 = "New Lane",
                                    Town = "Bodmin",
                                    Latitude = 50.476529600000006M,
                                    Longitude = -4.702104499999998M
                                },
                                Description = "Lovely 2 bed property",
                                MonthlyRentAmount = 790,
                                NoOfRooms = 2,
                                PurchaseDate = DateTime.Now.AddDays(-38),
                                Tenants = new List<Tenant>
                                {
                                    new Tenant
                                    {
                                        CreatedDate = DateTime.Now,
                                        FirstName = "Sally",
                                        LastName = "McBride",
                                        JobTitle = "HR",
                                        Nationality = "Brazillian",
                                        Notes = new List<Note>
                                        {
                                            new Note{CreatedDate = DateTime.Now, Description = "Initial meeting she seems lovely"}
                                        },
                                        PhoneNumber = "01928374572",
                                        TenancyStartDate = new DateTime(2019, 3, 5),
                                        TenantImage = @"\TenantImages\Example\Female1\b0d7ebf6d72cc032ad123e3de1a2e8ca.jpg",
                                    }
                                }
                            }
                        }
                        }, new Portfolio
                        {
                            CreatedDate = DateTime.Now,
                            Name = "North West",
                            Owner = await _userManager.FindByEmailAsync("beckystockton84@hotmail.co.uk"),
                            Properties = new List<Property>
                            {
                                new Property
                                {
                                    Address = new Address
                                    {
                                        City = "Liverpool",
                                        Line1 = "12",
                                        Line2 = "The Gables",
                                        Line3 = "Third Line",
                                        Latitude = 53.41009M,
                                        Longitude = -2.97843M,
                                        Postcode = "L1 4RT",
                                    },
                                    CreatedDate = DateTime.Now,
                                    Description = "Lovely 3 bed house",
                                    NoOfRooms = 3,
                                    MonthlyRentAmount = 1200,
                                    PropertyValue = 210000,
                                    PurchaseDate = new DateTime(2014, 4, 29),
                                    Images = new List<PropertyImage>
                                    {
                                        new PropertyImage{CreatedDate = DateTime.Now, FileName = "download (1).jpg", FilePath = @"\PropertyImages\Example\Property2\download (1).jpg", FileType = ".jpg"},
                                        new PropertyImage{CreatedDate = DateTime.Now, FileName = "Residential-property-2.jpg", FilePath = @"\PropertyImages\Example\Property2\Residential-property-2.jpg", FileType = ".jpg"},
                                    },
                                    Documents = new List<PropertyDocument>
                                    {
                                        new PropertyDocument{ CreatedDate = DateTime.Now, DocumentType = _context.DocumentTypes.First(x=>x.Description == "Gas Safety Certificate"), FileName = "Label-QR-Code-446103803.pdf", FilePath = @"\PropertyDocuments\Example\Label-QR-Code-446103803.pdf", FileType = ".pdf" }
                                    },
                                    Tenants = new List<Tenant>
                                    {
                                        new Tenant
                                        {
                                            CreatedDate = DateTime.Now,
                                            FirstName = "Jane",
                                            LastName = "Eyre",
                                            JobTitle = "Author",
                                            Nationality = "English",
                                            Notes = new List<Note>
                                            {
                                                new Note{CreatedDate = DateTime.Now, Description = "Bookish"}
                                            },
                                            PhoneNumber = "07748975421",
                                            TenancyStartDate = new DateTime(2020, 11, 15),
                                            TenantImage = @"\TenantImages\Example\Female2\images.jpg",
                                        }
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