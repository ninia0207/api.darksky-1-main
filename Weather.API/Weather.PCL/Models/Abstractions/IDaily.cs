

using System.Collections.Generic;
using Weather.PCL.Models.Implementations;

namespace Weather.PCL.Models.Abstractions
{
    public interface IDaily
    {
        public string Summary { get; set; }
        public string Icon { get; set; }
        public List<Datum> data { get; set; }
    }
}
