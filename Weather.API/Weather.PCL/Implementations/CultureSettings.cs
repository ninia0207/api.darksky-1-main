using Configs.Abstractions;
using Configs.Implementations;
using Newtonsoft.Json;
using Weather.PCL.Abstractions;
using Weather.PCL.Exceptions;
using Weather.PCL.Models.Enums;
using Weather.PCL.Models.Implementations;

namespace Weather.PCL.Implementations
{
    public class CultureSettings : ICultureSettings
    {
        public bool IsInited { get; private set; }
        public TempChoice Temperature = TempChoice.F;
        public MetersOrMiles MetersOrMiles = MetersOrMiles.mil;
        public IConfiguration configuration = new Configuration("config.json");
        public IConfiguration langConfiguration = new Configuration("language.json");
        private string _languageCode;

        
        public string LanguageCode
        {
            get { return _languageCode; }
            set {
                var langConfigJson = langConfiguration.GetConfigs();
                var langConfig = JsonConvert.DeserializeObject<Language[]>(langConfigJson);
                foreach (var item in langConfig)
                {
                    if (value == item.Code) _languageCode = value;
                    else throw new ThisLanguageDoesnotExsistsExcaption();
                }
                
            }
        }

        public CultureSettings()
        {
            Init();
        }
        private void Init()
        {
            if(configuration.IsConfigExsists())
            {
                var configJson = configuration.GetConfigs();
                var config = JsonConvert.DeserializeObject<WeatherConfig>(configJson);
                if(config is not null) {
                    Temperature = config.CorF;
                    LanguageCode = config.Lang;
                    MetersOrMiles = config.MilorMet;

                    IsInited = true;
                }
            }
            else IsInited = false;
        }

        public void ChoiceCorF(TempChoice choice)
        {
            Temperature = choice;
        }

        public void ChoiceMiorKM(MetersOrMiles metersOrMiles)
        {
            MetersOrMiles = metersOrMiles;
        }

        public bool ConvertMilesToMeters(out double meters, double miles)
        {
            if (MetersOrMiles == MetersOrMiles.meter) meters = miles * 1.609344;
            else meters = miles;

            return true;
        }

        public bool ConvertTemperatures(out double apparentTemperature, double temperature)
        {
            if (Temperature == TempChoice.C) temperature = (temperature - 32) * 5 / 9;
            apparentTemperature = temperature;

            return true;
        }

        public bool SaveChanges()
        {
            var weatherConfig = new WeatherConfig()
            {
                Id = 1,
                Lang = LanguageCode,
                CorF = Temperature,
                MilorMet = MetersOrMiles
            };

            configuration.SetConfigs(weatherConfig);
            Init();
            return true;
        }

        public bool SetLanguage(string languageCode)
        {
            LanguageCode = languageCode;
            return true;
        }
    }
}
