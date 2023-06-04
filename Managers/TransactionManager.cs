using System.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace InventoryApp.Managers
{
    internal class TransactionManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        // Insert Transaction Items
        public void InsertTransactionItems(ListBox listBox, string transactionId)
        {
            con.Open();
            string insertQuery = "INSERT INTO Orders (TransactionId, Name, Price, Quantity) VALUES (@TransactionId, @Name, @Price, @Quantity)";

            using (SqlCommand insertCommand = new SqlCommand(insertQuery, con))
            {
                foreach (var item in listBox.Items)
                {
                    string[] parts = item.ToString().Split(new string[] { " x ", " - $" }, StringSplitOptions.None);
                    string name = parts[1];
                    decimal price = decimal.Parse(parts[2]);
                    int quantity = int.Parse(parts[0]);

                    insertCommand.Parameters.Clear();
                    insertCommand.Parameters.AddWithValue("@TransactionId", transactionId);
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Price", price);
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                    insertCommand.ExecuteNonQuery();
                }
            }

            con.Close();
        }

        // Saved Transaction
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

        // Delete Cart data after Transactions
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
