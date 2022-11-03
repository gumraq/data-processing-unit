using System;

namespace ConverterStartup.Progression
{
    public interface IEtaCalculator
    {
        /// <summary> Clears all collected data.
        /// </summary>
        void Reset();

        /// <summary> Updates the current progress.
        /// </summary>
        /// <param name="progress">The current level of completion.
        /// Must be between 0.0 and 1.0 (inclusively).</param>
        void Update(float progress);

        /// <summary> Returns True when there is enough data to calculate the ETA.
        /// Returns False if the ETA is still calculating.
        /// </summary>
        bool ETAIsAvailable { get; }

        /// <summary> Calculates the Estimated Time of Arrival (Completion)
        /// </summary>
        DateTime ETA { get; }

        /// <summary> Calculates the Estimated Time Remaining.
        /// </summary>
        TimeSpan ETR { get; }

        /// <summary>
        /// Вычисляет информацию для статус бара конвентера
        /// </summary>
        ProgressInfo Progress { get; }
    }
}