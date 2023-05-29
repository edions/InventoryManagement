using System.Data.SqlClient;
using System;

namespace InventoryApp.Managers
{
    internal class TransactionManager
    {
        private SqlConnection con;

    public TransactionManager(SqlConnection connection)
    {
        con = connection;
    }

    public void SaveTransactionToDatabase(string transactionId, int total, int cash, double discountPercent, double discountAmount, double change, DateTime currentDate)
    {
        con.Open();
        string insertQuery = "INSERT INTO [Transaction] (TransactionId, Total, Cash, DiscountPercent, DiscountAmount, [Change], Date) VALUES (@TransactionId, @Total, @Cash, @DiscountPercent, @DiscountAmount, @Change, @Date)";

        using (SqlCommand command = new SqlCommand(insertQuery, con))
        {
            command.Parameters.AddWithValue("@TransactionId", transactionId);
            command.Parameters.AddWithValue("@Total", total);
            command.Parameters.AddWithValue("@Cash", cash);
            command.Parameters.AddWithValue("@DiscountPercent", discountPercent);
            command.Parameters.AddWithValue("@DiscountAmount", discountAmount);
            command.Parameters.AddWithValue("@Change", change);
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
