using System.Globalization;

namespace API.Helpers
{
    public static class DateOnlyHelper
    {
        public static DateOnly StringToDateOnly(string date)
        {
            var parsedDateOnly = DateOnly.Parse(date, CultureInfo.InvariantCulture);

            return parsedDateOnly;
        }
    }
}
