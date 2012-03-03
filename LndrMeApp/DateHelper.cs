using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LndrMeApp
{
    public class DateHelper
    {

        // Shamelessly adapted from http://gaskell.org/rails-helper-distance_of_time_in_words-ported-to-c/

        public static string TimeAgoInWords(DateTime fromTime)
        {
            return DistanceOfTimeInWords(fromTime, DateTime.Now);
        }

        public static string TimeAgoInWords(DateTime fromTime, bool includeSeconds)
        {
            return DistanceOfTimeInWords(fromTime, DateTime.Now, includeSeconds);
        }

        public static string DistanceOfTimeInWords(DateTime fromTime, DateTime toTime)
        {
            return DistanceOfTimeInWords(fromTime, toTime, false);
        }

        public static string DistanceOfTimeInWords(DateTime fromTime, DateTime toTime, bool includeSeconds)
        {
            TimeSpan ts = (toTime - fromTime).Duration();
            int distanceInMinutes = (int)ts.TotalMinutes;
            int distanceInSeconds = (int)ts.TotalSeconds;

            string inWords = string.Empty;

            if (distanceInMinutes <= 1)
            {
                if (includeSeconds)
                {
                    if (InRange(0, 4, distanceInSeconds)) { inWords = Resources.LessThanFiveSeconds; }
                    else if (InRange(5, 9, distanceInSeconds)) { inWords = Resources.LessThanTenSeconds; }
                    else if (InRange(10, 19, distanceInSeconds)) { inWords = Resources.LessThanTwentySeconds; }
                    else if (InRange(20, 39, distanceInSeconds)) { inWords = Resources.HalfAMinute; }
                    else if (InRange(40, 59, distanceInSeconds)) { inWords = Resources.LessThanAMinute; }
                    else { inWords = Resources.OneMinute; }
                }
                else
                {
                    inWords = distanceInMinutes == 0 ? Resources.LessThanAMinute : Resources.OneMinute;
                }
            }
            else
            {
                if (InRange(2, 44, distanceInMinutes)) { inWords = string.Format(Resources.MultipleMinutes, distanceInMinutes); }
                else if (InRange(45, 89, distanceInMinutes)) { inWords = Resources.AboutOneHour; }
                else if (InRange(90, 1439, distanceInMinutes))
                {
                    inWords = string.Format(Resources.AboutMultipleHours, RoundedDistance(distanceInMinutes, 60));
                }
                else if (InRange(1440, 2879, distanceInMinutes)) { inWords = Resources.OneDay; }
                else if (InRange(2880, 43199, distanceInMinutes)) { inWords = string.Format(Resources.MultipleDays, RoundedDistance(distanceInMinutes, 1440)); }
                else if (InRange(43200, 86399, distanceInMinutes)) { inWords = Resources.AboutOneMonth; }
                else if (InRange(86400, 525599, distanceInMinutes)) { inWords = string.Format(Resources.MultipleMonths, RoundedDistance(distanceInMinutes, 43200)); }
                else if (InRange(525600, 1051199, distanceInMinutes)) { inWords = Resources.AboutOneYear; }
                else { inWords = string.Format(Resources.MultipleYears, RoundedDistance(distanceInMinutes, 525600)); }
            }

            return inWords;
        }

        private static int RoundedDistance(int value, double dividedBy)
        {
            decimal d = Convert.ToDecimal(value / dividedBy);
            if (d > 0)
            {
                return (int)decimal.Ceiling(d);
            }
            else
            {
                return (int)decimal.Floor(d);
            }

            // MidpointRounding.AwayFromZero is not supported in Windows Phone 7 or Silverlight
            //return (int)decimal.Round(Convert.ToDecimal(value / dividedBy), MidpointRounding.AwayFromZero);
        }

        private static bool InRange(int low, int high, int value)
        {
            return (value >= low && value <= high);
        }
    }
}
