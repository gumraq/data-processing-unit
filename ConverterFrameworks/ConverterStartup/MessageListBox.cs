using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConverterStartup;
using log4net.Core;
using LoggerModules;

namespace ConverterTools.Logging
{
    public partial class MessageListBox : RichTextBox
    {
        public MessageListBox()
        {
            InitializeComponent();
            // MinLevelMessagesComboBox.Items.Add(Level.All.ToString());
            // MinLevelMessagesComboBox.Items.Add(Level.Info.ToString());
            // MinLevelMessagesComboBox.Items.Add(Level.Warn.ToString());
            // MinLevelMessagesComboBox.Items.Add(Level.Error.ToString());
           // MessageListBoxAppender.SetLevels(Level.Info, Level.Fatal);
            // Level lOLevel = MessageListBoxAppender.GetMinLevel();
            // if (!MinLevelMessagesComboBox.Items.Contains(lOLevel.ToString()))
            //     MinLevelMessagesComboBox.Items.Add(lOLevel.ToString());
            // MinLevelMessagesComboBox.SelectedItem = lOLevel.ToString();
           // MessageListBoxAppender.AddMessage = OnAddMessage;
        }

        public void SetDomainLogSettings(CompositionRootProxy proxy)
        {
            proxy.SetLevels();
            proxy.OnMessage(OnAddMessage);
        }

        /// <summary>
        ///   Добавить сообщение в лист сообщений
        /// </summary>
        /// <param name="pSValue"> Сообщение </param>
        private void OnAddMessage(string pSValue)
        {
            // Для запуска в асинхронном режиме изменить на BeginInvoke
            Invoke(new Action<string>((lSValue) =>
            {
                //SetMessageListBoxExtent(lSValue);
                AppendText( lSValue );
                
                //This selects and highlights the last line
                //this.SetSelected( this.Items.Count - 1, true );
                //this.SetSelected( this.Items.Count - 1, false );

            }), pSValue);
        }
        
        /// <summary>Sets a message list box extent.</summary>
        /// <param name="text">The text.</param>
        //private void SetMessageListBoxExtent(string text)
        //{
        //    int approxPixelsPerChar = 7;
        //    if ( text.Length > m_horizontalExtent )
        //    {
        //        m_horizontalExtent = text.Length;
        //        this.HorizontalExtent = m_horizontalExtent * approxPixelsPerChar;
        //    }
        //}
        
        /// <summary>Максимальная ширина поля для вывода сообщений.</summary>
        //private int m_horizontalExtent = 0;

        private void MessageListBox_TextChanged( object sender, EventArgs e )
        {
            // set the current caret position to the end
            SelectionStart = Text.Length;
            // scroll it automatically
            ScrollToCaret();
        }
    }
}
