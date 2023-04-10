using CityAir.Infrastructure.API;
using CityAir.Infrastructure.Models;
using CityAir.UI.Features.CityAirQuality;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;

namespace CityAir.UI.UnitTests.Features.CityAirQuality
{
    [TestFixture]
    public class CityAirViewModelFactoryTests
    {
        private Mock<IOpenAQApi> _openAQApiMock;
        private Mock<ILogger<CityAirViewModelFactory>> _loggerMock;

        private CityAirViewModelFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _openAQApiMock = new Mock<IOpenAQApi>();
            _loggerMock = new Mock<ILogger<CityAirViewModelFactory>>();

            _sut = new CityAirViewModelFactory(_openAQApiMock.Object, GetMemoryCache(It.IsAny<object>(), false), _loggerMock.Object);
        }

        [Test]
        public void Constructor_NullOpenAQApi_ThrowsArgumentNullException()
        {

            IOpenAQApi openAQApi = null;
            IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            ILogger<CityAirViewModelFactory> logger = new Mock<ILogger<CityAirViewModelFactory>>().Object;


            Assert.Throws<ArgumentNullException>(() => new CityAirViewModelFactory(openAQApi, memoryCache, logger));
        }

        [Test]
        public void Constructor_NullMemoryCache_ThrowsArgumentNullException()
        {

            IOpenAQApi openAQApi = new Mock<IOpenAQApi>().Object;
            IMemoryCache memoryCache = null;
            ILogger<CityAirViewModelFactory> logger = new Mock<ILogger<CityAirViewModelFactory>>().Object;


            Assert.Throws<ArgumentNullException>(() => new CityAirViewModelFactory(openAQApi, memoryCache, logger));
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {

            IOpenAQApi openAQApi = new Mock<IOpenAQApi>().Object;
            IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            ILogger<CityAirViewModelFactory> logger = null;


            Assert.Throws<ArgumentNullException>(() => new CityAirViewModelFactory(openAQApi, memoryCache, logger));
        }

        [Test]
        public async Task Create_ShouldReturnCachedResponse_WhenCurrentUrlIsCached()
        {

            var queryParam = new GetCityAirQueryParam { Country = "US", Limit = 10 };
            var cachedResponse = new GetCityAirViewModel { QueryParam = queryParam };
            _sut = new CityAirViewModelFactory(_openAQApiMock.Object, GetMemoryCache(cachedResponse, true), _loggerMock.Object);

            var result = await _sut.Create(queryParam);

            Assert.NotNull(result);
            Assert.AreEqual(cachedResponse, result);
        }


        [Test]
        public async Task Create_ShouldReturnAPIResponse_WhenCurrentUrlIsNotCached()
        {

            var queryParam = new GetCityAirQueryParam();
            var expectedResults = new List<GetCityResult>()
            {
                new GetCityResult
                {
                    City = "ML"
                }
            };
            var expected = new GetCityAirViewModel
            {
                QueryParam = queryParam,
                Items = expectedResults
            };

            _openAQApiMock.Setup(x => x.GetCities(It.IsAny<QueryParam>())).ReturnsAsync(new GetCityResponse { Results = expectedResults });

            var result = await _sut.Create(queryParam);


            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(expectedResults[0].City, result.Items[0].City);
        }

        [Test]
        public async Task Create_ThrowsException_ReturnsResultWithEmptyResultsList()
        {

            var queryParam = new GetCityAirQueryParam();
            var expected = new GetCityAirViewModel
            {
                QueryParam = queryParam,
                Items = new List<GetCityResult>()
            };

            var exception = await ApiException.Create(new HttpRequestMessage(), HttpMethod.Get, new HttpResponseMessage(), new RefitSettings(), null);
            _openAQApiMock.Setup(api => api.GetCities(queryParam)).ThrowsAsync(exception);

            var result = await _sut.Create(queryParam);

            Assert.That(result.QueryParam, Is.EqualTo(expected.QueryParam));
            Assert.That(result.Items, Is.EqualTo(expected.Items));
            _loggerMock.Verify(x =>
                    x.Log(
                      LogLevel.Error,
                      It.IsAny<EventId>(),
                      It.Is<It.IsAnyType>((v, t) => true),
                      It.IsAny<Exception>(),
                      It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        private static IMemoryCache GetMemoryCache(object expectedValue, bool @return)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(@return);

            var cachEntry = Mock.Of<ICacheEntry>();

            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            return mockMemoryCache.Object;
        }

    }
}