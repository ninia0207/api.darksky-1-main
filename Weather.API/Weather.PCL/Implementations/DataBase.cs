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
        private TempChoice _temperature = TempChoice.F;
        private Models.Implementations.Weather weatherInfo = null;

        public DataBase()
        {
            _webClient = new WebClient.Implementations.WebClient();

        }

        public void ChoiceCorF(TempChoice choice)
        {
            _temperature = choice;
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
            if(_temperature == TempChoice.C) weatherDataObj.Currently.ApparentTemperature = (weatherDataObj.Currently.ApparentTemperature - 32) * 5 / 9;

            return weatherInfo = weatherDataObj;
        }
    }
}
