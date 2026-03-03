using System.Collections.Generic;
using System.Linq;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.Tests.TestData
{
    public class CategoryTestData
    {
        private IEnumerable<CategoryWithProductsDto> _categoriesWithProducts;
        private IEnumerable<ProductCategory> _categories;
        private readonly ImageTestData _imageTestData;

        public CategoryTestData()
        {
            _imageTestData = new ImageTestData();
            SetCategoryWithProductsTestData();
            SetCategoryTestData();
        }

        public ProductCategory GetCategory(int id)
            => _categories.FirstOrDefault(c => c.Id == id);
        public IEnumerable<CategoryWithProductsDto> GetCategoryWithProducts() => _categoriesWithProducts;
        public IEnumerable<ProductCategory> GetProductCategories() => _categories;

        public ProductCategoryForCreateOrUpdateDto GetCategoryForCreateOrUpdateDto()
        {
            return new ProductCategoryForCreateOrUpdateDto
            {
                Name = "category1",
                SortingOrderOnWebpage = 2,
                Banner = new ImageForCreateDto
                {
                    Data = _imageTestData.GetImageAsBase64String(),
                    Name = "banner1"
                },
                Logo = new ImageForCreateDto
                {
                    Data = _imageTestData.GetImageAsBase64String(),
                    Name = "logo1"
                }
            };
        }

        private void SetCategoryTestData()  
        {
            _categories = new List<ProductCategory>()
            {
                new ProductCategory
                {
                    Id = 1,
                    Name = "category1",
                    SortingOrderOnWebpage = 1,
                    LogoId = 1,
                    Logo = new Image
                    {
                        Id = 1,
                        Data = new byte[64],
                        Name = "logo1",
                    },
                    BannerId = 2,
                    Banner = new Image
                    {
                        Id = 2,
                        Data = new byte[64],
                        Name = "banner2",
                    },
                },
                new ProductCategory
                {
                    Id = 2,
                    Name = "category2",
                    SortingOrderOnWebpage = 2,
                    LogoId = 4,
                    Logo = new Image
                    {
                        Id = 4,
                        Data = new byte[64],
                        Name = "logo4",
                    },
                    BannerId = 5,
                    Banner = new Image
                    {
                        Id = 5,
                        Data = new byte[64],
                        Name = "banner5",
                    },
                }
            };
        }

        private void SetCategoryWithProductsTestData()
        {
            _categoriesWithProducts = new List<CategoryWithProductsDto>
            {
                  new CategoryWithProductsDto
                  {
                      Name = "Pizza",
                      SortingOrderOnWebpage = 1,
                      Banner = new ImageForGetDto
                      {
                          Id = 1,
                          Name = "banner1"
                      },
                      Logo = new ImageForGetDto
                      {
                          Id = 1,
                          Name = "logo1"
                      },
                      Products = new List<ProductForGetDto>
                      {
                          new ProductForGetDto
                          {
                              Id = 1,
                              Name = "product1",
                              Description = "description1",
                              Category = new ProductCategoryDto
                              {
                                  Id = 1,
                                  Name = "Pizza"
                              },
                              Image = new ImageForGetDto
                              {
                                  Id = 3,
                                  Name = "product3",
                              },
                              WeightType = new WeightTypeDto()
                              {
                                  Id = 1,
                                  Name = "weightType1"
                              },
                              Variants = new List<ProductVariantForGetDto>
                              {
                                  new ProductVariantForGetDto
                                  {
                                      Id = 1,
                                      Name = "productVariant1",
                                      Price = 0.1m,
                                      Weight = 0.1f,
                                      SalePercentage = null,
                                  }
                              }
                          }
                      }
                  },
                  new CategoryWithProductsDto
                  {
                      Name = "Pasta",
                      SortingOrderOnWebpage = 2,
                      Banner = new ImageForGetDto
                      {
                          Id = 2,
                          Name = "banner2"
                      },
                      Logo = new ImageForGetDto
                      {
                          Id = 3,
                          Name = "logo3"
                      },
                      Products = new List<ProductForGetDto>
                      {
                          new ProductForGetDto
                          {
                              Id = 2,
                              Name = "product2",
                              Description = "description2",
                              Category = new ProductCategoryDto
                              {
                                  Id = 2,
                                  Name = "Pasta"
                              },
                              Image = new ImageForGetDto
                              {
                                  Id = 3,
                                  Name = "product3",
                              },
                              WeightType = new WeightTypeDto
                              {
                                  Id = 1,
                                  Name = "weightType1"
                              },
                              Variants = new List<ProductVariantForGetDto>
                              {
                                  new ProductVariantForGetDto
                                  {
                                      Id = 1,
                                      Name = "productVariant1",
                                      Price = 0.1m,
                                      Weight = 0.1f,
                                      SalePercentage = null,
                                  }
                              }
                          }
                      },
                  }
            };
        }



        
    }
}
