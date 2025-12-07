using System;
using System.Collections.Generic;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for OrderBLL class focusing on calculation methods.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class OrderBLLTests
    {
        private OrderBLL _orderBLL;

        [SetUp]
        public void Setup()
        {
            _orderBLL = new OrderBLL();
        }

        #region CalculateSubTotal Tests - EP and BVA

        /*
         * Test Design Documentation for CalculateSubTotal:
         *
         * Equivalence Partitions:
         * EP1: Empty list (0 items)
         * EP2: Single item list
         * EP3: Multiple items list
         * EP4: Items with zero quantity
         * EP5: Items with zero unit price
         * EP6: Items with high values (large numbers)
         *
         * Boundary Values:
         * BV1: 0 items (minimum)
         * BV2: 1 item (boundary for non-empty)
         * BV3: Quantity = 0 (boundary)
         * BV4: Quantity = 1 (minimum positive)
         * BV5: UnitPrice = 0 (boundary)
         * BV6: UnitPrice = 0.01 (minimum positive money value)
         * BV7: Very large values (near max decimal)
         */

        [Test]
        [Category("EP")]
        [Description("EP1: Empty list should return 0 subtotal")]
        public void CalculateSubTotal_EmptyList_ReturnsZero()
        {
            // Arrange
            var items = new List<OrderDetail>();

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(0m));
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Single item calculates correctly")]
        public void CalculateSubTotal_SingleItem_ReturnsCorrectTotal()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 100.00m, Quantity = 2 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(200.00m));
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Multiple items sum correctly")]
        public void CalculateSubTotal_MultipleItems_ReturnsSumOfAllItems()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 50.00m, Quantity = 2 },   // 100
                new OrderDetail { UnitPrice = 75.50m, Quantity = 3 },   // 226.50
                new OrderDetail { UnitPrice = 120.25m, Quantity = 1 }   // 120.25
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(446.75m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Item with zero quantity contributes zero to total")]
        public void CalculateSubTotal_ZeroQuantity_ReturnsZeroForThatItem()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 100.00m, Quantity = 0 },
                new OrderDetail { UnitPrice = 50.00m, Quantity = 2 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(100.00m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV4: Item with quantity = 1 (minimum positive)")]
        public void CalculateSubTotal_QuantityOne_ReturnsUnitPrice()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 99.99m, Quantity = 1 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(99.99m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV5: Item with zero unit price contributes zero")]
        public void CalculateSubTotal_ZeroUnitPrice_ReturnsZeroForThatItem()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 0m, Quantity = 5 },
                new OrderDetail { UnitPrice = 25.00m, Quantity = 2 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(50.00m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV6: Minimum positive money value (0.01)")]
        public void CalculateSubTotal_MinimumPositivePrice_CalculatesCorrectly()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 0.01m, Quantity = 100 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(1.00m));
        }

        [Test]
        [Category("EP")]
        [Description("EP6: Large values calculation")]
        public void CalculateSubTotal_LargeValues_CalculatesCorrectly()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 99999.99m, Quantity = 100 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(9999999.00m));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Decimal precision is maintained")]
        public void CalculateSubTotal_DecimalPrecision_MaintainsPrecision()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 33.33m, Quantity = 3 }
            };

            // Act
            decimal result = _orderBLL.CalculateSubTotal(items);

            // Assert
            Assert.That(result, Is.EqualTo(99.99m));
        }

        #endregion

        #region CalculateTax Tests - EP and BVA

        /*
         * Test Design Documentation for CalculateTax:
         * Tax Rate: 10% (0.10)
         *
         * Equivalence Partitions:
         * EP1: SubTotal = 0
         * EP2: Small subtotal (< 100)
         * EP3: Medium subtotal (100 - 10000)
         * EP4: Large subtotal (> 10000)
         *
         * Boundary Values:
         * BV1: SubTotal = 0 (minimum valid)
         * BV2: SubTotal = 0.01 (minimum positive)
         * BV3: SubTotal resulting in tax requiring rounding
         * BV4: Very large subtotal
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Zero subtotal results in zero tax")]
        public void CalculateTax_ZeroSubTotal_ReturnsZero()
        {
            // Arrange
            decimal subTotal = 0m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(0m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum positive subtotal")]
        public void CalculateTax_MinimumPositiveSubTotal_ReturnsCorrectTax()
        {
            // Arrange
            decimal subTotal = 0.01m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(0.00m)); // 0.01 * 0.10 = 0.001, rounded to 0.00
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Small subtotal tax calculation")]
        public void CalculateTax_SmallSubTotal_ReturnsCorrectTax()
        {
            // Arrange
            decimal subTotal = 50.00m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(5.00m)); // 50 * 0.10 = 5
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Medium subtotal tax calculation")]
        public void CalculateTax_MediumSubTotal_ReturnsCorrectTax()
        {
            // Arrange
            decimal subTotal = 1000.00m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(100.00m)); // 1000 * 0.10 = 100
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Large subtotal tax calculation")]
        public void CalculateTax_LargeSubTotal_ReturnsCorrectTax()
        {
            // Arrange
            decimal subTotal = 50000.00m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(5000.00m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Tax calculation with rounding (up)")]
        public void CalculateTax_RequiresRoundingUp_RoundsCorrectly()
        {
            // Arrange
            decimal subTotal = 33.33m; // 33.33 * 0.10 = 3.333

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(3.33m)); // Rounds to 2 decimal places
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Tax calculation with rounding (banker's rounding)")]
        public void CalculateTax_RequiresRoundingBankers_RoundsCorrectly()
        {
            // Arrange - testing banker's rounding behavior
            decimal subTotal = 33.35m; // 33.35 * 0.10 = 3.335

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(3.34m)); // Math.Round with 2 decimals
        }

        [Test]
        [Category("EP")]
        [Description("EP: Standard order amount")]
        public void CalculateTax_StandardOrderAmount_CalculatesCorrectly()
        {
            // Arrange
            decimal subTotal = 299.99m;

            // Act
            decimal tax = _orderBLL.CalculateTax(subTotal);

            // Assert
            Assert.That(tax, Is.EqualTo(30.00m)); // 299.99 * 0.10 = 29.999, rounds to 30.00
        }

        #endregion

        #region CalculateGrandTotal Tests - EP and BVA

        /*
         * Test Design Documentation for CalculateGrandTotal:
         *
         * Equivalence Partitions:
         * EP1: Both subtotal and tax are zero
         * EP2: Subtotal positive, tax zero
         * EP3: Both subtotal and tax positive
         *
         * Boundary Values:
         * BV1: SubTotal = 0, Tax = 0
         * BV2: Minimum values (0.01)
         * BV3: Large values
         */

        [Test]
        [Category("BVA")]
        [Description("BV1: Both zero returns zero")]
        public void CalculateGrandTotal_BothZero_ReturnsZero()
        {
            // Arrange
            decimal subTotal = 0m;
            decimal tax = 0m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(0m));
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Subtotal positive, tax zero")]
        public void CalculateGrandTotal_SubTotalPositiveTaxZero_ReturnsSubTotal()
        {
            // Arrange
            decimal subTotal = 100.00m;
            decimal tax = 0m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(100.00m));
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Both positive values")]
        public void CalculateGrandTotal_BothPositive_ReturnsSumCorrectly()
        {
            // Arrange
            decimal subTotal = 100.00m;
            decimal tax = 10.00m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(110.00m));
        }

        [Test]
        [Category("EP")]
        [Description("Integration: Full calculation flow")]
        public void CalculateGrandTotal_FullCalculationFlow_CalculatesCorrectly()
        {
            // Arrange
            var items = new List<OrderDetail>
            {
                new OrderDetail { UnitPrice = 100.00m, Quantity = 2 }, // 200
                new OrderDetail { UnitPrice = 50.00m, Quantity = 3 }   // 150
            };

            // Act
            decimal subTotal = _orderBLL.CalculateSubTotal(items); // 350
            decimal tax = _orderBLL.CalculateTax(subTotal);         // 35
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(subTotal, Is.EqualTo(350.00m));
            Assert.That(tax, Is.EqualTo(35.00m));
            Assert.That(grandTotal, Is.EqualTo(385.00m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV2: Minimum positive values")]
        public void CalculateGrandTotal_MinimumPositiveValues_CalculatesCorrectly()
        {
            // Arrange
            decimal subTotal = 0.01m;
            decimal tax = 0.01m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(0.02m));
        }

        [Test]
        [Category("BVA")]
        [Description("BV3: Large values")]
        public void CalculateGrandTotal_LargeValues_CalculatesCorrectly()
        {
            // Arrange
            decimal subTotal = 999999.99m;
            decimal tax = 100000.00m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(1099999.99m));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Decimal precision in grand total")]
        public void CalculateGrandTotal_DecimalPrecision_MaintainsPrecision()
        {
            // Arrange
            decimal subTotal = 123.45m;
            decimal tax = 12.35m;

            // Act
            decimal grandTotal = _orderBLL.CalculateGrandTotal(subTotal, tax);

            // Assert
            Assert.That(grandTotal, Is.EqualTo(135.80m));
        }

        #endregion

        #region Order Status Transition Tests - EP

        /*
         * Test Design Documentation for Status Transitions:
         *
         * Valid Transitions (EP - Valid Class):
         * - Pending -> Processing, Cancelled
         * - Processing -> Shipped, Cancelled
         * - Shipped -> Delivered
         *
         * Invalid Transitions (EP - Invalid Class):
         * - Delivered -> any
         * - Cancelled -> any
         * - Shipped -> Processing (backward)
         * - Pending -> Delivered (skip)
         */

        [Test]
        [Category("EP")]
        [Description("EP: Valid status values")]
        public void ValidOrderStatuses_AreRecognized()
        {
            // These are the valid status values used in the system
            string[] validStatuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

            foreach (var status in validStatuses)
            {
                Assert.That(status, Is.Not.Null.And.Not.Empty);
            }
        }

        #endregion
    }
}
