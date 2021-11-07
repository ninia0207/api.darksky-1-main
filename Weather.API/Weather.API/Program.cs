using Configs.Abstractions;
using Configs.Implementations;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Weather.PCL.Abstractions;
using Weather.PCL.Implementations;
using Weather.PCL.Models.Enums;
using Weather.PCL.Models.Implementations;

namespace Weather.API
{


    //string userLang = default;
    //int choiceCorF = default;

    //if (!configuration.IsConfigExsists())
    //{
    //    Console.WriteLine("1. Celcius");
    //    Console.WriteLine("2. Farenheit");
    //    Console.Write("Your Choice: ");
    //    var userChoice = int.TryParse(Console.ReadLine(), out choiceCorF);
    //    if (!userChoice || choiceCorF > 2) continue;
    //    db.ChoiceCorF((TempChoice)choiceCorF);

    //    Console.Write("Enter the language(use abbreviation like english = en): ");
    //    userLang = Console.ReadLine();
    //    if (userLang.Length > 2) continue;

    //    var weatherConfig = new WeatherConfig.WeatherConfig()
    //    {
    //        Id = 1,
    //        Lang = userLang,
    //        CorF = (TempChoice)choiceCorF
    //    };

    //    configuration.SetConfigs(weatherConfig);
    //}
    //else
    //{
    //    var confStr = configuration.GetConfigs();
    //    var confObj = JsonConvert.DeserializeObject<WeatherConfig.WeatherConfig>(confStr);
    //    userLang = confObj.Lang;
    //    choiceCorF = (int)confObj.CorF;
    //}

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            CultureSettings culture = new CultureSettings();

            IImagesService imageService = new ImagesService();

            //41.716667
            //44.783333

            do
            {
                if (!culture.IsInited)
                {
                    Console.WriteLine("1. Celcius");
                    Console.WriteLine("2. Farenheit");
                    Console.Write("Your Choice: ");
                    int choiceCorF = default;
                    var userChoice = int.TryParse(Console.ReadLine(), out choiceCorF);
                    if (!userChoice || choiceCorF > 2) continue;
                    culture.ChoiceCorF((TempChoice)choiceCorF);

                    Console.Write("Enter the language(use abbreviation like english = en): ");
                    string userLang = default;
                    userLang = Console.ReadLine();
                    if (userLang.Length > 2) continue;
                    culture.SetLanguage(userLang);

                    Console.WriteLine("1. mil");
                    Console.WriteLine("2. meter");
                    Console.Write("Your Choice: ");
                    var isSuccesskmOrMi = int.TryParse(Console.ReadLine(), out int kmOrMi);
                    culture.ChoiceMiorKM((MetersOrMiles)kmOrMi);

                    culture.SaveChanges();
                }

                IDataBase db = new DataBase(culture);

                culture.configuration.SetConfigFileName("cities.json");

                var isCitiesExsists = culture.configuration.IsConfigExsists();

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
                    var citiesJson = culture.configuration.GetConfigs();
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
                        culture.configuration.DeleteConfig("config.json");
                        culture.configuration.DeleteConfig("cities.json");
                        continue;
                    }

                    culture.configuration.SetConfigFileName("config.json");
                    
                    lng = currentCity.Lng;
                    lat = currentCity.Lat;
                }

                Console.Clear();
                Console.WriteLine("Loading...");
                var weatherInfo = await db.GetWeatherDataByLngAndLat(lat, lng, culture.LanguageCode);
                Console.Clear();

                Console.WriteLine($"Longitude: {weatherInfo.Longitude}  Latitude: {weatherInfo.Latitude}");
                Console.WriteLine($"Timezone: {weatherInfo.Timezone}");
                Console.WriteLine($"Summary: {weatherInfo.Currently.Summary}");

                Console.WriteLine($"Temperature: {(int)weatherInfo.Currently.ApparentTemperature} °{culture.Temperature.ToString()[0]}");
                Console.WriteLine($"Rain Probability: {weatherInfo.Currently.PrecipProbability}");
                Console.WriteLine($"Wind Speed: {weatherInfo.Currently.WindSpeed} m/s");
                Console.WriteLine($"Daily Summary: {weatherInfo.Daily.Summary}");
                Console.WriteLine($"Hourly Summary: {weatherInfo.Hourly.Summary}");

                imageService.OpenImage(weatherInfo.Currently.Icon);

                IConfiguration citConfiguration = new Configuration("cities.json");
                if (userLoc == 1)
                {
                    var citiesConfig = new City[]
                    {
                        new City
                        {
                            Id = 1,
                            CityName = $"{weatherInfo.Timezone}",
                            Lng = weatherInfo.Longitude,
                            Lat = weatherInfo.Latitude
                        }

                    };
                    citConfiguration.SetCityConfigs(citiesConfig);
                }


                Console.ReadKey();
                Console.Clear();
            } while (true);
            

            Console.ReadKey();
        }
    }
}
