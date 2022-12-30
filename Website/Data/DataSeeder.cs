using Bogus;
using CountryData.Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                    var user2 = new ApplicationUser { FirstName = "Dave", LastName = "Stockton", Email = "davestockton84@hotmail.co.uk", UserName = "davestockton84@hotmail.co.uk", };
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

                if (!_context.Nationalities.Any())
                {
                    _logger.LogInformation("Creating Nationalities data");

                    var nationalityList = new List<string>() { "Afghan", "Albanian", "Algerian", "American", "Andorran", "Angolan", "Antiguans", "Argentinean", "Armenian", "Australian", "Austrian", "Azerbaijani", "Bahamian", "Bahraini", "Bangladeshi", "Barbadian", "Barbudans", "Batswana", "Belarusian", "Belgian", "Belizean", "Beninese", "Bhutanee", "Bolivian", "Bosnian", "Brazilian", "British", "Bruneian", "Bulgarian", "Burkinabe", "Burmese", "Burundian", "Cambodian", "Cameroonian", "Canadian", "Cape Verdean", "Central African", "Chadian", "Chilean", "Chinese", "Colombian", "Comoran", "Congolese", "Costa Rican", "Croatian", "Cuban", "Cypriot", "Czech", "Danish", "Djibouti", "Dominican", "Dutch", "East Timorese", "Ecuadorean", "Egyptian", "Emirian", "Equatorial Guinean", "Eritrean", "Estonian", "Ethiopian", "Fijian", "Filipino", "Finnish", "French", "Gabonese", "Gambian", "Georgian", "German", "Ghanaian", "Greek", "Grenadian", "Guatemalan", "Guinea-Bissauan", "Guinean", "Guyanese", "Haitian", "Herzegovinian", "Honduran", "Hungarian", "I-Kiriati", "Icelander", "Indian", "Indonesian", "Iranian", "Iraqi", "Irish", "Israeli", "Italian", "Ivorian", "Jamaican", "Japanese", "Jordanian", "Kazakhstani", "Kenyan", "Kittian and Nevisian", "Kuwaiti", "Kyrgyz", "Laotian", "Latvian", "Lebanese", "Liberian", "Libyan", "Liechtensteiner", "Lithuanian", "Luxembourger", "Macedonian", "Malagasy", "Malawian", "Malaysian", "Maldivian", "Malian", "Maltese", "Marshallese", "Mauritanian", "Mauritian", "Mexican", "Microneian", "Moldovan", "Monacan", "Mongolian", "Moroccan", "Mosotho", "Motswana", "Mozambican", "Namibian", "Nauruan", "Nepalese", "New Zealander", "Ni-Vanuatu", "Nicaraguan", "Nigerian", "Nigerien", "North Korean", "Northern Irish", "Norwegian", "Omani", "Pakistani", "Palauan", "Panamanian", "Papua New Guinean", "Paraguayan", "Peruvian", "Polish", "Portuguese", "Qatari", "Romanian", "Russian", "Rwandan", "Saint Lucian", "Salvadoran", "Samoan", "San Marinese", "Sao Tomean", "Saudi", "Scottish", "Senegalese", "Serbian", "Seychellois", "Sierra Leonean", "Singaporean", "Slovakian", "Slovenian", "Solomon Islander", "Somali", "South African", "South Korean", "Spanish", "Sri Lankan", "Sudanese", "Surinamer", "Swazi", "Swedish", "Swiss", "Syrian", "Taiwanese", "Tajik", "Tanzanian", "Thai", "Togolese", "Tongan", "Trinidadian or Tobagonian", "Tunisian", "Turkish", "Tuvaluan", "Ugandan", "Ukrainian", "Uruguayan", "Uzbekistani", "Venezuelan", "Vietnamese", "Welsh", "Yemenite", "Zambian", "Zimbabwean" };
                    foreach (var nationality in nationalityList)
                    {
                        await _context.Nationalities.AddAsync( new Nationality { Name = nationality });
                    }
                    await _context.SaveChangesAsync();
                
                }

                if (!_context.Portfolios.Any())
                {
                    _logger.LogInformation("Creating portfolio and related data");
                    await _context.AddRangeAsync(
                        new Portfolio
                        {
                            Owner = await _userManager.FindByEmailAsync("davestockton84@hotmail.co.uk"),
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
                                        Nationality = _context.Nationalities.First(x=>x.Name == "British"),
                                        TenancyStartDate = new DateTime(2019, 11, 23),
                                        TenantImage = @"\TenantImages\Example\Male1\download.jpg",
                                        Notes = new List<Note>{ new Note {Description = "An overall good tenant. Keeps himself to himself, and looks after the property" } }
                                    },
                                    new Tenant
                                    {
                                        FirstName = "Tony",
                                        LastName = "Montana",
                                        JobTitle = "Professional Cleaner",
                                        Nationality = _context.Nationalities.First(x=>x.Name ==  "Cuban"),
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
                                    Latitude = 50.72704,
                                    Longitude = -3.49282,
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
                                    Latitude = 50.476529600000006,
                                    Longitude = -4.702104499999998
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
                                        Nationality = _context.Nationalities.First(x=>x.Name == "Brazilian"),
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
                            Owner = await _userManager.FindByEmailAsync("davestockton84@hotmail.co.uk"),
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
                                        Latitude = 53.41009,
                                        Longitude = -2.97843,
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
                                            Nationality = _context.Nationalities.First(x=>x.Name == "British"),
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


                    // Bogus this stuff up!:
                    _logger.LogInformation("Going ballz to the wall and generating random data with bogus!!");
                    var tenantFaker = new Faker<Tenant>("en_GB")
                        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                        .RuleFor(x => x.LastName, f => f.Person.LastName)
                        .RuleFor(x => x.JobTitle, f => f.Name.JobTitle())
                        .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
                        .RuleFor(x => x.TenantImage, f => f.Person.Avatar)
                        .RuleFor(x => x.Nationality, f => f.PickRandom(_context.Nationalities.ToList()))
                        .RuleFor(x => x.TenancyStartDate, f => f.Date.Past())
                        .RuleFor(x => x.TenancyEndDate, (f, u) => f.Date.BetweenOffset(u.TenancyStartDate, DateTime.Now).OrNull(f, .8f));

                    var propertyImageFaker = new Faker<PropertyImage>("en_GB")
                        .RuleFor(x => x.FilePath, f=>f.Image.LoremFlickrUrl(800, 600, "building,home"))
                        .RuleFor(x => x.FileName, f => f.System.FileName())
                        .RuleFor(x=>x.CreatedDate, f=>f.Date.Recent(100));

                    var propertyFaker = new Faker<Property>("en_GB")
                        .RuleFor(x=>x.NoOfRooms, f => f.Random.Number(1,4))
                        .RuleFor(x=>x.Address, f=> new Address {
                            Line1 = f.Address.BuildingNumber(),
                            Line2 = f.Address.StreetAddress(),
                            Town = f.Country().UnitedKingdom().Place().Name,
                            City = f.Address.City(),
                            Latitude = f.Address.Latitude(),
                            Longitude = f.Address.Longitude(),
                            Postcode = f.Country().UnitedKingdom().PostCode(),
                        })
                        .RuleFor(x=>x.Description, f=>f.Lorem.Paragraph())
                        .RuleFor(x=>x.Images, f => propertyImageFaker.Generate(f.Random.Number(2,5)))
                        .RuleFor(x=>x.MonthlyRentAmount, f=>f.Random.Number(500,4000))
                        .RuleFor(x=>x.PurchaseDate, f=>f.Date.Past())
                        .RuleFor(x=>x.Tenants, f=>tenantFaker.Generate(f.Random.Number(3,8)))
                        .RuleFor(x=>x.PropertyValue, f=>f.Random.Double(100000, 1000000));

                    var PortFolioFaker = new Faker<Portfolio>("en_GB")
                     .RuleFor(o => o.Name, f => f.Address.County())
                     .RuleFor(o => o.Owner, f => f.PickRandom(_context.Users.ToArray()))
                     .RuleFor(x => x.Properties, f=> propertyFaker.Generate(f.Random.Number(4,12)))
                     .RuleFor(o => o.CreatedDate, f => f.Date.Past());

                    var bogusPortilios = PortFolioFaker.Generate(10);
                    await _context.Portfolios.AddRangeAsync(bogusPortilios);
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