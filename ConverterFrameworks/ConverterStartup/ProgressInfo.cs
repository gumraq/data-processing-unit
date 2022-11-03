using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterStartup
{
    [Serializable]
    public struct ProgressInfo
    {
        public int Percent { get; set; }
        public TimeSpan Etr { get; set; }
    }
}
