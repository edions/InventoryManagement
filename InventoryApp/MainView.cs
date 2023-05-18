using InventoryApp.InventoryApp.Views;
using System;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp
{
    public partial class MainView : Form
    {
        private readonly Home homeForm; // Add a private field to store the instance of the "Home" form
        private bool sidebarExpanded = true;
        private const int MinSidebarWidth = 60;
        private const int MaxSidebarWidth = 212;
        private const int AnimationStep = 10;
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

        //SIDEBAR CONTROL
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sidebarExpanded)
            {
                panel1.Width -= AnimationStep;
                panel2.Left -= AnimationStep;
                panel2.Width += AnimationStep;
                if (panel1.Width <= MinSidebarWidth)
                {
                    sidebarExpanded = false;
                    timer1.Stop();
                }
            }
            else
            {
                panel1.Width += AnimationStep;
                panel2.Left += AnimationStep;
                panel2.Width -= AnimationStep;
                if (panel1.Width >= MaxSidebarWidth)
                {
                    sidebarExpanded = true;
                    timer1.Stop();
                }
            }
            if (!timer1.Enabled)
            {
                panel1.ResumeLayout();
                panel2.ResumeLayout();

                // Enable the button when the animation is complete
                button4.Enabled = true;
            }
        }

        //HAMBURGER BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                // Animation is already in progress, no need to start it again
                return;
            }

            // Disable the button during the animation
            button4.Enabled = false;

            panel2.SuspendLayout();
            panel1.SuspendLayout();
            timer1.Start();
        }
    }
}
