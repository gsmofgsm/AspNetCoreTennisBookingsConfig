using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Configuration
{
    public class HomePageConfigurationValidation : IValidateOptions<HomePageConfiguration>
    {
        private readonly bool _checkForProfanity;
        private readonly WeatherForecastingConfiguration _weatherConfig;
        private readonly IProfanityChecker _profanityChecker;

        public HomePageConfigurationValidation(
            IOptions<ContentConfiguration> contentConfig,
            IOptions<WeatherForecastingConfiguration> weatherConfig,
            IProfanityChecker profanityChecker)
        {
            _checkForProfanity = contentConfig.Value.CheckForProfanity;
            _weatherConfig = weatherConfig.Value;
            _profanityChecker = profanityChecker;
        }

        public ValidateOptionsResult Validate(string name, HomePageConfiguration options)
        {
            if (_weatherConfig.EnableWeatherForecast && options.EnableWeatherForecast
                && string.IsNullOrEmpty(options.ForecastSectionTitle))
            {
                return ValidateOptionsResult.Fail("A title is required, when forecast is enabled");
            }

            if (_checkForProfanity && _profanityChecker.ContainsProfanity(options.ForecastSectionTitle))
            {
                return ValidateOptionsResult.Fail("The title contains a blocked profanity word.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
