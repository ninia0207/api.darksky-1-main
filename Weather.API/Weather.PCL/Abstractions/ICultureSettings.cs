using Weather.PCL.Models.Enums;

namespace Weather.PCL.Abstractions
{
    public interface ICultureSettings
    {
        public void ChoiceCorF(TempChoice temp);
        public void ChoiceMiorKM(MetersOrMiles metersOrMiles);

        public bool ConvertTemperatures(out double apparentTemperature, double temperature);

        public bool ConvertMilesToMeters(out double meters, double miles);

        public bool SetLanguage(string LanguageCode);


        public bool SaveChanges();
    }
}
