using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        public class ProductViewModelValidationTest
        {
            private readonly ITestOutputHelper output;
            private ProductViewModel product;

            public ProductViewModelValidationTest(ITestOutputHelper output)
            {
                this.output = output;
                product = new ProductViewModel();
            }

            [Fact]
            public void TestMissingName()
            {
                product.Name = null;
                product.Price = "9";
                product.Stock = "1";
                Assert.False(ValidateModel(product));
                Assert.Equal("MissingName", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotANumber()
            {
                product.Name = "Test Product";
                product.Price = "abc";
                product.Stock = "3";
                Assert.False(ValidateModel(product));
                Assert.Equal("PriceNotANumber", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotGreaterThanZero()
            {
                product.Name = "Test Product";
                product.Price = "0";
                product.Stock = "1";
                Assert.False(ValidateModel(product));
                Assert.Equal("PriceNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestQuantityNotGreaterThanZero()
            {
                product.Name = "Test Product";
                product.Price = "7";
                product.Stock = "0";
                Assert.False(ValidateModel(product));
                Assert.Equal("QuantityNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingPrice()
            {
                product.Name = "Test Product";
                product.Stock = "1";
                product.Price = null;
                Assert.False(ValidateModel(product));
                Assert.Equal("MissingPrice", GetFirstErrorMessage(product));
            }

            private bool ValidateModel(object model)
            {
                var validationResults = new List<ValidationResult>();
                var ctx = new ValidationContext(model, null, null);

                return Validator.TryValidateObject(model, ctx, validationResults, true);
            }

            private string GetFirstErrorMessage(object model)
            {
                var validationResults = new List<ValidationResult>();
                var ctx = new ValidationContext(model, null, null);
                Validator.TryValidateObject(model, ctx, validationResults, true);

                foreach (var result in validationResults)
                {
                    output.WriteLine(result.ErrorMessage);
                }

                return validationResults.Count > 0 ? validationResults[0].ErrorMessage : null;
            }

        }
    }
}
