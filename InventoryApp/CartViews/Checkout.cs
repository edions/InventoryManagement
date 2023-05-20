using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class Checkout : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Checkout(int totalPrice)
        {
            InitializeComponent();
        }

        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
