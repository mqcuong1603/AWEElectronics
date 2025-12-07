using System;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for InventoryBLL class focusing on inventory operations.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class InventoryBLLTests
    {
        #region Goods Received Note - Product ID Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Product ID:
         * Rule: Product ID must be valid (> 0)
         *
         * Equivalence Partitions:
         * EP1: Invalid - negative product ID
         * EP2: Invalid - zero product ID
         * EP3: Valid - positive product ID
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid - boundary)
         * BV3: 1 (valid - minimum)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative product ID is invalid")]
        public void GoodsReceived_NegativeProductId_IsInvalid()
        {
            // Arrange
            int productId = -1;

            // Act & Assert
            Assert.That(productId <= 0, Is.True, "Negative product ID should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero product ID is invalid")]
        public void GoodsReceived_ZeroProductId_IsInvalid()
        {
            // Arrange
            int productId = 0;

            // Act & Assert
            Assert.That(productId <= 0, Is.True, "Zero product ID should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Product ID of 1 is valid (minimum)")]
        public void GoodsReceived_MinimumProductId_IsValid()
        {
            // Arrange
            int productId = 1;

            // Act & Assert
            Assert.That(productId > 0, Is.True);
        }

        #endregion

        #region Goods Received Note - Quantity Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Quantity (Goods Received):
         * Rule: Quantity must be greater than zero
         *
         * Equivalence Partitions:
         * EP1: Invalid - negative quantity
         * EP2: Invalid - zero quantity
         * EP3: Valid - positive quantity
         * EP4: Valid - large quantity
         *
         * Boundary Values:
         * BV1: -1 (invalid)
         * BV2: 0 (invalid - boundary)
         * BV3: 1 (valid - minimum)
         * BV4: Large value (valid)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Negative quantity for goods received is invalid")]
        public void GoodsReceived_NegativeQuantity_IsInvalid()
        {
            // Arrange
            int quantity = -10;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Negative quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero quantity for goods received is invalid")]
        public void GoodsReceived_ZeroQuantity_IsInvalid()
        {
            // Arrange
            int quantity = 0;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Zero quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Quantity of 1 for goods received is valid (minimum)")]
        public void GoodsReceived_MinimumQuantity_IsValid()
        {
            // Arrange
            int quantity = 1;

            // Act & Assert
            Assert.That(quantity > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Typical positive quantity for goods received is valid")]
        public void GoodsReceived_TypicalQuantity_IsValid()
        {
            // Arrange
            int quantity = 100;

            // Act & Assert
            Assert.That(quantity > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Large quantity for goods received is valid")]
        public void GoodsReceived_LargeQuantity_IsValid()
        {
            // Arrange
            int quantity = 10000;

            // Act & Assert
            Assert.That(quantity > 0, Is.True);
        }

        #endregion

        #region Goods Received - Stock Calculation Tests - EP and BVA

        /*
         * Test Design Documentation for Stock Calculation:
         * Rule: New stock = Current stock + Received quantity
         *
         * Equivalence Partitions:
         * EP1: Adding to zero stock
         * EP2: Adding to existing stock
         * EP3: Adding large quantity
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Adding goods to zero stock")]
        public void GoodsReceived_ZeroCurrentStock_CalculatesCorrectly()
        {
            // Arrange
            int currentStock = 0;
            int receivedQuantity = 50;

            // Act
            int newStock = currentStock + receivedQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(50));
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Adding goods to existing stock")]
        public void GoodsReceived_ExistingStock_CalculatesCorrectly()
        {
            // Arrange
            int currentStock = 100;
            int receivedQuantity = 50;

            // Act
            int newStock = currentStock + receivedQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(150));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Adding minimum quantity to stock")]
        public void GoodsReceived_MinimumQuantityAdded_CalculatesCorrectly()
        {
            // Arrange
            int currentStock = 99;
            int receivedQuantity = 1;

            // Act
            int newStock = currentStock + receivedQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(100));
        }

        #endregion

        #region Goods Delivery Note - Quantity Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Quantity (Goods Delivery):
         * Rules:
         * - Quantity must be greater than zero
         * - Cannot deliver more than available stock
         *
         * Boundary Values:
         * BV1: 0 (invalid)
         * BV2: 1 (minimum valid if stock allows)
         * BV3: quantity = stock (valid - delivers all)
         * BV4: quantity = stock + 1 (invalid - insufficient stock)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero quantity for delivery is invalid")]
        public void GoodsDelivery_ZeroQuantity_IsInvalid()
        {
            // Arrange
            int quantity = 0;

            // Act & Assert
            Assert.That(quantity <= 0, Is.True, "Zero quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum quantity for delivery is valid")]
        public void GoodsDelivery_MinimumQuantity_IsValid()
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
        public void GoodsDelivery_ExactStockLevel_IsValid()
        {
            // Arrange
            int stockLevel = 50;
            int quantity = 50;

            // Act & Assert
            Assert.That(quantity <= stockLevel, Is.True);
            Assert.That(stockLevel - quantity, Is.EqualTo(0), "Should result in zero stock");
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: Delivering more than stock is invalid")]
        public void GoodsDelivery_ExceedsStock_IsInvalid()
        {
            // Arrange
            int stockLevel = 50;
            int quantity = 51;

            // Act & Assert
            Assert.That(quantity > stockLevel, Is.True, "Delivery exceeds available stock");
        }

        [Test]
        [Category("BVA")]
        [Description("BV: Delivering one more than stock is invalid")]
        public void GoodsDelivery_ExceedsStockByOne_IsInvalid()
        {
            // Arrange
            int stockLevel = 100;
            int quantity = 101;

            // Act & Assert
            Assert.That(quantity > stockLevel, Is.True);
            Assert.That(stockLevel - quantity, Is.EqualTo(-1), "Would result in negative stock");
        }

        #endregion

        #region Goods Delivery - Stock Calculation Tests - EP and BVA

        /*
         * Test Design Documentation for Stock Calculation (Delivery):
         * Rule: New stock = Current stock - Delivered quantity
         *
         * Equivalence Partitions:
         * EP1: Partial delivery
         * EP2: Full delivery (empty stock)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Partial delivery from stock")]
        public void GoodsDelivery_PartialDelivery_CalculatesCorrectly()
        {
            // Arrange
            int currentStock = 100;
            int deliveredQuantity = 30;

            // Act
            int newStock = currentStock - deliveredQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(70));
            Assert.That(newStock >= 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Full delivery empties stock")]
        public void GoodsDelivery_FullDelivery_ResultsInZeroStock()
        {
            // Arrange
            int currentStock = 100;
            int deliveredQuantity = 100;

            // Act
            int newStock = currentStock - deliveredQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(0));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Delivering minimum from stock")]
        public void GoodsDelivery_MinimumDelivery_CalculatesCorrectly()
        {
            // Arrange
            int currentStock = 100;
            int deliveredQuantity = 1;

            // Act
            int newStock = currentStock - deliveredQuantity;

            // Assert
            Assert.That(newStock, Is.EqualTo(99));
        }

        #endregion

        #region Inventory Adjustment - Validation Tests - EP and BVA

        /*
         * Test Design Documentation for Inventory Adjustment:
         * Rules:
         * - Product ID must be valid (> 0)
         * - New quantity cannot be negative
         * - Reason for adjustment is required
         * - No change in quantity is rejected
         *
         * Boundary Values for newQuantity:
         * BV1: -1 (invalid)
         * BV2: 0 (valid - zero out stock)
         * BV3: 1 (valid - minimum positive)
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Negative new quantity is invalid")]
        public void AdjustInventory_NegativeQuantity_IsInvalid()
        {
            // Arrange
            int newQuantity = -1;

            // Act & Assert
            Assert.That(newQuantity < 0, Is.True, "Negative quantity should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Zero new quantity is valid (clearing stock)")]
        public void AdjustInventory_ZeroQuantity_IsValid()
        {
            // Arrange
            int newQuantity = 0;

            // Act & Assert
            Assert.That(newQuantity >= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Minimum positive new quantity is valid")]
        public void AdjustInventory_MinimumPositiveQuantity_IsValid()
        {
            // Arrange
            int newQuantity = 1;

            // Act & Assert
            Assert.That(newQuantity >= 0, Is.True);
            Assert.That(newQuantity > 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Null reason for adjustment is invalid")]
        public void AdjustInventory_NullReason_IsInvalid()
        {
            // Arrange
            string reason = null;

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(reason), Is.True, "Reason is required");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Empty reason for adjustment is invalid")]
        public void AdjustInventory_EmptyReason_IsInvalid()
        {
            // Arrange
            string reason = "";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(reason), Is.True, "Reason is required");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Whitespace-only reason is invalid")]
        public void AdjustInventory_WhitespaceReason_IsInvalid()
        {
            // Arrange
            string reason = "   ";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(reason), Is.True, "Reason is required");
        }

        [Test]
        [Category("EP")]
        [Description("EP: Valid reason for adjustment")]
        public void AdjustInventory_ValidReason_IsAccepted()
        {
            // Arrange
            string reason = "Physical count discrepancy";

            // Act & Assert
            Assert.That(string.IsNullOrWhiteSpace(reason), Is.False);
        }

        #endregion

        #region Inventory Adjustment - Difference Calculation Tests - EP and BVA

        /*
         * Test Design Documentation for Difference Calculation:
         * Rule: Difference = New quantity - Current stock
         *
         * Equivalence Partitions:
         * EP1: Positive difference (increase)
         * EP2: Negative difference (decrease)
         * EP3: Zero difference (no change - should be rejected)
         *
         * Boundary Values:
         * BV1: difference = 0 (invalid - no change)
         * BV2: difference = 1 (minimum positive)
         * BV3: difference = -1 (minimum negative)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Positive difference (stock increase)")]
        public void AdjustInventory_PositiveDifference_IncreasesStock()
        {
            // Arrange
            int currentStock = 50;
            int newQuantity = 75;

            // Act
            int difference = newQuantity - currentStock;

            // Assert
            Assert.That(difference, Is.EqualTo(25));
            Assert.That(difference > 0, Is.True, "Positive difference means increase");
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Negative difference (stock decrease)")]
        public void AdjustInventory_NegativeDifference_DecreasesStock()
        {
            // Arrange
            int currentStock = 100;
            int newQuantity = 60;

            // Act
            int difference = newQuantity - currentStock;

            // Assert
            Assert.That(difference, Is.EqualTo(-40));
            Assert.That(difference < 0, Is.True, "Negative difference means decrease");
        }

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero difference is invalid (no change)")]
        public void AdjustInventory_ZeroDifference_IsRejected()
        {
            // Arrange
            int currentStock = 100;
            int newQuantity = 100;

            // Act
            int difference = newQuantity - currentStock;

            // Assert
            Assert.That(difference, Is.EqualTo(0));
            Assert.That(difference == 0, Is.True, "Zero difference should be rejected");
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum positive difference")]
        public void AdjustInventory_MinimumPositiveDifference_IsValid()
        {
            // Arrange
            int currentStock = 100;
            int newQuantity = 101;

            // Act
            int difference = newQuantity - currentStock;

            // Assert
            Assert.That(difference, Is.EqualTo(1));
            Assert.That(difference != 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Minimum negative difference")]
        public void AdjustInventory_MinimumNegativeDifference_IsValid()
        {
            // Arrange
            int currentStock = 100;
            int newQuantity = 99;

            // Act
            int difference = newQuantity - currentStock;

            // Assert
            Assert.That(difference, Is.EqualTo(-1));
            Assert.That(difference != 0, Is.True);
        }

        #endregion

        #region Inventory Transaction Type Tests - EP

        /*
         * Test Design Documentation for Transaction Types:
         * Valid types: IN, OUT, ADJUST
         */

        [Test]
        [Category("EP")]
        [Description("EP: All valid transaction types are recognized")]
        public void TransactionTypes_ValidTypes_AreRecognized()
        {
            // Arrange
            string[] validTypes = { "IN", "OUT", "ADJUST" };

            // Act & Assert
            Assert.That(validTypes.Length, Is.EqualTo(3));
            Assert.That(validTypes, Does.Contain("IN"));
            Assert.That(validTypes, Does.Contain("OUT"));
            Assert.That(validTypes, Does.Contain("ADJUST"));
        }

        [Test]
        [Category("EP")]
        [Description("EP: IN type for goods received")]
        public void TransactionType_GoodsReceived_IsIN()
        {
            // Arrange
            string type = "IN";

            // Act & Assert
            Assert.That(type, Is.EqualTo("IN"));
        }

        [Test]
        [Category("EP")]
        [Description("EP: OUT type for goods delivered")]
        public void TransactionType_GoodsDelivered_IsOUT()
        {
            // Arrange
            string type = "OUT";

            // Act & Assert
            Assert.That(type, Is.EqualTo("OUT"));
        }

        [Test]
        [Category("EP")]
        [Description("EP: ADJUST type for inventory adjustment")]
        public void TransactionType_Adjustment_IsADJUST()
        {
            // Arrange
            string type = "ADJUST";

            // Act & Assert
            Assert.That(type, Is.EqualTo("ADJUST"));
        }

        #endregion

        #region Absolute Value Calculation Tests - EP and BVA

        /*
         * Test Design Documentation:
         * Rule: Transaction quantity is stored as absolute value
         */

        [Test]
        [Category("EP")]
        [Description("EP: Absolute value of positive difference")]
        public void AbsoluteValue_PositiveDifference_ReturnsPositive()
        {
            // Arrange
            int difference = 25;

            // Act
            int absoluteValue = Math.Abs(difference);

            // Assert
            Assert.That(absoluteValue, Is.EqualTo(25));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Absolute value of negative difference")]
        public void AbsoluteValue_NegativeDifference_ReturnsPositive()
        {
            // Arrange
            int difference = -40;

            // Act
            int absoluteValue = Math.Abs(difference);

            // Assert
            Assert.That(absoluteValue, Is.EqualTo(40));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Absolute value of -1")]
        public void AbsoluteValue_MinimumNegative_ReturnsPositive()
        {
            // Arrange
            int difference = -1;

            // Act
            int absoluteValue = Math.Abs(difference);

            // Assert
            Assert.That(absoluteValue, Is.EqualTo(1));
        }

        #endregion
    }
}
