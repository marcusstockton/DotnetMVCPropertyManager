namespace Website.Controllers.Tests
{
    [TestClass()]
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new Mock<IPortfolioService>();
        private readonly IMapper _mapper;

        private Guid _portfolio1Id;
        private Portfolio _portfolio1;

        public PortfolioControllerTests()
        {
            var portfolioProfile = new PortfolioProfile();
            var propertyProfile = new PropertyProfile();
            var addressProfile = new AddressProfile();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(portfolioProfile);
                cfg.AddProfile(propertyProfile);
                cfg.AddProfile(addressProfile);
            });
            _mapper = new Mapper(config);
        }

        [TestInitialize()]
        public void SetupData()
        {
            var _portfolio1Id = Guid.NewGuid();

            _portfolio1 = new Portfolio
            {
                Id = _portfolio1Id,
                Name = "Test 1",
                CreatedDate = DateTime.Now,
                OwnerId = "1",
                Properties = new List<Property> {
                    new Property {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Address = new Address{
                            Id = Guid.NewGuid(),
                            Line1 = "Line 1",
                            Line2 = "Line 2",
                            Line3 = "Line 3",
                            City = "Exeter",
                            CreatedDate = DateTime.Now,
                            Postcode = "EX11EX"
                        },
                        Description = "1 bed flat",
                        MonthlyRentAmount = 678,
                        PropertyValue = 134000,
                        PurchaseDate = new DateTime(2018, 2, 3) }
                }
            };
        }

        [TestMethod()]
        public async Task DetailsTest()
        {
            // Arrange
            _portfolioServiceMock.Setup(x => x.GetPortfolioById(_portfolio1Id)).ReturnsAsync(_portfolio1);
            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper);

            // Act
            var viewResult = await controller.Details(_portfolio1Id) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(viewResult.Model, typeof(PortfolioDetailsDto));
            var model = viewResult.Model as PortfolioDetailsDto;
            Assert.AreEqual("Test 1", model.Name);
            Assert.AreEqual(model.Properties.Count, model.NumberOfProperties);
        }

        [TestMethod()]
        public async Task CreateTest1()
        {
            var newPortfolio = new Portfolio
            {
                Name = "New Portfolio",
                OwnerId = "1",
            };

            _portfolioServiceMock.Setup(x => x.GetPortfolioById(_portfolio1Id)).ReturnsAsync(_portfolio1);
            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper);

            // Act
            var viewResult = await controller.Create(newPortfolio) as ViewResult;
            // Assert stuff..
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }
    }
}