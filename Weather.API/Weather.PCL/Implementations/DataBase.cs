using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.PCL.Abstractions;
using Weather.PCL.Models.Abstractions;
using Weather.PCL.Models.Enums;
using WebClient.Abstractions;

namespace Weather.PCL.Implementations
{
    public class DataBase : IDataBase
    {
        private readonly IWebClient _webClient = null;
        private readonly ICultureSettings _cultureSettings = null;

        private Models.Implementations.Weather weatherInfo = null;

        public DataBase(ICultureSettings cultureSettings)
        {
            _webClient = new WebClient.Implementations.WebClient();
            _cultureSettings = cultureSettings;
        }


        public async Task<IWeather> GetWeatherDataByLngAndLat(double lat, double lng, string lang)
        {
            if(weatherInfo is not null)
            if(weatherInfo.Latitude == lat && weatherInfo.Longitude == lng)
            {
                return weatherInfo;
            }


            var weatherUrl = $"https://api.darksky.net/forecast/f11a85ff0f9dca472f8be4d387384bfb/{lat},{lng}?lang={lang}";
            _webClient.ChangeUrl(weatherUrl);
            var weatherData = await _webClient.GetDataAsync();
            var weatherDataObj = JsonConvert.DeserializeObject<Weather.PCL.Models.Implementations.Weather>(weatherData);

            _cultureSettings.ConvertTemperatures(out double temp, weatherDataObj.Currently.ApparentTemperature);
            weatherDataObj.Currently.ApparentTemperature = temp;

            _cultureSettings.ConvertMilesToMeters(out double value, weatherDataObj.Currently.WindSpeed);
            weatherDataObj.Currently.WindSpeed = value;

            return weatherInfo = weatherDataObj;
        }
    }
}
