using System.Data.SqlClient;
using System;

namespace InventoryApp.Managers
{
    internal class TransactionManager
    {
        private readonly SqlConnection con;

        public TransactionManager(SqlConnection connection)
        {
            con = connection;
        }

        public void SaveTransactionToDatabase(string transactionId, int subtotal, int cash, double discountPercent, double discountAmount, double change, DateTime currentDate, double total)
        {
            con.Open();

            // Get the quantity from the Cart table and update the product stock
            string updateStockQuery = "UPDATE Product SET Stock = Stock - Cart.Quantity FROM Product INNER JOIN Cart ON Product.Id = Cart.ProductId";
            using (SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, con))
            {
                updateStockCommand.ExecuteNonQuery();
            }

            string insertQuery = "INSERT INTO [Transaction] (TransactionId, Subtotal, Cash, DiscountPercent, DiscountAmount, [Change], Total, Date) VALUES (@TransactionId, @Subtotal, @Cash, @DiscountPercent, @DiscountAmount, @Change, @Total, @Date)";

            using (SqlCommand command = new SqlCommand(insertQuery, con))
            {
                command.Parameters.AddWithValue("@TransactionId", transactionId);
                command.Parameters.AddWithValue("@Subtotal", subtotal);
                command.Parameters.AddWithValue("@Cash", cash);
                command.Parameters.AddWithValue("@DiscountPercent", discountPercent);
                command.Parameters.AddWithValue("@DiscountAmount", discountAmount);
                command.Parameters.AddWithValue("@Change", change);
                command.Parameters.AddWithValue("@Total", total);
                command.Parameters.AddWithValue("@Date", currentDate);
                command.ExecuteNonQuery();
            }

            con.Close();
        }

        public void DeleteCartData()
        {
            con.Open();
            string deleteQuery = "DELETE FROM [Cart]";
            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, con))
            {
                deleteCommand.ExecuteNonQuery();
            }

            con.Close();
        }
    }
}
