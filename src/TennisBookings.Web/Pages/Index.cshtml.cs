using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGreetingService _greetingService;
        private readonly IConfiguration _configuration;
        private readonly IWeatherForecaster _weatherForecaster;

        public IndexModel(IGreetingService greetingService, IConfiguration configuration, IWeatherForecaster weatherForecaster)
        {
            _greetingService = greetingService;
            _configuration = configuration;
            _weatherForecaster = weatherForecaster;
        }

        public string Greeting { get; private set; }
        public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; }

        public async Task OnGet()
        {
            var features = new Features();
            _configuration.Bind("Features:HomePage", features);

            if (features.EnableGreeting)
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            ShowWeatherForecast = features.EnableWeatherForecast
                && _weatherForecaster.ForecastEnabled; 
                    // this prop is hard coded in different implementations of IWeatherForecaster
                    // and Configure configures which implementation gets injected0

            if (ShowWeatherForecast)
            {
                //var title = homePageFeatures["ForecastSectionTitle"]; // by default, values from Configuration are of type string
                var title = features.ForecastSectionTitle;
                ForecastSectionTitle = string.IsNullOrEmpty(title) ? "How's the weather?" : title;

                var currentWeather = await _weatherForecaster.GetCurrentWeatherAsync();

                if (currentWeather != null)
                {
                    switch (currentWeather.Description)
                    {
                        case "Sun":
                            WeatherDescription = "It's sunny right now. A great day for tennis!";
                            break;

                        case "Cloud":
                            WeatherDescription = "It's cloudy at the moment and the outdoor courts are in use.";
                            break;

                        case "Rain":
                            WeatherDescription = "We're sorry but it's raining here. No outdoor courts in use.";
                            break;

                        case "Snow":
                            WeatherDescription = "It's snowing!! Outdoor courts will remain closed until the small";
                            break;
                    }
                }
            }
        }

        private class Features
        {
            public bool EnableGreeting { get; set; }  // property names must exactly match key names in appsetting
            public bool EnableWeatherForecast { get; set; }
            public string ForecastSectionTitle { get; set; }
        }
    }
}
