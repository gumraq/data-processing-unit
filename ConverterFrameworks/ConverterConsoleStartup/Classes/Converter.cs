using CompositionRoot;
using ConverterTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using ConverterTools.Configurations;
using log4net;
using System.Runtime.ExceptionServices;
using ConverterConsoleStartup.Classes;

namespace ConverterConsoleStartup
{
    public class Converter
    {
        private readonly string _assemblyPath;
        private readonly string _pathToConfig;
        private CancellationTokenSource _cancellationTokenSource;
        private ConsoleConverterCompositionRoot _compositionRoot;
        private  ILog _logger;
        private float _prevProgress = -1;

        public Converter(string pathToAsm, string pathToConfig)
        {
            _assemblyPath = pathToAsm;
            _pathToConfig = pathToConfig;
            Initialize();
        }

        public event EventHandler<ProgressChangedEventArgs> ProgressChange;
        public void Execute()
        {
            IEnumerable<IConverter> converters = _compositionRoot.CreateConverter();
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;
            try
            {
                foreach (IConverter converter in converters)
                {
                    converter.ProgressChange += OnProgressChange;
                    converter.Execute(token);
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
     
        }

        private void Initialize()
        {
            _compositionRoot = new ConsoleConverterCompositionRoot(_assemblyPath);
            _logger = _compositionRoot.GetLogger();
            SetParams();
        }

        private void OnProgressChange(object o, ProgressChangedEventArgs args)
        {
            var currentProgress = args.ProgressPercentage;
            if (_prevProgress!= currentProgress)
            {
                _logger.Info($"Progress: {currentProgress}%");
                _prevProgress = currentProgress;
            }     
            ProgressChange?.Invoke(o, args);
        }
        private IEnumerable<ParamDescWithValue> ReadConfig(string pathToConfig)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ParamDescWithValue));
            XPathDocument document = new XPathDocument(pathToConfig);
            string xpath = "configuration/converter/setting/value/ParamDescWithValue";
            foreach (XPathNavigator navParam in document.CreateNavigator().Select(xpath))
            {
                yield return (ParamDescWithValue)serializer.Deserialize(navParam.ReadSubtree());
            }
        }

        void SetParams()
        {
            var config = _compositionRoot.CreateConfiguration();
            if (_pathToConfig == null) return;
            var paramDict = config.Parameters.Values.ToDictionary(x => x.ParamName, StringComparer.OrdinalIgnoreCase);
            foreach (var arg in ReadConfig(_pathToConfig))
            {
                if (paramDict.TryGetValue(arg.ParamName,out var param))
                {
                    param.Value = arg.Value;
                }
            }
        }
    }
}
