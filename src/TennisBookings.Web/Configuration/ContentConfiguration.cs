using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisBookings.Web.Configuration
{
    public class ContentConfiguration : IContentConfiguration
    {
        public bool CheckForProfanity { get; set; }
    }
}
