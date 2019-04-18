using System;

namespace com.csutil {

    public static class DateTimeParser {

        public static DateTime NewDateTimeFromUnixTimestamp(long unixTimeInMs, bool autoCorrectIfPassedInSeconds = true) {
            AssertV2.IsTrue(unixTimeInMs > 0, "NewDateTimeFromUnixTimestamp: unixTimeInMs was " + unixTimeInMs);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var result = dtDateTime.AddMilliseconds(unixTimeInMs);
            if (autoCorrectIfPassedInSeconds && result.Year == 1970) {
                var correctedDate = NewDateTimeFromUnixTimestamp(unixTimeInMs * 1000, false);
                Log.e("The passed unixTimeInMs was likely passed in seconds instead of milliseconds,"
                    + " it was too small by a factor of *1000, which would result in " + correctedDate.ToReadableString());
                return correctedDate;
            }
            return result;
        }

        public static DateTime ParseV2(string utcString) {
            if (utcString.Contains("GMT")) {
                utcString = utcString.Substring(0, "GMT", true);
            } else if (utcString.Contains("UTC")) {
                // RFC1123Pattern expects GMT and crashes on UTC
                utcString = utcString.Substring(0, "UTC", false) + "GMT";
            }
            return DateTime.Parse(utcString);
        }

        public static bool IsBetween(this DateTime self, DateTime lowerBound, DateTime upperBound) {
            return self.Ticks >= lowerBound.Ticks && self.Ticks <= upperBound.Ticks;
        }

    }

    public static class DateTimeExtensions {
        /// <summary> sortable, short, hard to read wrong, can be used in pathes. read also https://stackoverflow.com/a/15952652/165106 </summary>
        public static string ToReadableString(this DateTime self) {
            return self.ToUniversalTime().ToString("yyyy-MM-dd_HH.mm");
        }

        public static long ToUnixTimestamp(this DateTime self) {
            var zero = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return Convert.ToInt64(Math.Truncate((self.ToUniversalTime().Subtract(zero)).TotalMilliseconds));
        }

        public static bool IsBefore(this DateTime self, DateTime other) {
            return self.ToUnixTimestamp() < other.ToUnixTimestamp();
        }

        public static bool IsAfter(this DateTime self, DateTime other) {
            return self.ToUnixTimestamp() > other.ToUnixTimestamp();
        }

    }

}