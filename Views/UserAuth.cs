using InventoryApp.InventoryApp;
using InventoryApp.Managers;
using System;
using System.Windows.Forms;

namespace InventoryApp.Views
{
    public partial class UserAuth : Form
    {
        private bool isRegisterMode = false;
        private readonly AccountManager accountManger;
        public UserAuth()
        {
            InitializeComponent();
            accountManger = new AccountManager();
        }

        //Validate Users Credentials
        private bool ValidateUserCredentials(string username, string password)
        {
            return accountManger.ValidateUserCredentials(username, password);
        }

        // Login or Register Button
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (isRegisterMode)
            {
                RegisterUser(username, password);
            }
            else
            {
                if (ValidateUserCredentials(username, password))
                {
                    MainView mainpage = new MainView();
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

        // Register new user
        private void RegisterUser(string username, string password)
        {
            accountManger.RegisterUser(username, password);
        }

        // Toggle label mode
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
    }
}
