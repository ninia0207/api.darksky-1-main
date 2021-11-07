using System.Diagnostics;
using System.Runtime.InteropServices;
using Weather.PCL.Abstractions;

namespace Weather.PCL.Implementations
{
    public class ImagesService : IImagesService
    {
        private string _rootPath = @"C:\Users\davit\OneDrive\Desktop\api.darksky-1-main\Weather_images\";

        public bool OpenImage(string name)
        {
            var fileName = _rootPath + name + ".jpg";

            if (!(string.IsNullOrEmpty(fileName) && string.IsNullOrWhiteSpace(fileName)))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {fileName}") { CreateNoWindow = true });
                }
            }
            return default;
        }
    }
}
