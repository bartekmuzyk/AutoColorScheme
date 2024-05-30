using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoColorScheme
{
    internal static class TimeIndex
    {
        public static int TimeToTimeIndex(TimeSpan timeOfDay)
        {
            return timeOfDay.Hours * 2 + (timeOfDay.Minutes >= 30 ? 1 : 0);
        }

        public static string TimeIndexToTime(int timeIndex)
        {
            var hours = timeIndex / 2;
            var minutes = timeIndex % 2 == 0 ? 0 : 30;
            return $"{hours:D2}:{minutes:D2}";
        }
    }
}
