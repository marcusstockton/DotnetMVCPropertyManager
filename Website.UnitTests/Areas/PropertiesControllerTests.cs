using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Areas;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Properties;

namespace Website.UnitTests.Areas
{
    [TestClass]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _propertyServiceMock;
        private FakeLogger<PropertiesController> _fakeLogger;
        private Mock<IMapper> _mapperMock;
        private PropertiesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _propertyServiceMock = new Mock<IPropertyService>();
            _mapperMock = new Mock<IMapper>();
            _fakeLogger = new FakeLogger<PropertiesController>();
            _controller = new PropertiesController(_propertyServiceMock.Object, _fakeLogger, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetPropertiesForPortfolio_ReturnsProperties()
        {
            // Arrange
            var portfolioId = Guid.NewGuid();
            var properties = new List<Property> { new Property { Id = Guid.NewGuid(), Description = "Test Property" } };
            var propertiesDto = new List<PropertyListDTO> { new PropertyListDTO { Id = Guid.NewGuid(), Description = "Test Property" } };

            _propertyServiceMock.Setup(s => s.GetPropertiesForPortfolio(portfolioId)).ReturnsAsync(properties);
            _mapperMock.Setup(m => m.Map<List<PropertyListDTO>>(properties)).Returns(propertiesDto);

            // Act
            var result = await _controller.GetPropertiesForPortfolio(portfolioId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            var response = result.Result as OkObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status200OK);

            var portfolioListResult = response.Value as List<PropertyListDTO>;
            Assert.AreEqual(1, portfolioListResult.Count);
            _propertyServiceMock.Verify(s => s.GetPropertiesForPortfolio(portfolioId), Times.Once);
            //_loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(2, _fakeLogger.Collector.Count);
        }
    }
}
