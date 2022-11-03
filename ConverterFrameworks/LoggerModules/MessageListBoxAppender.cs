using System;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Repository.Hierarchy;

namespace LoggerModules
{
    public class MessageListBoxAppender : AppenderSkeleton
    {
        /// <summary>
        ///   Делегат вывода сообщений на форму
        /// </summary>
        public static Action<string> AddMessage = delegate { };

        /// <summary>
        ///   Получить объект фильтра уровней
        /// </summary>
        /// <returns> Объект фильтра или null </returns>
        public static LevelRangeFilter GetLevelRangeFilter()
        {
            // Поиск нужного аппендера
#if NETFULL
            Hierarchy lOHierarchy = LogManager.GetRepository() as Hierarchy;
#else
            Hierarchy lOHierarchy = LogManager.GetRepository(string.Empty) as Hierarchy;        // не уверен, что string.Empty для .Net Core будет работать. нужно попробовать
#endif
            MessageListBoxAppender lOMessageListBoxAppender =
                lOHierarchy == null ? null : lOHierarchy.Root.GetAppender("MessageListBoxAppender") as MessageListBoxAppender;
            if (lOMessageListBoxAppender != null)
            {
                // Поиск нужного фильтра
                IFilter lOFilter = lOMessageListBoxAppender.FilterHead;
                while (lOFilter != null)
                {
                    LevelRangeFilter lOLevelRangeFilter = lOFilter as LevelRangeFilter;
                    if (lOLevelRangeFilter != null)
                        return lOLevelRangeFilter;
                    lOFilter = lOFilter.Next;
                }
            }
            return null;
        }

        /// <summary>
        ///   Установить программно уровни сообщений для вывода
        /// </summary>
        /// <param name="pOMinLevel"> Минимальный уровень </param>
        /// <param name="pOMaxLevel"> Максимальный уровень </param>
        public static void SetLevels(Level pOMinLevel, Level pOMaxLevel)
        {
            LevelRangeFilter lOLevelRangeFilter = GetLevelRangeFilter();
            if (lOLevelRangeFilter != null)
            {
                lOLevelRangeFilter.LevelMin = pOMinLevel;
                lOLevelRangeFilter.LevelMax = pOMaxLevel;
            }
        }

        /// <summary>
        /// Метод - фактически копия метода сверху. Но сделан для того, чтобы не вызывать log4net для UI. 
        /// </summary>
        public static void SetLevels4ListBox()
        {
            SetLevels(Level.Info, Level.Fatal);
        }

        /// <summary>
        ///   Получить минимальный уровень фильтра
        /// </summary>
        /// <returns> Объект уровня или null </returns>
        public static Level GetMinLevel()
        {
            LevelRangeFilter lOLevelRangeFilter = GetLevelRangeFilter();
            return (lOLevelRangeFilter != null) ? lOLevelRangeFilter.LevelMin : null;
        }

        /// <summary>
        ///   Добавление события в журнал
        /// </summary>
        /// <param name="pOLoggingEvent"> Объект журналируемого события </param>
        protected override void Append(LoggingEvent pOLoggingEvent)
        {
            AddMessage(base.RenderLoggingEvent(pOLoggingEvent));
        }
    }
}