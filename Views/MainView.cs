using InventoryApp.InventoryApp.dlg;
using InventoryApp.InventoryApp.Views;
using System;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp
{
    public partial class MainView : Form
    {
        private Form currentForm;
        public MainView()
        {
            InitializeComponent();
            SwitchForm(new Product());
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
                SwitchForm(new Cart());
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
    }
}
