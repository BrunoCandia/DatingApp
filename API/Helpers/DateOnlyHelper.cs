using System.Globalization;

namespace API.Helpers
{
    public static class DateOnlyHelper
    {
        public static DateOnly StringToDateOnly(string dateOnly)
        {
            var parsedDateOnly = DateOnly.Parse(dateOnly, CultureInfo.InvariantCulture);

            return parsedDateOnly;
        }
    }
}
