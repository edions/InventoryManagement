using System;
using InventoryApp.Data;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class AddStock : Form
    {
        private readonly StockManager stockManager;
        readonly private string itemName;
        public AddStock(string name)
        {
            InitializeComponent();
            stockManager = new StockManager();
            itemName = name;
            label3.Text = name;
        }

        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            int productId = stockManager.GetProductIdByName(itemName);
            int currentStock = stockManager.GetCurrentStockById(productId);
            int addedStocks = Convert.ToInt32(textBox2.Text);

            stockManager.UpdateStock(productId, currentStock + addedStocks);
            stockManager.InsertHistory(productId, addedStocks);

            DialogResult = DialogResult.OK;
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
