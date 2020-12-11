namespace Website.UnitTests
{
    [TestClass]
    public class PortfolioControllerUnitTests
    {
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new Mock<IPortfolioService>();
        private readonly Mock<IPropertyImageService> _propertyImageService = new Mock<IPropertyImageService>();
        private readonly Mock<IPropertyDocumentService> _propertyDocumentService = new Mock<IPropertyDocumentService>();
        private readonly Mock<IPortfolioService> _portfolioService = new Mock<IPortfolioService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        [TestMethod]
        public void TestMethod1()
        {
            Assert.Fail();
        }
    }
}