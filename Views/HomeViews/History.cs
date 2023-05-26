using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.dlg
{
    public partial class History : Form
    {
        private readonly int productId;
        public History(int id)
        {
            InitializeComponent();
            productId = id;
            DisplayHistory();

        }

        //FETCH DATA FROM HISTORY TABLE
        private void DisplayHistory()
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ProductID, [Added Stocks], [Date] FROM History WHERE ProductID = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", productId);

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
