using Configs.Models.Abstractions;
namespace Configs.Abstractions
{
    public interface IConfiguration
    {
        public bool IsConfigExsists();

        public bool SetConfigs(IConfig config);
        public bool SetCityConfigs(IConfig[] config);
        public string GetConfigs();

        public bool DeleteConfig(string path);

        public void SetConfigFileName(string name);
    }
}
