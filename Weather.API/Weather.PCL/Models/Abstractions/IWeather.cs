using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.PCL.Models.Implementations;

namespace Weather.PCL.Models.Abstractions
{
    public interface IWeather
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public Currently Currently { get; set; }
        public Daily Daily { get; set; }
        public Hourly Hourly { get; set; }

    }
}
