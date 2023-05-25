using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.dlg
{
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
            DisplayHTransaction();
        }

        //FETCH DATA FROM TABLE
        private void DisplayHTransaction()
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Transaction]", con))
                {
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
