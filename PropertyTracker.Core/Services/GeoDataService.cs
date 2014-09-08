using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTracker.Core.Services
{

    public class GeoDataService : IGeoDataService
    {
        public GeoDataService()
        {
            
        }

        public string GetCountryCode(string countryName)
        {         
            string countryCode = null;
            switch (countryName.ToLower())
            {
                case "can":
                case "ca":
                case "canada":
                    countryCode = "CA";
                    break;
                case "united states of america":
                case "united states":
                case "us":
                case "usa":
                    countryCode = "US";
                    break;
            }
            return countryCode;
        }

        public List<string> GetCities(string countryCode)
        {
            var cities = new List<string> { "Toronto", "Quebec City", "Halifax", "Fredericton", "Winnipeg", "Victoria", "Charlottetown", "Regina", "Edmonton", "St. John's" };
            cities.Sort();           
            return cities;
        }

        
        public List<string> GetStatesOrProvinces(string countryCode)
        {
            var provs = new List<string> { "Ontario", "Nova Scotia", "Quebec", "Manitoba", "Alberta", "British Columbia", "New Brunswick", "Prince Edward Island", "Saskatchewan", "Newfoundland and Labrador" };
            provs.Sort();
            return provs;
        }


        /*
         * 
         top-level
        {
            "totalResultsCount": 219,
            "geonames":[  ]
        }
          
         geoname entry:
         {
         CANADA
         "adminCode1":"08",
         "lng":"-75.69812",
         "geonameId":6094817,
         "toponymName":"Ottawa",
         "countryId":"6251999",
         "fcl":"P",
         "population":812129,
         "countryCode":"CA",
         "name":"Ottawa",
         "fclName":"city, village,...",
         "countryName":"Canada",
         "fcodeName":"capital of a political entity",
         "adminName1":"Ontario",
         "lat":"45.41117",
         "fcode":"PPLC"
          },
         
         US
         "countryId":"6252001",
         "adminCode1":"NY",
         "countryName":"United States",
         "fclName":"city, village,...",
         "countryCode":"US",
         "lng":"-74.00597",
         "fcodeName":"populated place",
         "toponymName":"New York City",
         "fcl":"P",
         "name":"New York",
         "fcode":"PPL",
         "geonameId":5128581,
         "lat":"40.71427",
         "adminName1":"New York",
         "population":8175133
        */

        // http://api.geonames.org/searchJSON?country=CA&maxRows=1000&username=kshaikh&cities=cities15000&style=medium

        // http://api.geonames.org/searchJSON?country=US&maxRows=1000&username=kshaikh&cities=cities15000&style=medium&startRow=3000
        private void GetGeoData()
        {
            
        }

        void canada()
        {            

        }
    }
}
