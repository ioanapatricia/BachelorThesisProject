using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;

namespace ProductManagement.API.Helpers
{
    public class DbInitializer
    {
        public static void SeedData(DataContext context)
        {
            if (context.Products.Any()) 
                return;

            //Images
            var imageTypeData = File.ReadAllText("Helpers/DataForSeed/Images.json");
            var images = JsonConvert.DeserializeObject<List<Image>>(imageTypeData);

            foreach (var image in images)
            {
                image.Data = File.ReadAllBytes("./Helpers/DataForSeed/ProductImages/" + image.Name);
                var nameWithoutExtension = image.Name.Substring(0, image.Name.IndexOf(".", StringComparison.InvariantCultureIgnoreCase));
                image.Name = nameWithoutExtension;
                var extensionWithoutDot = image.Extension.Substring(image.Extension.IndexOf('.') + 1);
                image.Extension = extensionWithoutDot;
                context.Add(image);
            }

            context.SaveChanges();

            //ProductCategory
            var productCategoriesData = File.ReadAllText("Helpers/DataForSeed/ProductCategories.json");
            var productCategoriesDynamic = JsonConvert.DeserializeObject<List<dynamic>>(productCategoriesData);
                    
            foreach (var productCategoryDynamic in productCategoriesDynamic)
            {       
                string logoName = productCategoryDynamic.LogoName;
                string bannerName = productCategoryDynamic.BannerName;

                var logo = context.Images
                    .FirstOrDefault(img => img.Name == logoName);

                var banner = context.Images
                    .FirstOrDefault(img => img.Name == bannerName);

                var productCategory = new ProductCategory
                {               
                    Name = productCategoryDynamic.Name,
                    Logo = logo,
                    Banner = banner,
                    SortingOrderOnWebpage = productCategoryDynamic.SortingOrderOnWebpage
                };
                context.Add(productCategory);
            }

            //WeightType
            var weightTypeData = File.ReadAllText("Helpers/DataForSeed/WeightTypes.json");
            var weightTypes = JsonConvert.DeserializeObject<List<WeightType>>(weightTypeData);

            foreach (var weightType in weightTypes)
            {
                context.Add(weightType);
            }


            context.SaveChanges();

            //Products
            var productData = File.ReadAllText("Helpers/DataForSeed/Products.json");
            var dynamicProductList = JsonConvert.DeserializeObject<List<dynamic>>(productData);
            var productVariantData = File.ReadAllText("Helpers/DataForSeed/ProductVariants.json");
            var dynamicProductVariantsList = JsonConvert.DeserializeObject<List<dynamic>>(productVariantData);

            foreach (var dynamicProduct in dynamicProductList)
            {
                var product = new Product();

                product.Name = dynamicProduct.Name;
                product.Description = dynamicProduct.Description;

                string productCategoryName = dynamicProduct.CategoryName;
                product.Category =
                    context.ProductCategories.FirstOrDefault(category => category.Name == productCategoryName);

                string productImageName = dynamicProduct.ImageName;
                product.Image =
                    context.Images.FirstOrDefault(image => image.Name == productImageName);

                string productWeightTypeName = dynamicProduct.WeightTypeName;
                product.WeightType = context.WeightTypes.FirstOrDefault(wt => wt.Name == productWeightTypeName);

                var dynamicVariantsForThisProduct =
                    dynamicProductVariantsList
                        .Where(x => x.BelongsToProduct.ToString() == product.Name);

                var variantsForThisProduct = dynamicVariantsForThisProduct
                    .Select(dynamicVariant => new ProductVariant {Name = dynamicVariant.Name, Price = dynamicVariant.Price, Weight = dynamicVariant.Weight})
                    .ToList();

                product.Variants = variantsForThisProduct;

                context.Add(product);
            }
            context.SaveChanges();
        }
    }
}