using Configs.Abstractions;
using Configs.Models.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Configs.Implementations
{
    public class Configuration : IConfiguration
    {
        private string _rootPath = @"C:\Users\ninia\OneDrive\Documents\GitHub\api.darksky-1-main\Weather.API\Configs\Configs\";

        private string _path;

        public string Path
        {
            get { return _path; }
            set { 
                _path = _rootPath + value; 
            }
        }



        public Configuration(string path)
        {
            SetConfigFileName(path);
        }

        public string GetConfigs()
        {
            using (StreamReader sr = new StreamReader(_path))
            {
                var config = sr.ReadToEnd();

                if (config is null) return "Err";

                sr.Close();

                return config;
            }
        }

        public bool SetConfigs(IConfig config)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_path))
                {
                    var json = JsonConvert.SerializeObject(config);
                    sw.Write(json);

                    sw.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetCityConfigs(IConfig[] config)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_path))
                {
                    var json = JsonConvert.SerializeObject(config);
                    sw.Write(json);

                    sw.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool IsConfigExsists()
        {
            using (StreamReader sr = new StreamReader(_path))
            {
                var config = sr.ReadLine();

                if (config is null || string.IsNullOrEmpty(config) || string.IsNullOrWhiteSpace(config)) return false;

                sr.Close();
            }
            return true;
        }

        public void SetConfigFileName(string name) => Path = name;

        public bool DeleteConfig(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_rootPath + path))
                {
                    sw.Write("");

                    sw.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
