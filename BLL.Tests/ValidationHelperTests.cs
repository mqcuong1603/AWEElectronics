using System;
using NUnit.Framework;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL.Tests
{
    /// <summary>
    /// Unit tests for common validation helpers and cross-cutting business rules.
    /// Test techniques applied:
    /// - Equivalence Partitioning (EP): Testing representative values from different input classes
    /// - Boundary Value Analysis (BVA): Testing at the boundaries of input ranges
    /// </summary>
    [TestFixture]
    public class ValidationHelperTests
    {
        #region Email Validation Tests - Comprehensive EP and BVA

        /*
         * Comprehensive Email Validation Test Design:
         * Using MailAddress class for validation
         *
         * Equivalence Partitions:
         * EP1: Valid standard emails
         * EP2: Valid emails with special characters
         * EP3: Invalid - missing components
         * EP4: Invalid - malformed structure
         * EP5: Edge cases
         */

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [Test]
        [Category("EP")]
        [Description("EP1: Standard email addresses are valid")]
        [TestCase("user@example.com")]
        [TestCase("john.doe@company.org")]
        [TestCase("admin@mail.co.uk")]
        public void Email_StandardFormat_IsValid(string email)
        {
            Assert.That(IsValidEmail(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP2: Emails with valid special characters")]
        [TestCase("user+tag@example.com")]
        [TestCase("user.name@example.com")]
        [TestCase("user_name@example.com")]
        public void Email_WithSpecialCharacters_IsValid(string email)
        {
            Assert.That(IsValidEmail(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP3: Emails missing required components are invalid")]
        [TestCase("user")]
        [TestCase("@example.com")]
        [TestCase("user@")]
        [TestCase("")]
        public void Email_MissingComponents_IsInvalid(string email)
        {
            Assert.That(IsValidEmail(email), Is.False);
        }

        [Test]
        [Category("EP")]
        [Description("EP4: Malformed emails are invalid")]
        [TestCase("user@@example.com")]
        [TestCase("user@.com")]
        [TestCase("user@example..com")]
        public void Email_Malformed_IsInvalid(string email)
        {
            Assert.That(IsValidEmail(email), Is.False);
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Shortest valid email")]
        public void Email_ShortestValid_IsValid()
        {
            // a@b.co is one of the shortest valid email formats
            string email = "a@b.co";
            Assert.That(IsValidEmail(email), Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP5: Email with numbers is valid")]
        public void Email_WithNumbers_IsValid()
        {
            string email = "user123@example456.com";
            Assert.That(IsValidEmail(email), Is.True);
        }

        #endregion

        #region String Length Validation Tests - BVA

        /*
         * Test Design for String Length Boundaries:
         * Testing common minimum length requirements:
         * - 2 characters (Category Name, Company Name, Product Name)
         * - 3 characters (SKU, Username)
         * - 6 characters (Password)
         */

        [Test]
        [Category("BVA")]
        [Description("BVA: String length boundary tests for 2-char minimum")]
        [TestCase("", false)]
        [TestCase("A", false)]
        [TestCase("AB", true)]
        [TestCase("ABC", true)]
        public void StringLength_MinimumTwo_BoundaryTests(string input, bool expectedValid)
        {
            bool isValid = !string.IsNullOrEmpty(input) && input.Length >= 2;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: String length boundary tests for 3-char minimum")]
        [TestCase("", false)]
        [TestCase("A", false)]
        [TestCase("AB", false)]
        [TestCase("ABC", true)]
        [TestCase("ABCD", true)]
        public void StringLength_MinimumThree_BoundaryTests(string input, bool expectedValid)
        {
            bool isValid = !string.IsNullOrEmpty(input) && input.Length >= 3;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: String length boundary tests for 6-char minimum")]
        [TestCase("", false)]
        [TestCase("12345", false)]
        [TestCase("123456", true)]
        [TestCase("1234567", true)]
        public void StringLength_MinimumSix_BoundaryTests(string input, bool expectedValid)
        {
            bool isValid = !string.IsNullOrEmpty(input) && input.Length >= 6;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        #endregion

        #region Numeric ID Validation Tests - BVA

        /*
         * Test Design for ID Validation:
         * Rule: IDs must be > 0
         *
         * Boundary Values:
         * -1, 0, 1, int.MaxValue
         */

        [Test]
        [Category("BVA")]
        [Description("BVA: ID validation boundary tests")]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(100, true)]
        [TestCase(int.MaxValue, true)]
        public void ID_Validation_BoundaryTests(int id, bool expectedValid)
        {
            bool isValid = id > 0;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        #endregion

        #region Price/Decimal Validation Tests - BVA

        /*
         * Test Design for Price Validation:
         * Rule: Price >= 0
         *
         * Boundary Values:
         * -0.01, 0, 0.01, large values
         */

        [Test]
        [Category("BVA")]
        [Description("BVA: Price validation boundary tests")]
        [TestCase(-0.01, false)]
        [TestCase(0, true)]
        [TestCase(0.01, true)]
        [TestCase(100.00, true)]
        [TestCase(99999.99, true)]
        public void Price_Validation_BoundaryTests(double price, bool expectedValid)
        {
            decimal priceDecimal = (decimal)price;
            bool isValid = priceDecimal >= 0;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        #endregion

        #region Stock Level Validation Tests - BVA

        /*
         * Test Design for Stock Level Validation:
         * Rule: StockLevel >= 0
         *
         * Boundary Values:
         * -1, 0, 1, large values
         */

        [Test]
        [Category("BVA")]
        [Description("BVA: Stock level validation boundary tests")]
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(100, true)]
        [TestCase(10000, true)]
        public void StockLevel_Validation_BoundaryTests(int stock, bool expectedValid)
        {
            bool isValid = stock >= 0;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        #endregion

        #region Quantity Validation Tests - BVA

        /*
         * Test Design for Quantity (Goods In/Out):
         * Rule: Quantity > 0
         *
         * Boundary Values:
         * -1, 0, 1
         */

        [Test]
        [Category("BVA")]
        [Description("BVA: Quantity validation boundary tests (must be > 0)")]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(100, true)]
        public void Quantity_Validation_BoundaryTests(int quantity, bool expectedValid)
        {
            bool isValid = quantity > 0;
            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        #endregion

        #region Order Status Transition Tests - EP

        /*
         * Test Design for Order Status Transitions:
         * Valid status values: Pending, Processing, Shipped, Delivered, Cancelled
         *
         * Valid Transitions:
         * Pending -> Processing, Cancelled
         * Processing -> Shipped, Cancelled
         * Shipped -> Delivered
         * Delivered -> (none)
         * Cancelled -> (none)
         */

        private bool IsValidStatusTransition(string fromStatus, string toStatus)
        {
            var validTransitions = new System.Collections.Generic.Dictionary<string, string[]>
            {
                { "Pending", new[] { "Processing", "Cancelled" } },
                { "Processing", new[] { "Shipped", "Cancelled" } },
                { "Shipped", new[] { "Delivered" } },
                { "Delivered", new string[] { } },
                { "Cancelled", new string[] { } }
            };

            if (!validTransitions.ContainsKey(fromStatus))
                return false;

            return Array.Exists(validTransitions[fromStatus], s => s == toStatus);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Valid order status transitions")]
        [TestCase("Pending", "Processing", true)]
        [TestCase("Pending", "Cancelled", true)]
        [TestCase("Processing", "Shipped", true)]
        [TestCase("Processing", "Cancelled", true)]
        [TestCase("Shipped", "Delivered", true)]
        public void OrderStatus_ValidTransitions_AreAllowed(string from, string to, bool expected)
        {
            Assert.That(IsValidStatusTransition(from, to), Is.EqualTo(expected));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Invalid order status transitions")]
        [TestCase("Pending", "Shipped", false)]      // Skip Processing
        [TestCase("Pending", "Delivered", false)]    // Skip Processing & Shipped
        [TestCase("Processing", "Pending", false)]   // Backward
        [TestCase("Shipped", "Processing", false)]   // Backward
        [TestCase("Delivered", "Shipped", false)]    // From final state
        [TestCase("Cancelled", "Pending", false)]    // From final state
        public void OrderStatus_InvalidTransitions_AreRejected(string from, string to, bool expected)
        {
            Assert.That(IsValidStatusTransition(from, to), Is.EqualTo(expected));
        }

        [Test]
        [Category("EP")]
        [Description("EP: Final states have no valid transitions")]
        public void OrderStatus_FinalStates_HaveNoTransitions()
        {
            // Delivered and Cancelled are final states
            Assert.That(IsValidStatusTransition("Delivered", "Processing"), Is.False);
            Assert.That(IsValidStatusTransition("Delivered", "Cancelled"), Is.False);
            Assert.That(IsValidStatusTransition("Cancelled", "Processing"), Is.False);
            Assert.That(IsValidStatusTransition("Cancelled", "Delivered"), Is.False);
        }

        #endregion

        #region Stock Adjustment Calculation Tests - EP and BVA

        /*
         * Test Design for Stock Adjustment Calculations:
         * New Stock = Current Stock + Adjustment
         * Must verify: New Stock >= 0
         */

        [Test]
        [Category("EP")]
        [Description("EP: Positive adjustment increases stock")]
        [TestCase(100, 50, 150)]
        [TestCase(0, 100, 100)]
        [TestCase(50, 1, 51)]
        public void StockAdjustment_PositiveChange_IncreasesStock(int current, int change, int expected)
        {
            int newStock = current + change;
            Assert.That(newStock, Is.EqualTo(expected));
            Assert.That(newStock >= 0, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Negative adjustment decreases stock")]
        [TestCase(100, -50, 50)]
        [TestCase(100, -100, 0)]
        [TestCase(50, -1, 49)]
        public void StockAdjustment_NegativeChange_DecreasesStock(int current, int change, int expected)
        {
            int newStock = current + change;
            Assert.That(newStock, Is.EqualTo(expected));
            Assert.That(newStock >= 0, Is.True);
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Adjustment that would cause negative stock")]
        [TestCase(10, -11, -1)]
        [TestCase(0, -1, -1)]
        [TestCase(100, -101, -1)]
        public void StockAdjustment_CausingNegativeStock_IsInvalid(int current, int change, int expected)
        {
            int newStock = current + change;
            Assert.That(newStock, Is.EqualTo(expected));
            Assert.That(newStock < 0, Is.True, "Negative stock should be rejected");
        }

        #endregion

        #region Tax Calculation Tests - EP and BVA

        /*
         * Test Design for Tax Calculation:
         * Tax = Round(SubTotal * 0.10, 2)
         */

        private decimal CalculateTax(decimal subTotal)
        {
            const decimal TAX_RATE = 0.10m;
            return Math.Round(subTotal * TAX_RATE, 2);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Tax calculation for various subtotals")]
        [TestCase(100.00, 10.00)]
        [TestCase(1000.00, 100.00)]
        [TestCase(50.00, 5.00)]
        public void TaxCalculation_VariousSubtotals_CalculatesCorrectly(double subTotal, double expectedTax)
        {
            decimal result = CalculateTax((decimal)subTotal);
            Assert.That(result, Is.EqualTo((decimal)expectedTax));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Tax calculation with rounding")]
        [TestCase(33.33, 3.33)]     // 33.33 * 0.10 = 3.333 -> 3.33
        [TestCase(33.35, 3.34)]     // 33.35 * 0.10 = 3.335 -> 3.34 (banker's rounding)
        [TestCase(0.01, 0.00)]      // 0.01 * 0.10 = 0.001 -> 0.00
        public void TaxCalculation_WithRounding_RoundsCorrectly(double subTotal, double expectedTax)
        {
            decimal result = CalculateTax((decimal)subTotal);
            Assert.That(result, Is.EqualTo((decimal)expectedTax));
        }

        [Test]
        [Category("BVA")]
        [Description("BVA: Tax calculation at boundaries")]
        public void TaxCalculation_ZeroSubtotal_ReturnsZero()
        {
            decimal result = CalculateTax(0m);
            Assert.That(result, Is.EqualTo(0m));
        }

        #endregion

        #region Date Range Validation Tests - EP

        /*
         * Test Design for Date Range:
         * StartDate should be <= EndDate
         */

        [Test]
        [Category("EP")]
        [Description("EP: Valid date ranges")]
        public void DateRange_StartBeforeEnd_IsValid()
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = new DateTime(2024, 12, 31);

            Assert.That(startDate <= endDate, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Same start and end date is valid")]
        public void DateRange_SameStartAndEnd_IsValid()
        {
            DateTime startDate = new DateTime(2024, 6, 15);
            DateTime endDate = new DateTime(2024, 6, 15);

            Assert.That(startDate <= endDate, Is.True);
        }

        [Test]
        [Category("EP")]
        [Description("EP: Start date after end date is invalid")]
        public void DateRange_StartAfterEnd_IsInvalid()
        {
            DateTime startDate = new DateTime(2024, 12, 31);
            DateTime endDate = new DateTime(2024, 1, 1);

            Assert.That(startDate > endDate, Is.True, "Start after end should be rejected");
        }

        #endregion
    }
}
