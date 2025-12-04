using System;

namespace AWEElectronics.DTO
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public decimal Amount { get; set; }
        public string Provider { get; set; } // Credit Card, PayPal, Bank Transfer
        public string PaymentMethod { get; set; } // Alias for Provider
        public string Status { get; set; } // Pending, Completed, Failed, Refunded
        public string TransactionRef { get; set; }
        public string TransactionID { get; set; } // Alias for TransactionRef
        public DateTime? PaidAt { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation property
        public string OrderCode { get; set; }
    }
}
