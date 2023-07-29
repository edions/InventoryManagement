using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryApp.Data
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
                    insertCommand.Parameters.AddWithValue("@Price", "$" + price.ToString());
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

            int currentUID = UserSession.SessionUID;

            // Get the quantity from the Cart table and update the product stock
            string updateStockQuery = "UPDATE Product SET Stock = Stock - Cart.Quantity FROM Product INNER JOIN Cart ON Product.Id = Cart.ProductId";
            using (SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, con))
            {
                updateStockCommand.ExecuteNonQuery();
            }

            string insertQuery = "INSERT INTO [Transaction] (TransactionId, Subtotal, Cash, DiscountPercent, DiscountAmount, [Change], Total, Date, Uid)" +
                                 "VALUES (@TransactionId, @Subtotal, @Cash, @DiscountPercent, @DiscountAmount, @Change, @Total, @Date, @Uid)";

            using (SqlCommand command = new SqlCommand(insertQuery, con))
            {
                command.Parameters.AddWithValue("@TransactionId", transactionId);
                command.Parameters.AddWithValue("@Subtotal", "$" + subtotal.ToString());
                command.Parameters.AddWithValue("@Cash", "$" + cash.ToString());
                command.Parameters.AddWithValue("@DiscountPercent", Math.Round(discountPercent, 0) + "%");
                command.Parameters.AddWithValue("@DiscountAmount", "$" + discountAmount.ToString());
                command.Parameters.AddWithValue("@Change", "$" + change.ToString());
                command.Parameters.AddWithValue("@Total", "$" + total.ToString());
                command.Parameters.AddWithValue("@Date", currentDate);
                command.Parameters.AddWithValue("@Uid", currentUID);
                command.ExecuteNonQuery();
            }

            con.Close();
        }

        // Delete Cart data after Transactions
        public void DeleteCartData()
        {
            con.Open();

            int currentUID = UserSession.SessionUID;

            string deleteQuery = "DELETE FROM [Cart] WHERE Uid = @Uid";
            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, con))
            {
                deleteCommand.Parameters.AddWithValue("@Uid", currentUID);
                deleteCommand.ExecuteNonQuery();
            }

            con.Close();
        }
    }
}
