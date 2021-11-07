using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.PCL.Models.Abstractions
{
    public interface ILanguage
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }

    }
}
