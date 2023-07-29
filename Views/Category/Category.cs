using System.Windows.Forms;
using InventoryApp.Data;

namespace InventoryApp.InventoryApp.Views
{
    public partial class Category : Form
    {
        private readonly CategoryManager categoryManager;
        public Category()
        {
            InitializeComponent();
            categoryManager = new CategoryManager();
            dataGridView1.DataSource = categoryManager.GetCategories();
        }

        //ADD BUTTON - Category
        private void button1_Click(object sender, System.EventArgs e)
        {
            CatDialog dlg = new CatDialog(categoryManager);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = categoryManager.GetCategories();
            }
        }

        //UPDATE BUTTON - Category
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the data from the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells["Id"].Value;
                string categoryItem = row.Cells["CategoryItem"].Value.ToString();

                // Pass the data to EditCat
                CatDialog dlg = new CatDialog(categoryManager, id, categoryItem);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.DataSource = categoryManager.GetCategories();
                }
            }
            else
            {
                MessageBox.Show("No category is available for editing.", "Empty Category",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //DELETE BUTTON - Category
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                if (MessageBox.Show("Are you sure want to delete this category?", "Warning!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    categoryManager.DeleteCategory(id);
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.", "Empty Category",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
