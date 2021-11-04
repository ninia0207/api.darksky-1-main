namespace Weather.PCL.Models.Abstractions
{
    public interface ICity
    {
        public string CityName { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
