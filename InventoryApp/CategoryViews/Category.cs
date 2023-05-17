using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.Views
{
    public partial class Category : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Category()
        {
            InitializeComponent();
            CategoryDisplay();
        }

        //FETCH DATA FROM CATEGORY DATABASE
        private void CategoryDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Category";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        //ADD BUTTON
        //Category
        private void button1_Click(object sender, System.EventArgs e)
        {

        }

        //UPDATE BUTTON
        //Category
        private void button2_Click(object sender, System.EventArgs e)
        {

        }

        //DELETE BUTTON
        //Category
        private void button3_Click(object sender, System.EventArgs e)
        {

        }
    }
}
