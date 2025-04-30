using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
            DisplayProduct();
            ProductDgv.CellContentDoubleClick += ProductDgv_CellContentDoubleClick; // Tambahkan pengait event di sini
        }

        readonly SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\muham\OneDrive\Documents\SistemManajemenTokoEmas.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayProduct()
        {
            try
            {
                string query = "SELECT * FROM [Product]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                ProductDgv.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Product_Load(object sender, EventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            ProIdTb.Text = "";
            ProNameCb.Text = "";
            ProCatCb.Text = "";
            ProQuanTb.Text = "";
            UPTb.Text = "";
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProIdTb.Text == "" || ProNameCb.Text == "" || ProCatCb.Text == "" || ProQuanTb.Text == "" || UPTb.Text == "")
                {
                    MessageBox.Show("Semua kolom harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Con.Open();
                string query = "INSERT INTO [Product] (ProId, ProName, ProCategory, Quantity, UnitPrice) VALUES (@ProId, @ProName, @ProCategory, @Quantity, @UnitPrice)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@ProId", ProIdTb.Text);
                cmd.Parameters.AddWithValue("@ProName", ProNameCb.Text);
                cmd.Parameters.AddWithValue("@ProCategory", ProCatCb.Text);
                cmd.Parameters.AddWithValue("@Quantity", ProQuanTb.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", UPTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produk berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Con.Close();
                DisplayProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProIdTb.Text == "" || ProNameCb.Text == "" || ProCatCb.Text == "" || ProQuanTb.Text == "" || UPTb.Text == "")
                {
                    MessageBox.Show("Semua kolom harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Con.Open();
                string query = "UPDATE [Product] SET ProName = @ProName, ProCategory = @ProCategory, Quantity = @Quantity, UnitPrice = @UnitPrice WHERE ProId = @ProId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@ProId", ProIdTb.Text);
                cmd.Parameters.AddWithValue("@ProName", ProNameCb.Text);
                cmd.Parameters.AddWithValue("@ProCategory", ProCatCb.Text);
                cmd.Parameters.AddWithValue("@Quantity", ProQuanTb.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", UPTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produk berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Con.Close();
                DisplayProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProIdTb.Text == "")
                {
                    MessageBox.Show("ID produk harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Con.Open();
                string query = "DELETE FROM [Product] WHERE ProId = @ProId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@ProId", ProIdTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produk berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Con.Close();

                DisplayProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void Customerlbl_Click(object sender, EventArgs e)
        {
            Customer obj = new Customer();
            obj.Show();
            this.Hide();
        }

        private void Billlbl_Click(object sender, EventArgs e)
        {
            Bill obj = new Bill();
            obj.Show();
            this.Hide();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void ProductDgv_DoubleClick(object sender, EventArgs e)
        {
            if (ProductDgv.CurrentRow != null && ProductDgv.CurrentRow.Index != -1)
            {
                ProIdTb.Text = ProductDgv.CurrentRow.Cells["ProId"].Value.ToString();
                ProNameCb.Text = ProductDgv.CurrentRow.Cells["ProName"].Value.ToString();
                ProCatCb.Text = ProductDgv.CurrentRow.Cells["ProCategory"].Value.ToString();
                ProQuanTb.Text = ProductDgv.CurrentRow.Cells["Quantity"].Value.ToString();
                UPTb.Text = ProductDgv.CurrentRow.Cells["UnitPrice"].Value.ToString();
            }
        }

        private void ProductDgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ProductDgv.Rows[e.RowIndex].Cells["ProId"].Value != null)
            {
                DataGridViewRow row = ProductDgv.Rows[e.RowIndex];
                ProIdTb.Text = row.Cells["ProId"].Value.ToString();
                ProNameCb.Text = row.Cells["ProName"].Value.ToString();
                ProCatCb.Text = row.Cells["ProCategory"].Value.ToString();
                ProQuanTb.Text = row.Cells["Quantity"].Value.ToString();
                UPTb.Text = row.Cells["UnitPrice"].Value.ToString();
            }
        }
    }
}
