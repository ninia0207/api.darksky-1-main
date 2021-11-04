using System.Threading.Tasks;
using Weather.PCL.Models.Abstractions;

namespace Weather.PCL.Abstractions
{
    public interface IDataBase
    {
        public Task<IWeather> GetWeatherDataByLngAndLat(double lat, double lng, string lang);

    }
}
