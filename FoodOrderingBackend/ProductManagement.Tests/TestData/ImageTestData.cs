using System.IO;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.Tests.TestData
{
    class ImageTestData
    {
        private byte[] _testImageData;
        private string _testImageDataString;
        public ImageTestData()
        {
            SetImage();
        }

        public byte[] GetImageAsByteArray()
            => _testImageData;

        public string GetImageAsBase64String()
            => _testImageDataString;

        public ImageForCreateDto GetImageForCreateDto()
        {
            return new ImageForCreateDto
            {
                Name = "name1",
                Data = _testImageDataString
            };
        }


        public Image GetImage()
        {
            return new Image
            {
                Name = "name1",
                Extension = "png",
                Data = _testImageData
            };
        }

        private void SetImage()
        {
            const string path = "../../../TestData/testImg.png";

            _testImageData = File.ReadAllBytes(path);
            _testImageDataString = System.Convert.ToBase64String(_testImageData);
        }
    }
}
