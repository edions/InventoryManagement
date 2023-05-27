using System.Data.SqlClient;

namespace InventoryApp
{
    public static class ConnectionManager
    {
        private static readonly string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }
    }
}
