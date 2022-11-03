using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterTools
{
    public interface IDataSourceProvider<T>
    {
        T Source();
    }
}
