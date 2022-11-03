using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConverterStartup.PropertyGridItems;


namespace ConverterStartup
{
    public partial class MainForm : Form
    {
        private StateForm stateForm;
        private CompositionRootProxy proxy;
        private AppDomain domain;


        public MainForm()
        {
            InitializeComponent();

            this.stateForm = new StateForm(this);
            this.stateForm.ConverterLibNotLoad();
        }

        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Модуль конвертера (*.dll)|*.dll",
                Title = "Выберите сборку конвертера"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.LoadConverter(ofd.FileName);
                    Properties.Settings.Default.ConverterDllPath = ofd.FileName;
                    Properties.Settings.Default.Save();
                }
            }
        }
        private void toolStripSettings_Click(object sender, EventArgs e)
        {
            this.proxy.ManageConfiguration();
        }

        private async void toolStripStart_Click(object sender, EventArgs e)
        {
            if (this.stateForm.CurrentState != StateForm.ConvertingStateForm)
            {
                // Запуск конвертирования
                this.stateForm.Converting();

                try
                {
                    using (Timer timer = new Timer() { Interval = 1000 })
                    {
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        await Task.Run(() => this.proxy.ExecuteConverter());

                        MessageBox.Show("Конвертирование выполнено");
                        this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff")} WARN  - Конвертирование выполнено!\r\n");
                    }
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Конвертирование было прервано");
                    this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff")} WARN  - Конвертирование остановлено по запросу пользователя!\r\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Конвертирование остановлено из-за ошибки:\n{this.AllDetails(ex)}", "Произошла ошибка");
                    this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff")} WARN  - Конвертирование остановлено из-за ошибки!\r\n");
                }
                finally
                {
                    stateForm.AwaitingStartConverter();
                }


                
            }
            else
            {
                // Прерывание конвертирования
                if (MessageBox.Show("Конвертирование ещё не закончилось. Прервать?", "Остановка конвертера", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.proxy.CancelConvert();
                    this.stateForm.AwaitingStartConverter();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ProgressInfo pi = this.proxy.Progress();
            this.toolStripProgressBar1.Value = pi.Percent;
            if (pi.Etr.Days>=0 && pi.Etr.Days<100)
            {
                this.toolStripStatusLabel1.Text = string.Concat("Осталось: ",
                    pi.Etr.TotalMinutes > 1 ? string.Concat((int)pi.Etr.TotalMinutes, " мин. ") : string.Empty,
                    pi.Etr.Seconds, " сек.");
            }
            else
            {
                this.toolStripStatusLabel1.Text = "Оценка оставшегося времени...";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stateForm.CurrentState == StateForm.ConvertingStateForm)
            {
                e.Cancel = e.Cancel ||
                       MessageBox.Show("Закрыть конвертер?", "Закрытие конвертера", MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) != DialogResult.OK;
            }

            if (!e.Cancel)
            {
                if (this.domain != null)
                {
                    AppDomain.Unload(this.domain);
                    this.domain = null;
                }
            }
        }

        private void LoadConverter(string assemblyPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(assemblyPath) && File.Exists(assemblyPath))
                {
                    if (this.domain != null)
                    {
                        AppDomain.Unload(this.domain);
                        this.domain = null;
                    }
                    AsmInfoTable asmInfoTable = new AsmInfoTable();
                    asmInfoTable.LoadAsmInfo(assemblyPath);

                    AppDomainSetup domaininfo = new AppDomainSetup { ApplicationBase = System.Environment.CurrentDirectory};
                    Evidence adevidence = AppDomain.CurrentDomain.Evidence;
                    this.domain = AppDomain.CreateDomain("ConverterStartupDomain", adevidence, domaininfo, AppDomain.CurrentDomain.PermissionSet);
                    Type type = typeof(CompositionRootProxy);
                    this.proxy = (CompositionRootProxy)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                    proxy.LoadAssembly(assemblyPath);
                    this.richTextBoxLog.SetDomainLogSettings(this.proxy);


                    this.Text = this.proxy.AssemblyInfo;
                    this.gvAsmInfo.DataSource = asmInfoTable;

                    this.stateForm.AwaitingStartConverter();
                }

                if (!Text.Contains("Оболочка v"))
                    Text += ",   Оболочка v" + GetShellVersion();
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Concat("Не удалось загрузить конвенвертер.\n", ex.Message), "Ошибка при запуске",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetShellVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return version;
        }

        #region Состояние элементов управления формы
        /// <summary>
        /// Автомат состояний формы
        /// </summary>
        private class StateForm
        {
            public static string ConverterLibNotLoadStateForm = "ConverterLibNotLoad";
            public static string AwaitingStartConverterStateForm = "AwaitingStartConverter";
            public static string ConvertingStateForm = "Converting";

            private MainForm form;
            public string CurrentState { get; private set; }

            public StateForm(MainForm form)
            {
                this.form = form;
            }

            /// <summary>
            /// Применить настройки элементов управления, когда не загрукжен конвертер
            /// </summary>
            public void ConverterLibNotLoad()
            {
                form.toolStripStart.Image = Properties.Resources.bt_play;
                form.toolStripStart.Enabled = false;
                form.toolStripSettings.Enabled = false;
                form.toolStripOpen.Enabled = true;
                this.CurrentState = StateForm.ConverterLibNotLoadStateForm;
            }

            /// <summary>
            /// Применить настройки элементов управления, когда ожидается старт конвертирования
            /// </summary>
            public void AwaitingStartConverter()
            {
                form.toolStripStart.Image = Properties.Resources.bt_play;
                form.toolStripStart.Enabled = true;
                form.toolStripSettings.Enabled = true;
                form.toolStripOpen.Enabled = true;
                form.toolStripProgressBar1.Value = 0;
                form.toolStripStatusLabel1.Text = string.Empty;
                this.CurrentState = StateForm.AwaitingStartConverterStateForm;
            }

            /// <summary>
            /// Применить настройки элементов управления, когда идет ковертирование
            /// </summary>
            public void Converting()
            {
                form.toolStripStart.Image = Properties.Resources.bt_stop;
                form.toolStripStart.Enabled = true;
                form.toolStripSettings.Enabled = false;
                form.toolStripOpen.Enabled = false;
                this.CurrentState = StateForm.ConvertingStateForm;
            }
        }

        #endregion

        private void MainForm_Load( object sender, EventArgs e )
        {
            Text += " [Оболочка v" + GetShellVersion() + "]";
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ConverterDllPath))
            {
               this.LoadConverter(Properties.Settings.Default.ConverterDllPath);
            }
        }

        public string AllDetails(Exception exception)
        {
            if (exception == null)
                return null;

            Exception current;

            var traces = new Stack<string>();
            current = exception;
            do
            {
                traces.Push(current.StackTrace);
                traces.Push(current.Message);
            } while ((current = current.InnerException) != null);
            traces.Push("StackTrace:");
            traces.Push(string.Empty);

            //соберём только Messages ещё раз в заголовке в нормальном порядке
            var headerMessages = new Queue<string>();
            current = exception;
            do
            {
                headerMessages.Enqueue(current.Message);
            } while ((current = current.InnerException) != null);

            return string.Join("\r\n", headerMessages.Concat(traces));
        }
    }
}
