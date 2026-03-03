using System;
using System.Collections.Generic;
using System.Linq;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Validators
{
    public class ImageValidator : ControllerBase, IImageValidator
    {
        public Result ValidateImage(ImageForCreateDto imageForCreateDto)
        {
            if (string.IsNullOrEmpty(imageForCreateDto.Name))
                 return Result.Fail("Image name is required.");

            if (imageForCreateDto.Name.Length < 3 || imageForCreateDto.Name.Length > 50)
                return Result.Fail("Image name should be between 3 and 50 characters.");

            if (string.IsNullOrEmpty(imageForCreateDto.Data) || string.IsNullOrWhiteSpace(imageForCreateDto.Data))
                return Result.Fail("File is required.");

            var givenExtension = imageForCreateDto.Name.Substring(imageForCreateDto.Name.IndexOf('.') + 1);
            if (!GetAllowedExtensions().Any(s => s.Equals(givenExtension, StringComparison.OrdinalIgnoreCase)))
                return Result.Fail("Image extension not allowed.");
                
            return Result.Ok(); 
        }

        public static IEnumerable<string> GetAllowedExtensions()
        {
            return new List<string>
            {
                "png",
                "jpg",
                "jpeg"
            };
        }
    }
}
