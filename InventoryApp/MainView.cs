using InventoryApp.InventoryApp.Views;
using System;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp
{
    public partial class MainView : Form
    {
        private Form currentForm;

        private bool sidebarExpanded = true;
        private const int MinSidebarWidth = 59;
        private const int MaxSidebarWidth = 200;
        private const int AnimationStep = 10;
        public MainView()
        {
            InitializeComponent();
            SwitchForm(new Home());
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
                SwitchForm(new Home());
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
