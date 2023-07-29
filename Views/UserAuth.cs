using InventoryApp.InventoryApp;
using InventoryApp.Data;
using System.Windows.Forms;
using System;

namespace InventoryApp.Views
{
    public partial class UserAuth : Form
    {
        private bool isRegisterMode = false;
        private readonly AccountManager accountManager;
        public UserAuth()
        {
            InitializeComponent();
            accountManager = new AccountManager();
        }

        // Validate Users Credentials
        private int ValidateUserCredentials(string username, string password)
        {
            return accountManager.ValidateUserCredentials(username, password);
        }

        // Process Login Form
        private void ProcessLoginForm()
        {
            errorProvider1.Clear();

            string username = textBox1.Text;
            string password = textBox2.Text;

            bool isValid = true;

            if (string.IsNullOrEmpty(username))
            {
                errorProvider1.SetError(textBox1, "Please enter a username.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(password))
            {
                errorProvider1.SetError(textBox2, "Please enter a password.");
                isValid = false;
            }

            if (isValid)
            {
                if (isRegisterMode)
                {
                    RegisterUser(username, password);
                }
                else
                {
                    int uid = ValidateUserCredentials(username, password);
                    if (uid != 0)
                    {
                        UserSession.SessionUID = uid;
                        MainView mainpage = new MainView(username);
                        mainpage.FormClosed += (s, args) => this.Close();
                        mainpage.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password. Please try again.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Toggle label mode
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            isRegisterMode = !isRegisterMode; // Toggle the mode

            if (isRegisterMode)
            {
                // Switch to register mode
                errorProvider1.Clear();
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
                errorProvider1.Clear();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                linkLabel1.Text = "REGISTER";
                button1.Text = "LOGIN";
            }
        }

        // Login or Register Button
        private void button1_Click(object sender, EventArgs e)
        {
            ProcessLoginForm();
        }

        // Register new user
        private void RegisterUser(string username, string password)
        {
            accountManager.RegisterUser(username, password);
        }

        // Show password checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        //TextBox key press event
        #region
        // Event handler for key press event in textBox1
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    textBox2.Focus();
                    e.Handled = true;
                }
            }
        }

        // Event handler for key press event in textBox2
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ProcessLoginForm();
                    e.Handled = true;
                }
            }
        }
        #endregion
    }
}
