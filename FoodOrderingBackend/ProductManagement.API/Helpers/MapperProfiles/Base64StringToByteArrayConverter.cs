using AutoMapper;

namespace ProductManagement.API.Helpers.MapperProfiles
{
    public class Base64StringToByteArrayConverter : ITypeConverter<string, byte[]>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
        {
            return System.Convert.FromBase64String(source);
        }
    }
}
