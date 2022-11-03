using ConverterTools.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CompositionRoot;
using ConverterTools;
using System.Threading;
using ConverterStartup.Progression;
using LoggerModules;
using log4net.Core;

namespace ConverterStartup
{
    public class CompositionRootProxy : MarshalByRefObject
    {
        string assemblyPath;
        ConverterCompositionRoot compositionRoot;
        CancellationTokenSource cancellationTokenSource;
        IEtaCalculator calculator;

        public void LoadAssembly(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
            this.compositionRoot = new ConverterCompositionRoot(this.assemblyPath);
            this.calculator = new EtaCalculator(10, 5);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public string AssemblyInfo
        {
            get
            {
                return  string.Format("{0}{1}",
                    string.IsNullOrEmpty(this.compositionRoot.ConverterTitle) ? "Безымянный конвертер" : this.compositionRoot.ConverterTitle,
                    string.IsNullOrEmpty(this.compositionRoot.ConverterVersion) ? "." : ". Версия " + this.compositionRoot.ConverterVersion);
            }
        }


        public void ManageConfiguration()
        {
            IConfiguration configuration = this.compositionRoot.CreateConfiguration();
            using (Form form = new ConfigForm(configuration))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }

            configuration.Save();
        }

        public void ExecuteConverter()
        {
            IEnumerable<IConverter> converters = this.compositionRoot.CreateConverter();
            this.cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = this.cancellationTokenSource.Token;
            foreach (IConverter converter in converters)
            {
                calculator.Reset();
                converter.ProgressChange += (o, args) => calculator.Update(args.ProgressPercentage / 100f);
                converter.Execute(token);
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            }
        }

        public void CancelConvert()
        {
            this.cancellationTokenSource.Cancel();
        }

        public ProgressInfo Progress()
        {
            return this.calculator.Progress;
        }

        public void SetLevels()
        {
            MessageListBoxAppender.SetLevels(Level.Info, Level.Fatal);
        }

        public void OnMessage(Action<string> action)
        {
            MessageListBoxAppender.AddMessage = action;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.IndexOf("resources", StringComparison.Ordinal) == -1)
            {
                Int32 z = args.Name.IndexOf(",", StringComparison.Ordinal);
                if (z > 0)
                {
                    string curModulePath = GetCurrentModulePath();
                    if (!String.IsNullOrEmpty(curModulePath))
                    {
                        String newName = Path.Combine(GetCurrentModulePath(),
                            args.Name.Substring(0, z).TrimEnd() + ".dll");
                        if (File.Exists(newName))
                        {
                            return Assembly.LoadFrom(newName);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Директория, в которой нужно искать сборки, если они не нашлись стандартным способом
        /// </summary>

        private string GetCurrentModulePath()
        {
            string currDir = Path.GetDirectoryName(this.assemblyPath);
            if (null == currDir)
            {
                var entryAsm = Assembly.GetEntryAssembly();
                if (null != entryAsm)
                {
                    currDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
            }

            return currDir;
        }
    }
}
