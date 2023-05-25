using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.dlg
{
    public partial class TransactionItem : Form
    {
        //private readonly string transactionId;
        public TransactionItem(string id)
        {
            InitializeComponent();
            //this.transactionId = transactionId;
            DisplayTransactionItems(id);

        }

        //FETCH DATA FROM HISTORY TABLE
        private void DisplayTransactionItems(string transactionId)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT TransactionId, Name, Price, Quantity FROM TransactionItem WHERE TransactionId = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", transactionId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

                con.Close();
            }
        }
    }
}
