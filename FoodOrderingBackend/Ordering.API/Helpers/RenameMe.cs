using System.Text.RegularExpressions;

namespace Ordering.API.Helpers
{
    public static class RenameMe
    {
        public static bool IsAValid24HexString(string value)
        {
            var rgx = new Regex("^[0-9a-fA-F]{24}$");
            return rgx.IsMatch(value);
        }
    }
}
    