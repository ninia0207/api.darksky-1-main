using Configs.Models.Abstractions;
using Weather.PCL.Models.Abstractions;

namespace Weather.PCL.Models.Implementations
{
    public class City : ICity, IConfig
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
