using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTracker.Core.Services
{
    public interface IGeoDataService
    {
        string GetCountryCode(string countryName);
        List<string> GetCities(string countryCode);
        List<string> GetStatesOrProvinces(string countryCode);
    }
}
