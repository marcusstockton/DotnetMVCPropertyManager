using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Areas;
using Website.Interfaces;
using Website.Models.DTOs.Portfolios;

namespace Website.Tests.Controllers
{
    [TestClass]
    public class PortfolioControllerTests
    {
        private Mock<IPortfolioService> _portfolioServiceMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private FakeLogger<PortfolioController> _fakeLogger;
        private PortfolioController _controller;

        [TestInitialize]
        public void Setup()
        {
            _portfolioServiceMock = new Mock<IPortfolioService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _fakeLogger = new FakeLogger<PortfolioController>();
            _controller = new PortfolioController(_portfolioServiceMock.Object, _memoryCacheMock.Object, _fakeLogger);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public async Task GetMyPortfolios_ReturnsPortfolios()
        {
            // Arrange
            var portfolios = new List<PortfolioDetailsDto> { new PortfolioDetailsDto { Id = Guid.NewGuid(), Name = "Test Portfolio" } };
            var cacheEntryMock = new Mock<ICacheEntry>();
            _memoryCacheMock.Setup(mc => mc.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
            _portfolioServiceMock.Setup(s => s.GetMyPortfolios(It.IsAny<string>())).ReturnsAsync(portfolios);

            // Act
            var result = await _controller.GetMyPortfolios();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var response = result.Result as OkObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status200OK);

            var portfolioListResult = response.Value as List<PortfolioDetailsDto>;
            Assert.IsNotNull(portfolioListResult);
            Assert.AreEqual(1, portfolioListResult.Count);

            _portfolioServiceMock.Verify(s => s.GetMyPortfolios("test-user-id"), Times.Once);
            Assert.AreEqual(3, _fakeLogger.Collector.Count);
        }
    }
}