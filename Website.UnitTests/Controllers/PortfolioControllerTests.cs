using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Portfolios;
using Website.Profiles;

namespace Website.Controllers.Tests
{
    [TestClass()]
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new Mock<IPortfolioService>();
        private readonly IMapper _mapper;

        private readonly Guid _portfolio1Id;
        private Portfolio _portfolio1;
        private ClaimsPrincipal _user;

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

            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"), new Claim(ClaimTypes.Name, "TestUser1@test.com") }, "Owner"));
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
        public async Task Can_Create_New_Portfolio()
        {
            var newPortfolio = new Portfolio
            {
                Name = "New Portfolio",
                OwnerId = "1",
            };

            _portfolioServiceMock.Setup(x => x.GetPortfolioById(_portfolio1Id)).ReturnsAsync(_portfolio1);
            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = _user }; // Mock a logged in user

            // Act
            var viewResult = await controller.Create(newPortfolio) as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            // Successfull if it returns to an index page.
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod()]
        public async Task CanEditPortfolioAsync()
        {
            var existingPortfolioId = Guid.NewGuid();
            var portfolio = new Portfolio
            {
                CreatedDate = DateTime.Now.AddDays(-23),
                Name = "Test To Update",
                OwnerId = "1",
                Id = existingPortfolioId
            };

            _portfolioServiceMock.Setup(x => x.UpdatePortfolio(portfolio)).ReturnsAsync(new Portfolio
            {
                CreatedDate = DateTime.Now.AddDays(-23),
                Name = "Updated Test",
                OwnerId = "1",
                Id = existingPortfolioId
            });

            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = _user }; // Mock a logged in user

            var viewResult = await controller.Edit(existingPortfolioId, portfolio) as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            // Successfull if it returns to an index page.
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod()]
        public async Task DeleteConfirmedTestAsync()
        {
            var existingPortfolioId = Guid.NewGuid();
            var portfolio = new Portfolio
            {
                CreatedDate = DateTime.Now.AddDays(-23),
                Name = "Test To Update",
                OwnerId = "1",
                Id = existingPortfolioId
            };
            _portfolioServiceMock.Setup(x => x.GetPortfolioById(existingPortfolioId)).ReturnsAsync(portfolio);
            _portfolioServiceMock.Setup(x => x.DeletePortfolio(portfolio)).ReturnsAsync(true);

            var controller = new PortfolioController(_portfolioServiceMock.Object, _mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = _user }; // Mock a logged in user

            var viewResult = await controller.DeleteConfirmed(existingPortfolioId) as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            // Successfull if it returns to an index page.
            Assert.AreEqual("Index", viewResult.ActionName);
        }
    }
}