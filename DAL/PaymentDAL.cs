using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class PaymentDAL
    {
        public List<Payment> GetAll()
        {
            List<Payment> payments = new List<Payment>();
            string query = @"SELECT p.*, o.OrderCode
                            FROM Payments p
                            LEFT JOIN Orders o ON p.OrderID = o.OrderID
                            ORDER BY p.PaymentID DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                payments.Add(MapPayment(row));
            }
            return payments;
        }

        public Payment GetByOrderId(int orderId)
        {
            string query = @"SELECT p.*, o.OrderCode
                            FROM Payments p
                            LEFT JOIN Orders o ON p.OrderID = o.OrderID
                            WHERE p.OrderID = @OrderID";

            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapPayment(dt.Rows[0]);

            return null;
        }

        public int Insert(Payment payment)
        {
            string query = @"INSERT INTO Payments (OrderID, Amount, Provider, Status, TransactionRef, PaidAt)
                            VALUES (@OrderID, @Amount, @Provider, @Status, @TransactionRef, @PaidAt);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", payment.OrderID),
                new SqlParameter("@Amount", payment.Amount),
                new SqlParameter("@Provider", payment.Provider),
                new SqlParameter("@Status", payment.Status ?? "Pending"),
                new SqlParameter("@TransactionRef", (object)payment.TransactionRef ?? DBNull.Value),
                new SqlParameter("@PaidAt", (object)payment.PaidAt ?? DBNull.Value)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool UpdateStatus(int paymentId, string status, string transactionRef = null)
        {
            string query = @"UPDATE Payments SET
                            Status = @Status,
                            TransactionRef = @TransactionRef,
                            PaidAt = CASE WHEN @Status = 'Completed' THEN GETDATE() ELSE PaidAt END
                            WHERE PaymentID = @PaymentID";

            SqlParameter[] parameters = {
                new SqlParameter("@PaymentID", paymentId),
                new SqlParameter("@Status", status),
                new SqlParameter("@TransactionRef", (object)transactionRef ?? DBNull.Value)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private Payment MapPayment(DataRow row)
        {
            return new Payment
            {
                PaymentID = Convert.ToInt32(row["PaymentID"]),
                OrderID = Convert.ToInt32(row["OrderID"]),
                Amount = Convert.ToDecimal(row["Amount"]),
                Provider = row["Provider"].ToString(),
                Status = row["Status"].ToString(),
                TransactionRef = row["TransactionRef"]?.ToString(),
                PaidAt = row["PaidAt"] != DBNull.Value ? Convert.ToDateTime(row["PaidAt"]) : (DateTime?)null,
                OrderCode = row.Table.Columns.Contains("OrderCode") ? row["OrderCode"]?.ToString() : null
            };
        }
    }
}
