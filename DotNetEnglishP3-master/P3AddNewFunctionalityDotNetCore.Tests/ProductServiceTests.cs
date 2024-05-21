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
                // Arrange
                product.Name = null;
                product.Price = "9.9";
                product.Stock = "1";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("ErrorMissingName", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotANumber()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "abc";
                product.Stock = "3";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("PriceNotANumber", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestPriceNotGreaterThanZero()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "0";
                product.Stock = "1";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("PriceNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestQuantityNotGreaterThanZero()
            {
                // Arrange
                product.Name = "Test Product";
                product.Price = "7";
                product.Stock = "0";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("QuantityNotGreaterThanZero", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingPrice()
            {
                // Arrange
                product.Name = "Test Product";
                product.Stock = "1";
                product.Price = null;

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("ErrorMissingPrice", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestMissingQuantity()
            {
                // Arrange
                product.Name = "Test Product";
                product.Stock = null;
                product.Price = "42";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("ErrorMissingQuantity", GetFirstErrorMessage(product));
            }

            [Fact]
            public void TestQuantityNotAnInteger()
            {
                // Arrange
                product.Name = "Test Product";
                product.Stock = "1.5";
                product.Price = "19";

                // Act
                var isValid = ValidateModel(product);

                // Assert
                Assert.False(isValid);
                Assert.Equal("QuantityNotAnInteger", GetFirstErrorMessage(product));
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
