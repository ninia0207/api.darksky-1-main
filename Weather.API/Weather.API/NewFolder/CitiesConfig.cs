using Configs.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.API.NewFolder
{
    public class CitiesConfig : IConfig
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
