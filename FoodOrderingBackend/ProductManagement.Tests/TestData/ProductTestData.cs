using System.Collections.Generic;
using System.Linq;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.Tests.TestData
{
    public class ProductTestData
    {
        private IEnumerable<Product> _products;
        private readonly ImageTestData _imageTestData;

        public ProductTestData()
        {
            _imageTestData = new ImageTestData();

            SetProducts();
        }

            
        public Product GetProduct(int id)
            => _products.FirstOrDefault(p => p.Id == id);


        public IEnumerable<Product> GetProducts()
            => _products;

        public ProductForCreateOrUpdateDto GetProductForCreateOrUpdateDto()
        {
            return new ProductForCreateOrUpdateDto
            {
                Name = "product1",
                Description = "description1",
                CategoryId = 1,
                WeightTypeId = 1,
                Image = new ImageForCreateDto
                {
                    Data = _imageTestData.GetImageAsBase64String(),
                    Name = "name1"
                },
                Variants = new List<ProductVariantForCreateDto>
                {
                    new ProductVariantForCreateDto
                    {
                        Name = "productVariant1",
                        Price = 0.1m,
                        Weight = 0.1f,
                        SalePercentage = null,
                    }
                }
            };
        }

        private void SetProducts()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "product1",
                    Description = "description1",
                    CategoryId = 1,
                    Category = new ProductCategory
                    {
                        Id = 1,
                        Name = "category1",
                        SortingOrderOnWebpage = 1,
                        LogoId = 1,
                        Logo = new Image
                        {
                            Id = 1,
                            Data = _imageTestData.GetImageAsByteArray(),
                            Name = "logo1",
                        },
                        BannerId = 2,
                        Banner = new Image
                        {
                            Id = 2,
                            Data = _imageTestData.GetImageAsByteArray(),
                            Name = "banner2",
                        },

                    },
                    WeightTypeId = 1,
                    WeightType = new WeightType
                    {
                        Id = 1,
                        Name = "weightType1"
                    },
                    ImageId = 3,
                    Image = new Image
                    {
                        Id = 3,
                        Data = _imageTestData.GetImageAsByteArray(),
                        Name = "product3",
                    },
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant
                        {
                            Id = 1,
                            Name = "productVariant1",
                            Price = 0.1m,
                            Weight = 0.1f,
                            SalePercentage = null,
                            ProductId = 1
                        }
                    }

                },

                new Product
                {
                    Id = 2,
                    Name = "product2",
                    Description = "description2",
                    CategoryId = 2,
                    Category = new ProductCategory
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

                    },
                    WeightTypeId = 2,
                    WeightType = new WeightType
                    {
                        Id = 2,
                        Name = "weightType2"
                    },
                    ImageId = 6,
                    Image = new Image
                    {
                        Id = 6,
                        Data = new byte[64],
                        Name = "product6",
                    },
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant
                        {
                            Id = 2,
                            Name = "productVariant2",
                            Price = 0.1m,
                            Weight = 0.1f,
                            SalePercentage = null,
                            ProductId = 1
                        },
                        new ProductVariant
                        {
                            Id = 3,
                            Name = "productVariant3",
                            Price = 0.1m,
                            Weight = 0.1f,
                            SalePercentage = null,
                            ProductId = 1
                        }
                    }



                },

                 new Product
                {
                    Id = 3,
                    Name = "product3",
                    Description = "description3",
                    CategoryId = 3,
                    Category = new ProductCategory
                    {
                        Id = 3,
                        Name = "category3",
                        SortingOrderOnWebpage = 3,
                        LogoId = 7,
                        Logo = new Image
                        {
                            Id = 7,
                            Data = new byte[64],
                            Name = "logo7",
                        },
                        BannerId = 8,
                        Banner = new Image
                        {
                            Id = 8,
                            Data = new byte[64],
                            Name = "banner8",
                        },

                    },
                    WeightTypeId = 3,
                    WeightType = new WeightType
                    {
                        Id = 3,
                        Name = "weightType3"
                    },
                    ImageId = 9,
                    Image = new Image
                    {
                        Id = 9,
                        Data = new byte[64],
                        Name = "product9",
                    },
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant
                        {
                            Id = 4,
                            Name = "productVariant4",
                            Price = 0.1m,
                            Weight = 0.1f,
                            SalePercentage = null,
                            ProductId = 1
                        }
                    }
                }
             };
        }


    }
}
