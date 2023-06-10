using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.dlg
{
    public partial class Details : Form
    {
        public Details(string id)
        {
            InitializeComponent();
            DisplayTransactionItems(id);

        }

        //FETCH DATA FROM ORDERS TABLE
        private void DisplayTransactionItems(string transactionId)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Name, Price, Quantity FROM Orders WHERE TransactionId = @transactionId", con))
                {
                    cmd.Parameters.AddWithValue("@transactionId", transactionId);

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
