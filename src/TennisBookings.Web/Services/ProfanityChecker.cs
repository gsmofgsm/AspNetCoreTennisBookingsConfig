using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisBookings.Web.Services
{
    public class ProfanityChecker : IProfanityChecker
    {
        public bool ContainsProfanity(string input)
        {
            return string.IsNullOrEmpty(input) ? false : input.ToLowerInvariant().Contains("darn");
        }
    }
}
