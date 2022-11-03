using System;
using System.ComponentModel;
using System.Threading;

namespace ConverterTools
{
    public interface IConverter
    {
        event EventHandler<ProgressChangedEventArgs> ProgressChange;
        void Execute(CancellationToken cancellationToken);
    }
}
