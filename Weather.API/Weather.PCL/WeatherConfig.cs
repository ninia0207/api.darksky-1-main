using Configs.Models.Abstractions;
using Weather.PCL.Models.Enums;

namespace Weather.PCL
{
    public class WeatherConfig : IConfig
    {
        public int Id { get; set; }

        public string Lang { get; set; }
        public TempChoice CorF { get; set; }
        public MetersOrMiles MilorMet { get; set; }
    }
}
