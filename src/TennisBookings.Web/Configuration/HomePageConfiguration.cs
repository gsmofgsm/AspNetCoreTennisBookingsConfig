using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisBookings.Web.Configuration
{
    public class HomePageConfiguration
    {
        public bool EnableGreeting { get; set; }  // property names must exactly match key names in appsetting
        public bool EnableWeatherForecast { get; set; }
        [Required]
        public string ForecastSectionTitle { get; set; }
    }
}
