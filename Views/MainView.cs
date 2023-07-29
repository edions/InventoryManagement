using System;
using InventoryApp.Data;
using System.Windows.Forms;
using InventoryApp.InventoryApp.dlg;
using InventoryApp.InventoryApp.Views;
using InventoryApp.Views;
using InventoryApp.Views.Dashboard;

namespace InventoryApp.InventoryApp
{
    public partial class MainView : Form
    {
        private Form currentForm;
        public MainView(string username)
        {
            InitializeComponent();
            SwitchForm(new Dashboard());

            button1.Text = "Logout (" + username + ")";

            // Initialize Cart item counter
            itemCountTimer = new Timer
            {
                Interval = 1000
            };
            itemCountTimer.Tick += itemCountTimer_Tick;
            itemCountTimer.Start();
        }

        //NAVIGATION CONTROL
        private void SwitchForm(Form newForm)
        {
            currentForm?.Hide();
            newForm.TopLevel = false;
            newForm.FormBorderStyle = FormBorderStyle.None;
            newForm.Dock = DockStyle.Fill;
            // Check if panel2 already contains a form
            if (panel2.Controls.Count > 0)
            {
                Control currentFormControl = panel2.Controls[0];
                currentFormControl.Hide();
                panel2.Controls.Remove(currentFormControl);
            }
            panel2.Controls.Add(newForm);
            newForm.Show();

            currentForm = newForm;
        }

        // DASHBOARD
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                SwitchForm(new Dashboard());
            }
        }

        //HOME TAB
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SwitchForm(new Product());
            }
        }

        //CATEGORY TAB
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                SwitchForm(new Category());
            }
        }

        //CART TAB
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                SwitchForm(new Sale());
            }
        }

        //TRANSACTION TAB
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                SwitchForm(new Transaction());
            }
        }

        // LOGOUT BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to logout?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                UserAuth userauth = new UserAuth();
                userauth.FormClosed += (s, args) => this.Close();
                userauth.Show();
                Hide();
            }
        }

        // CART COUNTER
        private void itemCountTimer_Tick(object sender, EventArgs e)
        {
            CartManager cartManager = new CartManager();
            int cartItemCount = cartManager.GetCartItemCount();
            radioButton3.Text = "Sale (" + cartItemCount.ToString() + ")";
        }
    }
}
