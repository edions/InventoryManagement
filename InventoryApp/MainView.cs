using InventoryApp.InventoryApp.Views;
using System;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp
{
    public partial class MainView : Form
    {
        private readonly Home homeForm; // Add a private field to store the instance of the "Home" form
        public MainView()
        {
            InitializeComponent();

            homeForm = new Home
            {
                // Set up the mainForm
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            }; // Instantiate the "Main" form

            // Add the mainForm to the panel
            panel2.Controls.Add(homeForm);
            homeForm.Show();
        }

        //NAVIGATION CONTROL
        private void AddForm(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Check if panel2 already contains a form
            if (panel2.Controls.Count > 0)
            {
                // Hide the current form instead of clearing the panel
                Control currentForm = panel2.Controls[0];
                currentForm.Hide();
                panel1.Controls.Remove(currentForm);
            }

            panel2.Controls.Add(form);
            form.Show();
        }

        //HOME TAB
        private void button1_Click(object sender, EventArgs e)
        {
            // Show the Main form if it exists in the panel
            if (panel2.Controls.Count > 0 && panel2.Controls[0] is Home)
            {
                panel2.Controls[0].Show();
            }
            else
            {
                AddForm(new Home());
            }
        }

        //CATEGORY TAB
        private void button2_Click(object sender, EventArgs e)
        {
            Category categoryView = null;

            foreach (Form form in Application.OpenForms)
            {
                if (form is Category category)
                {
                    categoryView = category;
                    break;
                }
            }

            if (categoryView == null)
            {
                categoryView = new Category();
                AddForm(categoryView);
            }
            else
            {
                // Check if the Category form is hidden
                if (categoryView.Visible == false)
                {
                    categoryView.Show();
                }
                // Check if the Category form is not the currently displayed form
                else if (panel2.Controls.Count > 0 && panel2.Controls[0] != categoryView)
                {
                    panel2.Controls[0].Hide();
                    categoryView.Show();
                }
            }
        }
        
        //CART TAB
        private void button3_Click(object sender, EventArgs e)
        {
            Cart dlg = new Cart();
            dlg.ShowDialog();
        }
    }
}
