using System;
using System.IO;

namespace BackendForFrontend.API.Helpers
{
    public static class Extensions
    {
        public static string GetMimeTypeFromFileName(this string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase))
                return "image/jpeg";
            if (extension.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase))
                return "image/jpeg";
            if (extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase))
                return "image/png";

            throw new ArgumentOutOfRangeException($"Mime type for extension {extension} not found.");
        }
    }
}