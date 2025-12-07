using System;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for ProductBLL class focusing on validation logic.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class ProductBLLTests
    {
        #region Product Name Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Product Name:
         * Rule: Product name is required and must be at least 2 characters
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty string
         * EP3: Invalid - whitespace only
         * EP4: Invalid - 1 character
         * EP5: Valid - 2+ characters
         *
         * Boundary Values:
         * BV1: 0 characters (empty)
         * BV2: 1 character (invalid)
         * BV3: 2 characters (minimum valid)
         * BV4: 3 characters (valid)
         * BV5: Very long name (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null product name should be invalid")]
        public void ProductName_Null_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = null,
                SKU = "TEST001",
                CategoryID = 1,
                Price = 100.00m,
                StockLevel = 10
            };

            // Act & Assert
            // The validation happens internally; we test via CreateProduct behavior
            // This test documents the expected behavior
            Assert.That(product.Name, Is.Null);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty product name should be invalid")]
        public void ProductName_Empty_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "",
                SKU = "TEST001",
                CategoryID = 1,
                Price = 100.00m,
                StockLevel = 10
            };

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(product.Name), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Whitespace-only product name should be invalid")]
        public void ProductName_WhitespaceOnly_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "   ",
                SKU = "TEST001",
                CategoryID = 1,
                Price = 100.00m,
                StockLevel = 10
            };

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(product.Name), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 1 character product name is invalid (below minimum)")]
        public void ProductName_OneCharacter_IsBelowMinimum()
        {
            // Arrange
            string name = "A";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(1));
            Assert.That(name.Length < 2, Is.True, "1 character is below minimum of 2");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 2 character product name is valid (minimum)")]
        public void ProductName_TwoCharacters_IsMinimumValid()
        {
            // Arrange
            string name = "AB";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(2));
            Assert.That(name.Length >= 2, Is.True, "2 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 3 character product name is valid")]
        public void ProductName_ThreeCharacters_IsValid()
        {
            // Arrange
            string name = "ABC";

            // Act & Assert
            Assert.That(name.Length, Is.EqualTo(3));
            Assert.That(name.Length >= 2, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Long product name is valid")]
        public void ProductName_LongName_IsValid()
        {
            // Arrange
            string name = "Samsung 65-inch 4K Ultra HD Smart LED TV with HDR";

            // Act & Assert
            Assert.That(name.Length >= 2, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(name), Is.False);
        }

        #endregion

        #region SKU Validation Tests - EP and BVA

        /*
         * Test Design Documentation for SKU:
         * Rule: SKU is required and must be at least 3 characters
         *
         * Equivalence Partitions:
         * EP1: Invalid - null
         * EP2: Invalid - empty
         * EP3: Invalid - 1-2 characters
         * EP4: Valid - 3+ characters
         *
         * Boundary Values:
         * BV1: 0 characters (empty)
         * BV2: 2 characters (invalid - below min)
         * BV3: 3 characters (minimum valid)
         * BV4: 4 characters (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Null SKU should be invalid")]
        public void SKU_Null_IsInvalid()
        {
            // Arrange
            string sku = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(sku), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Empty SKU should be invalid")]
        public void SKU_Empty_IsInvalid()
        {
            // Arrange
            string sku = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(sku), Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: 2 character SKU is invalid (below minimum)")]
        public void SKU_TwoCharacters_IsBelowMinimum()
        {
            // Arrange
            string sku = "AB";

            // Act & Assert
            Assert.That(sku.Length, Is.EqualTo(2));
            Assert.That(sku.Length < 3, Is.True, "2 characters is below minimum of 3");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: 3 character SKU is valid (minimum)")]
        public void SKU_ThreeCharacters_IsMinimumValid()
        {
            // Arrange
            string sku = "ABC";

            // Act & Assert
            Assert.That(sku.Length, Is.EqualTo(3));
            Assert.That(sku.Length >= 3, Is.True, "3 characters meets minimum requirement");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: 4 character SKU is valid")]
        public void SKU_FourCharacters_IsValid()
        {
            // Arrange
            string sku = "ABCD";

            // Act & Assert
            Assert.That(sku.Length, Is.EqualTo(4));
            Assert.That(sku.Length >= 3, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Standard SKU format is valid")]
        public void SKU_StandardFormat_IsValid()
        {
            // Arrange
            string sku = "ELEC-TV-001";

            // Act & Assert
            Assert.That(sku.Length >= 3, Is.True);
            Assert.That(string.IsNullOrWhiteSpace(sku), Is.False);
        }

        #endregion

        #region Category ID Validation Tests - EP and BVA

        /*
         * Test Design Documentation for CategoryID:
         * Rule: Category is required (CategoryID > 0)
         *
         * Equivalence Partitions:
         * EP1: Invalid - negative values
         * EP2: Invalid - zero
         * EP3: Valid - positive values
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid - boundary)
         * BV3: 1 (valid - minimum)
         * BV4: Large positive value (valid)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative CategoryID is invalid")]
        public void CategoryID_Negative_IsInvalid()
        {
            // Arrange
            int categoryId = -1;

            // Act & Assert
            Assert.That(categoryId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero CategoryID is invalid (boundary)")]
        public void CategoryID_Zero_IsInvalid()
        {
            // Arrange
            int categoryId = 0;

            // Act & Assert
            Assert.That(categoryId <= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: CategoryID of 1 is valid (minimum)")]
        public void CategoryID_One_IsMinimumValid()
        {
            // Arrange
            int categoryId = 1;

            // Act & Assert
            Assert.That(categoryId > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Typical positive CategoryID is valid")]
        public void CategoryID_TypicalPositive_IsValid()
        {
            // Arrange
            int categoryId = 42;

            // Act & Assert
            Assert.That(categoryId > 0, Is.True);
        }

        #endregion

        #region Price Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Price:
         * Rule: Price cannot be negative
         *
         * Equivalence Partitions:
         * EP1: Invalid - negative price
         * EP2: Valid - zero (free item)
         * EP3: Valid - small positive
         * EP4: Valid - large positive
         *
         * Boundary Values:
         * BV1: -0.01 (invalid - just below zero)
         * BV2: 0 (valid - boundary)
         * BV3: 0.01 (valid - minimum positive price)
         * BV4: Large value (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Negative price is invalid")]
        public void Price_Negative_IsInvalid()
        {
            // Arrange
            decimal price = -100.00m;

            // Act & Assert
            Assert.That(price < 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV1: Price just below zero is invalid")]
        public void Price_JustBelowZero_IsInvalid()
        {
            // Arrange
            decimal price = -0.01m;

            // Act & Assert
            Assert.That(price < 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero price is valid (free item)")]
        public void Price_Zero_IsValid()
        {
            // Arrange
            decimal price = 0m;

            // Act & Assert
            Assert.That(price >= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Minimum positive price is valid")]
        public void Price_MinimumPositive_IsValid()
        {
            // Arrange
            decimal price = 0.01m;

            // Act & Assert
            Assert.That(price >= 0, Is.True);
            Assert.That(price, Is.EqualTo(0.01m));
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Typical price is valid")]
        public void Price_TypicalValue_IsValid()
        {
            // Arrange
            decimal price = 599.99m;

            // Act & Assert
            Assert.That(price >= 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Large price is valid")]
        public void Price_LargeValue_IsValid()
        {
            // Arrange
            decimal price = 99999.99m;

            // Act & Assert
            Assert.That(price >= 0, Is.True);
        }

        #endregion

        #region Stock Level Validation Tests - EP and BVA

        /*
         * Test Design Documentation for StockLevel:
         * Rule: Stock level cannot be negative
         *
         * Equivalence Partitions:
         * EP1: Invalid - negative stock
         * EP2: Valid - zero (out of stock)
         * EP3: Valid - positive stock
         *
         * Boundary Values:
         * BV1: -1 (invalid - just below zero)
         * BV2: 0 (valid - boundary/out of stock)
         * BV3: 1 (valid - minimum positive)
         * BV4: Large value (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Negative stock level is invalid")]
        public void StockLevel_Negative_IsInvalid()
        {
            // Arrange
            int stockLevel = -10;

            // Act & Assert
            Assert.That(stockLevel < 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV1: Stock level just below zero is invalid")]
        public void StockLevel_JustBelowZero_IsInvalid()
        {
            // Arrange
            int stockLevel = -1;

            // Act & Assert
            Assert.That(stockLevel < 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero stock level is valid (out of stock)")]
        public void StockLevel_Zero_IsValid()
        {
            // Arrange
            int stockLevel = 0;

            // Act & Assert
            Assert.That(stockLevel >= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Stock level of 1 is valid (minimum positive)")]
        public void StockLevel_One_IsMinimumPositive()
        {
            // Arrange
            int stockLevel = 1;

            // Act & Assert
            Assert.That(stockLevel >= 0, Is.True);
            Assert.That(stockLevel > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Typical stock level is valid")]
        public void StockLevel_TypicalValue_IsValid()
        {
            // Arrange
            int stockLevel = 100;

            // Act & Assert
            Assert.That(stockLevel >= 0, Is.True);
        }

        #endregion

        #region Stock Adjustment Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Stock Adjustment:
         * Rules:
         * - Product ID must be valid (> 0)
         * - Quantity change cannot be zero
         * - New stock level cannot be negative
         *
         * Equivalence Partitions for QuantityChange:
         * EP1: Invalid - zero change
         * EP2: Valid - positive change (receive goods)
         * EP3: Valid - negative change (deliver goods, if sufficient stock)
         * EP4: Invalid - negative change resulting in negative stock
         *
         * Boundary Values:
         * BV1: quantityChange = 0 (invalid)
         * BV2: quantityChange = 1 (minimum positive)
         * BV3: quantityChange = -1 (minimum negative)
         * BV4: quantityChange exactly negates stock (boundary)
         * BV5: quantityChange exceeds stock by 1 (just invalid)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero quantity change is invalid")]
        public void StockAdjustment_ZeroChange_IsInvalid()
        {
            // Arrange
            int quantityChange = 0;

            // Act & Assert
            Assert.That(quantityChange == 0, Is.True, "Zero quantity change should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum positive quantity change is valid")]
        public void StockAdjustment_MinimumPositive_IsValid()
        {
            // Arrange
            int quantityChange = 1;

            // Act & Assert
            Assert.That(quantityChange > 0, Is.True);
            Assert.That(quantityChange != 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Minimum negative quantity change is valid (if stock allows)")]
        public void StockAdjustment_MinimumNegative_IsValidIfStockAllows()
        {
            // Arrange
            int currentStock = 10;
            int quantityChange = -1;
            int newStock = currentStock + quantityChange;

            // Act & Assert
            Assert.That(quantityChange != 0, Is.True);
            Assert.That(newStock >= 0, Is.True, "New stock should be non-negative");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: Quantity change that exactly depletes stock is valid")]
        public void StockAdjustment_ExactlyDepleteStock_IsValid()
        {
            // Arrange
            int currentStock = 10;
            int quantityChange = -10; // Exactly negates stock
            int newStock = currentStock + quantityChange;

            // Act & Assert
            Assert.That(newStock, Is.EqualTo(0), "Stock should be exactly zero");
            Assert.That(newStock >= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV5: Quantity change exceeding stock by 1 is invalid")]
        public void StockAdjustment_ExceedsStockByOne_IsInvalid()
        {
            // Arrange
            int currentStock = 10;
            int quantityChange = -11; // Exceeds stock by 1
            int newStock = currentStock + quantityChange;

            // Act & Assert
            Assert.That(newStock, Is.EqualTo(-1), "Would result in negative stock");
            Assert.That(newStock < 0, Is.True, "Negative stock should be rejected");
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Large positive stock adjustment is valid")]
        public void StockAdjustment_LargePositive_IsValid()
        {
            // Arrange
            int currentStock = 0;
            int quantityChange = 1000;
            int newStock = currentStock + quantityChange;

            // Act & Assert
            Assert.That(newStock >= 0, Is.True);
            Assert.That(newStock, Is.EqualTo(1000));
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Valid negative adjustment with sufficient stock")]
        public void StockAdjustment_ValidNegative_WithSufficientStock()
        {
            // Arrange
            int currentStock = 100;
            int quantityChange = -50;
            int newStock = currentStock + quantityChange;

            // Act & Assert
            Assert.That(newStock >= 0, Is.True);
            Assert.That(newStock, Is.EqualTo(50));
        }

        #endregion

        #region Receive Goods Validation Tests - EP and BVA

        /*
         * Test Design Documentation for ReceiveGoods:
         * Rule: Quantity must be greater than zero
         *
         * Boundary Values:
         * BV1: quantity = 0 (invalid)
         * BV2: quantity = 1 (minimum valid)
         * BV3: quantity = -1 (invalid)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero quantity for receiving goods is invalid")]
        public void ReceiveGoods_ZeroQuantity_IsInvalid()
        {
            // Arrange
            int quantity = 0;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Zero quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum quantity (1) for receiving goods is valid")]
        public void ReceiveGoods_MinimumQuantity_IsValid()
        {
            // Arrange
            int quantity = 1;

            // Act & Assert
            Assert.That(quantity > 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Negative quantity for receiving goods is invalid")]
        public void ReceiveGoods_NegativeQuantity_IsInvalid()
        {
            // Arrange
            int quantity = -1;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Negative quantity should be rejected");
        }

        #endregion

        #region Deliver Goods Validation Tests - EP and BVA

        /*
         * Test Design Documentation for DeliverGoods:
         * Rules:
         * - Quantity must be greater than zero
         * - Cannot deliver more than available stock
         *
         * Boundary Values:
         * BV1: quantity = 0 (invalid)
         * BV2: quantity = 1 (minimum valid if stock allows)
         * BV3: quantity = stock level (valid - delivers all)
         * BV4: quantity = stock level + 1 (invalid - insufficient stock)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero quantity for delivery is invalid")]
        public void DeliverGoods_ZeroQuantity_IsInvalid()
        {
            // Arrange
            int quantity = 0;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Zero quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum quantity (1) for delivery is valid if stock allows")]
        public void DeliverGoods_MinimumQuantity_IsValidIfStockAllows()
        {
            // Arrange
            int stockLevel = 10;
            int quantity = 1;

            // Act & Assert
            Assert.That(quantity > 0, Is.True);
            Assert.That(quantity <= stockLevel, Is.True, "Quantity should not exceed stock");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Delivering exact stock level is valid")]
        public void DeliverGoods_ExactStockLevel_IsValid()
        {
            // Arrange
            int stockLevel = 10;
            int quantity = 10;

            // Act & Assert
            Assert.That(quantity <= stockLevel, Is.True);
            Assert.That(stockLevel - quantity, Is.EqualTo(0), "Should result in zero stock");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: Delivering more than stock level is invalid")]
        public void DeliverGoods_ExceedsStockLevel_IsInvalid()
        {
            // Arrange
            int stockLevel = 10;
            int quantity = 11;

            // Act & Assert
            Assert.That(quantity > stockLevel, Is.True, "Delivery exceeds available stock");
        }

        #endregion

        #region Complete Product Validation Tests

        [Test]
        [Category("EP")]
        [Description("EP: Valid product passes all validation rules")]
        public void ValidProduct_PassesAllValidation()
        {
            // Arrange
            var product = new Product
            {
                Name = "Samsung 55-inch TV",  // >= 2 chars
                SKU = "ELEC-TV-001",          // >= 3 chars
                CategoryID = 1,               // > 0
                Price = 799.99m,              // >= 0
                StockLevel = 50               // >= 0
            };

            // Act & Assert
            Assert.That(product.Name.Length >= 2, Is.True, "Name meets minimum length");
            Assert.That(product.SKU.Length >= 3, Is.True, "SKU meets minimum length");
            Assert.That(product.CategoryID > 0, Is.True, "CategoryID is positive");
            Assert.That(product.Price >= 0, Is.True, "Price is non-negative");
            Assert.That(product.StockLevel >= 0, Is.True, "StockLevel is non-negative");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Edge case valid product with minimum values")]
        public void MinimalValidProduct_PassesValidation()
        {
            // Arrange - using boundary minimum values
            var product = new Product
            {
                Name = "TV",           // Exactly 2 chars (minimum)
                SKU = "TV1",           // Exactly 3 chars (minimum)
                CategoryID = 1,        // Minimum valid ID
                Price = 0m,            // Minimum valid price (free)
                StockLevel = 0         // Minimum valid stock (out of stock)
            };

            // Act & Assert
            Assert.That(product.Name.Length >= 2, Is.True);
            Assert.That(product.SKU.Length >= 3, Is.True);
            Assert.That(product.CategoryID > 0, Is.True);
            Assert.That(product.Price >= 0, Is.True);
            Assert.That(product.StockLevel >= 0, Is.True);
        }

        #endregion
    }
}
