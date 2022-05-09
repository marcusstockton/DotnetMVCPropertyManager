using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Portfolios;
using Website.Profiles;

namespace Website.Areas.Tests
{
    [TestClass()]
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new Mock<IPortfolioService>();
        private readonly Mock<IPropertyImageService> _propertyImageService = new Mock<IPropertyImageService>();
        private readonly Mock<IPropertyDocumentService> _propertyDocumentService = new Mock<IPropertyDocumentService>();
        private readonly IMapper _mapper;
        private readonly ILogger<PortfolioController> _logger;
        private ClaimsPrincipal _user;

        public PortfolioControllerTests()
        {
            var portfolioProfile = new PortfolioProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(portfolioProfile));
            _mapper = new Mapper(config);
            
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"), new Claim(ClaimTypes.Name, "TestUser1@test.com") }, "Owner"));
        }

        [TestMethod()]
        public async Task GetMyPortfolios_Returns_Only_My_Portfolios()
        {
            // Arrange
            var portfolioId = Guid.NewGuid();
            var results = new List<Portfolio> { new Portfolio { Id = portfolioId, Name = "Test", OwnerId = "1" } };

            _portfolioServiceMock.Setup(x => x.GetMyPortfolios(It.IsAny<string>())).ReturnsAsync(results);

            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper, _logger);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = _user }; // Mock a logged in user

            // Act
            var result = await controller.GetMyPortfolios();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<IList<PortfolioDetailsDto>>));
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<PortfolioDetailsDto>));
            var portfolioDetailsList = okResult.Value as List<PortfolioDetailsDto>;
            Assert.AreEqual(1, portfolioDetailsList.Count());
            var firstRecord = portfolioDetailsList.First();
            Assert.AreEqual(portfolioId, firstRecord.Id);
            Assert.AreEqual("Test", firstRecord.Name);
        }
    }
}