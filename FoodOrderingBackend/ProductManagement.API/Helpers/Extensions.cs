using System;

namespace ProductManagement.API.Helpers
{
    public static class Extensions
    {
        public static string GetMimeTypeFromString(this string ext)
        {
            if (ext.Equals("jpg", StringComparison.InvariantCultureIgnoreCase))
                return "image/jpeg";
            if (ext.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase))
                return "image/jpeg";
            if (ext.Equals("png", StringComparison.InvariantCultureIgnoreCase))
                return "image/png";

            throw new ArgumentOutOfRangeException($"Mime type for extension {ext} not found.");
        }   
    }
}
