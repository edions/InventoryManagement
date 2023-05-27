using InventoryApp.Entity;
using System;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class CreateCat : Form
    {
        private readonly CategoryManager categoryManager;

        public CreateCat(CategoryManager manager)
        {
            InitializeComponent();
            categoryManager = manager;
        }

        //INSERT BUTTON - Cat
        private void button1_Click(object sender, EventArgs e)
        {
            categoryManager.AddCategory(textBox2.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON - Cat
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
