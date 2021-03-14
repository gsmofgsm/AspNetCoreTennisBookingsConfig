using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using TennisBookings.Web.Configuration;
using TennisBookings.Web.Core.Caching;
using TennisBookings.Web.Domain;
using TennisBookings.Web.Services;
using Xunit;

namespace TennisBookings.Web.Tests
{
    public class CachedWeatherForecasterTests
    {
        [Fact]
        public async Task GetCurrentWeatherAsync_CachesForCorrectPeriodOfTime()
        {
            const int expectedMInsToCache = 101;
            int minsToCache = -1;
            var forecasterMock = new Mock<IWeatherForecaster>();
            forecasterMock.Setup(x => x.GetCurrentWeatherAsync())
                .ReturnsAsync(new Domain.CurrentWeatherResult { Description = "This is a weather description" });

            var cacheMock = new Mock<IDistributedCache<CurrentWeatherResult>>();
            cacheMock.Setup(x => x.TryGetValueAsync(It.IsAny<string>()))
                .ReturnsAsync((false, (CurrentWeatherResult)null));
            cacheMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CurrentWeatherResult>(), It.IsAny<int>()))
                .Callback<string, CurrentWeatherResult, int>((key, result, mins) => minsToCache = mins);

            var optionsMock = new Mock<IOptionsMonitor<ExternalServicesConfig>>();
            optionsMock.Setup(x => x.Get(ExternalServicesConfig.WeatherApi))
                .Returns(new ExternalServicesConfig { MinsToCache = expectedMInsToCache });

            var sut = new CachedWeatherForecaster(forecasterMock.Object, cacheMock.Object, optionsMock.Object);

            _ = await sut.GetCurrentWeatherAsync();

            cacheMock.Verify(x => x.SetAsync(
                It.IsAny<string>(), It.IsAny<CurrentWeatherResult>(), It.IsAny<int>()), Times.Once);

            Assert.Equal(expectedMInsToCache, minsToCache);
        }

        [Fact]
        public async Task GetCurrentWeatherAsync_CashesForCorrectPeriodOfTime_UsingStubsAndServiceProvider()
        {
            const int expectedMinsTocache = 101;
            var stubCache = new StubDistributedCache();

            var options = new ServiceCollection()
                .Configure<ExternalServicesConfig>(ExternalServicesConfig.WeatherApi, opt =>
                    opt.MinsToCache = expectedMinsTocache)
                .BuildServiceProvider()
                .GetRequiredService<IOptionsMonitor<ExternalServicesConfig>>();

            var sut = new CachedWeatherForecaster(new StubWeatherForecaster(), stubCache, options);

            _ = await sut.GetCurrentWeatherAsync();

            Assert.True(stubCache.ItemCached);
            Assert.Equal(expectedMinsTocache, stubCache.CachedForMins);
        }

        private class StubDistributedCache : IDistributedCache<CurrentWeatherResult>
        {
            public bool ItemCached { get; private set; }
            public int CachedForMins { get; private set; }

            public Task<CurrentWeatherResult> GetAsync(string key)
            {
                throw new NotImplementedException();
            }

            public Task RemoveAsync(string key)
            {
                throw new NotImplementedException();
            }

            public Task SetAsync(string key, CurrentWeatherResult item, int minutesToCache)
            {
                ItemCached = true;
                CachedForMins = minutesToCache;
                return Task.CompletedTask;
            }

            public Task<(bool Found, CurrentWeatherResult Value)> TryGetValueAsync(string key) =>
                Task.FromResult((false, (CurrentWeatherResult)null));
        }

        private class StubWeatherForecaster : IWeatherForecaster
        {
            public bool ForecastEnabled => true;

            public Task<CurrentWeatherResult> GetCurrentWeatherAsync() =>
                Task.FromResult(new CurrentWeatherResult { Description = "This is a weather description" });
        }
    }
}
