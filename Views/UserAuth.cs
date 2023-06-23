using InventoryApp.InventoryApp;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.Views
{
    public partial class UserAuth : Form
    {
        private bool isRegisterMode = false;
        public UserAuth()
        {
            InitializeComponent();
        }

        private bool ValidateUserCredentials(string username, string password)
        {
            string connectionString = ConnectionManager.GetConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                int count = (int)command.ExecuteScalar();

                return (count > 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (isRegisterMode)
            {
                // Perform registration logic
                RegisterUser(username, password);
            }
            else
            {
                // Perform login logic
                if (ValidateUserCredentials(username, password))
                {
                    // Navigate to another form or perform any desired actions on successful login
                    MainView anotherForm = new MainView();
                    anotherForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RegisterUser(string username, string password)
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

                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
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

        private bool IsUsernameExists(string username)
        {
            string connectionString = ConnectionManager.GetConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                int count = (int)command.ExecuteScalar();

                return (count > 0);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            isRegisterMode = !isRegisterMode; // Toggle the mode

            if (isRegisterMode)
            {
                // Switch to register mode
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                linkLabel1.Text = "LOGIN";
                button1.Text = "REGISTER";
            }
            else
            {
                // Switch to login mode
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                linkLabel1.Text = "REGISTER";
                button1.Text = "LOGIN";
            }
        }
    }
}
