
using System.Collections.Generic;
using Weather.PCL.Models.Abstractions;

namespace Weather.PCL.Models.Implementations
{
    public class Hourly : IHourly
    {
        public string Summary { get; set; }
        public string Icon { get; set; }
        public List<Datum> data { get; set; }
    }
}
