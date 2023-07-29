using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.Data
{
    class AccountManager
    {
        //Validate Users Credentials
        public int ValidateUserCredentials(string username, string password)
        {
            string connectionString = ConnectionManager.GetConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Uid FROM Account WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                object result = command.ExecuteScalar();
                int uid = result != null ? (int)result : 0;

                return uid;
            }
        }

        // Register new user
        public void RegisterUser(string username, string password)
        {
            // Check if the username already exists in the database
            if (IsUsernameExists(username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save the new user to the database
            string connectionString = ConnectionManager.GetConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Account (Username, Password) VALUES (@Username, @Password)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registration successful!", "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to register. Please try again.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Check if user name already exists
        private bool IsUsernameExists(string username)
        {
            string connectionString = ConnectionManager.GetConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Account WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                int count = (int)command.ExecuteScalar();

                return (count > 0);
            }
        }
    }
}
