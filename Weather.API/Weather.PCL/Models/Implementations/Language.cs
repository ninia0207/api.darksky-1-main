using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.PCL.Models.Abstractions;

namespace Weather.PCL.Models.Implementations
{
    public class Language : ILanguage
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
    }
}
