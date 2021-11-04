using Configs.Abstractions;
using Configs.Implementations;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.PCL.Abstractions;
using Weather.PCL.Implementations;
using Weather.PCL.Models.Enums;
using Weather.PCL.Models.Implementations;

namespace Weather.API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IDataBase db = new DataBase();
            //41.716667
            //44.783333

            do
            {
                IConfiguration configuration = new Configuration("config.json");

                configuration.SetConfigFileName("cities.json");

                var isCitiesExsists = configuration.IsConfigExsists();

                bool isCorrectLng = false;
                bool isCorrectLat = false;
                double lng = default;
                double lat = default;
                int userLoc = default;
                if (!isCitiesExsists)
                {
                    Console.Write("Enter lng: ");
                    isCorrectLng = double.TryParse(Console.ReadLine(), out lng);
                    Console.Write("Enter lat: ");
                    isCorrectLat = double.TryParse(Console.ReadLine(), out lat);

                    Console.WriteLine("Do you want to save this location? ");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    Console.Write("Enter your answer: ");
                    userLoc = Int32.Parse(Console.ReadLine());

                    if (!isCorrectLng || !isCorrectLat) continue;
                }
                else
                {
                    var citiesJson = configuration.GetConfigs();
                    var citiesArray = JsonConvert.DeserializeObject<City[]>(citiesJson);

                    foreach (var item in citiesArray)
                    {
                        Console.WriteLine(item.Id + ". " + item.CityName);
                    }

                    Console.WriteLine((citiesArray.LastOrDefault().Id + 1) + ". Clear History ");


                    var cityChoice = int.TryParse(Console.ReadLine(), out int cityId);
                    var currentCity = citiesArray.FirstOrDefault(o => o.Id == cityId);

                    if(currentCity is null)
                    {
                        Console.Clear();
                        configuration.DeleteConfig("config.json");
                        configuration.DeleteConfig("cities.json");
                        continue;
                    }

                    lng = currentCity.Lng;
                    lat = currentCity.Lat;
                }

                configuration.SetConfigFileName("config.json");


                string userLang = default;
                int choiceCorF = default;

                if (!configuration.IsConfigExsists())
                {
                    Console.WriteLine("1. Celcius");
                    Console.WriteLine("2. Farenheit");
                    Console.Write("Your Choice: ");
                    var userChoice = int.TryParse(Console.ReadLine(), out choiceCorF);
                    if (!userChoice || choiceCorF > 2) continue;
                    db.ChoiceCorF((TempChoice)choiceCorF);

                    Console.Write("Enter the language(use abbreviation like english = en): ");
                    userLang = Console.ReadLine();
                    if (userLang.Length > 2) continue;

                    var weatherConfig = new WeatherConfig.WeatherConfig()
                    {
                        Id = 1,
                        Lang = userLang,
                        CorF = (TempChoice)choiceCorF
                    };

                    configuration.SetConfigs(weatherConfig);
                }
                else
                {
                    var confStr = configuration.GetConfigs();
                    var confObj = JsonConvert.DeserializeObject<WeatherConfig.WeatherConfig>(confStr);
                    userLang = confObj.Lang;
                    choiceCorF = (int)confObj.CorF;
                }

                Console.Clear();
                Console.WriteLine("Loading...");
                var weatherInfo = await db.GetWeatherDataByLngAndLat(lat, lng, userLang);
                Console.Clear();

                Console.WriteLine($"Longitude: {weatherInfo.Longitude}  Latitude: {weatherInfo.Latitude}");
                Console.WriteLine($"Timezone: {weatherInfo.Timezone}");
                Console.WriteLine($"Summary: {weatherInfo.Currently.Summary}");
                char tempChoice;
                TempChoice userTempChoice = (TempChoice)choiceCorF;
                if (userTempChoice == TempChoice.C)
                {
                    tempChoice = 'C';
                }
                else
                {
                    tempChoice = 'F';
                }
                Console.WriteLine($"Temperature: {(int)weatherInfo.Currently.ApparentTemperature} °{tempChoice}");
                Console.WriteLine($"Rain Probability: {weatherInfo.Currently.PrecipProbability}");
                Console.WriteLine($"Wind Speed: {weatherInfo.Currently.WindSpeed} m/s");
                Console.WriteLine($"Daily Summary: {weatherInfo.Daily.Summary}");
                Console.WriteLine($"Hourly Summary: {weatherInfo.Hourly.Summary}");

                IConfiguration citConfiguration = new Configuration("cities.json");
                if (userLoc == 1)
                {
                    var citiesConfig = new City()
                    {
                        Id = 1,
                        CityName = $"{weatherInfo.Timezone}", 
                        Lng = weatherInfo.Longitude, 
                        Lat = weatherInfo.Latitude
                    };
                    citConfiguration.SetConfigs(citiesConfig);
                }

                
                Console.ReadKey();
                Console.Clear();
            } while (true);
            

            Console.ReadKey();
        }
    }
}
