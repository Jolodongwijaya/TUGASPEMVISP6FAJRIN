using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Bill : Form
    {
        public Bill()
        {
            InitializeComponent();
            DisplayBill();
            DisplayProduct();
            GetCusId();
        }

        readonly SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\muham\OneDrive\Documents\SistemManajemenTokoEmas.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayBill()
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string query = "SELECT * FROM [Bill]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Con);
                var ds = new DataSet();
                adapter.Fill(ds);
                BillDgv.DataSource = ds.Tables[0];

                foreach (DataGridViewColumn column in BillDgv.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                BillDgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void DisplayProduct()
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string query = "SELECT * FROM [Product]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Con);
                var ds = new DataSet();
                adapter.Fill(ds);
                ProductDgv.DataSource = ds.Tables[0];

                foreach (DataGridViewColumn column in ProductDgv.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                ProductDgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void GetCusId()
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                SqlCommand cmd = new SqlCommand("SELECT CusId FROM Customer", Con);
                SqlDataReader Rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("CusId", typeof(int));
                dt.Load(Rdr);
                CusIdCb.ValueMember = "CusId";
                CusIdCb.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void DisplayCusName()
        {
            if (CusIdCb.SelectedValue == null) return;
            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string ss = "SELECT CusName FROM Customer WHERE CusId = " + CusIdCb.SelectedValue.ToString();
                SqlCommand cmd = new SqlCommand(ss, Con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    CustNameTb.Text = dr["CusName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bill_Load(object sender, EventArgs e)
        {
            DisplayCusName();
        }

        private void CusIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DisplayCusName();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            BillIdTb.Text = "";
            ProNameCb.Text = "";
            CusIdCb.Text = "";
            CustNameTb.Text = "";
            ProQuanTb.Text = "";
            UPTb.Text = "";
            PriceTb.Text = "";
        }

        int flag = 0;
        int stock;
        int sum = 0;

        private void ProductDgv_DoubleClick(object sender, EventArgs e)
        {
            if (ProductDgv.SelectedRows.Count > 0)
            {
                ProNameCb.Text = ProductDgv.SelectedRows[0].Cells[1].Value.ToString();
                UPTb.Text = ProductDgv.SelectedRows[0].Cells[4].Value.ToString();
                stock = Convert.ToInt32(ProductDgv.SelectedRows[0].Cells[3].Value.ToString());
                flag = 1;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BillIdTb.Text) ||
                string.IsNullOrWhiteSpace(ProQuanTb.Text) ||
                string.IsNullOrWhiteSpace(UPTb.Text))
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return;
            }

            if (Convert.ToInt32(ProQuanTb.Text) > stock)
            {
                MessageBox.Show("Stock tidak mencukupi.");
                return;
            }

            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                int Total = Convert.ToInt32(ProQuanTb.Text) * Convert.ToInt32(UPTb.Text);
                sum += Total;
                PriceTb.Text = sum.ToString();

                string query = "INSERT INTO [Bill] (BillId, Product, CusId, CusName, Quantity, UnitPrice, Total) " +
                               "VALUES (@BillId, @Product, @CusId, @CusName, @Quantity, @UnitPrice, @Total)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@BillId", BillIdTb.Text);
                cmd.Parameters.AddWithValue("@Product", ProNameCb.Text);
                cmd.Parameters.AddWithValue("@CusId", CusIdCb.SelectedValue);
                cmd.Parameters.AddWithValue("@CusName", CustNameTb.Text);
                cmd.Parameters.AddWithValue("@Quantity", ProQuanTb.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", UPTb.Text);
                cmd.Parameters.AddWithValue("@Total", Total);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil ditambahkan.");
                DisplayBill();
                UpdateProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void UpdateProduct()
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string id = ProductDgv.SelectedRows[0].Cells[0].Value.ToString();
                int Qty = stock - Convert.ToInt32(ProQuanTb.Text);

                if (Qty < 0)
                {
                    MessageBox.Show("Stok tidak cukup.");
                    return;
                }

                string query = "UPDATE Product SET Quantity = @Qty WHERE ProId = @ProId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Qty", Qty);
                cmd.Parameters.AddWithValue("@ProId", id);
                cmd.ExecuteNonQuery();
                DisplayProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
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

        private void Productlbl_Click(object sender, EventArgs e)
        {
            Product obj = new Product();
            obj.Show();
            this.Hide();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BillIdTb.Text))
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return;
            }

            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string query = "DELETE FROM [Bill] WHERE BillId = @BillId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@BillId", BillIdTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil dihapus.");
                DisplayBill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BillIdTb.Text) ||
                string.IsNullOrWhiteSpace(ProQuanTb.Text) ||
                string.IsNullOrWhiteSpace(UPTb.Text) ||
                string.IsNullOrWhiteSpace(CustNameTb.Text))
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return;
            }

            try
            {
                if (Con.State != ConnectionState.Open)
                    Con.Open();

                string checkQuery = "SELECT COUNT(*) FROM [Bill] WHERE BillId = @BillId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Con);
                checkCmd.Parameters.AddWithValue("@BillId", BillIdTb.Text);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Bill ID tidak ditemukan.");
                    return;
                }

                int total = Convert.ToInt32(ProQuanTb.Text) * Convert.ToInt32(UPTb.Text);
                string query = "UPDATE [Bill] SET Product = @Product, CusId = @CusId, CusName = @CusName, Quantity = @Quantity, UnitPrice = @UnitPrice, Total = @Total WHERE BillId = @BillId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@BillId", BillIdTb.Text);
                cmd.Parameters.AddWithValue("@Product", ProNameCb.Text);
                cmd.Parameters.AddWithValue("@CusId", CusIdCb.SelectedValue);
                cmd.Parameters.AddWithValue("@CusName", CustNameTb.Text);
                cmd.Parameters.AddWithValue("@Quantity", ProQuanTb.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", UPTb.Text);
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil diperbarui.");
                DisplayBill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void BillDgv_DoubleClick(object sender, EventArgs e)
        {
            if (BillDgv.SelectedRows.Count > 0)
            {
                BillIdTb.Text = BillDgv.SelectedRows[0].Cells[0].Value.ToString();
                ProNameCb.Text = BillDgv.SelectedRows[0].Cells[1].Value.ToString();
                CusIdCb.Text = BillDgv.SelectedRows[0].Cells[2].Value.ToString();
                CustNameTb.Text = BillDgv.SelectedRows[0].Cells[3].Value.ToString();
                ProQuanTb.Text = BillDgv.SelectedRows[0].Cells[4].Value.ToString();
                UPTb.Text = BillDgv.SelectedRows[0].Cells[5].Value.ToString();
                PriceTb.Text = BillDgv.SelectedRows[0].Cells[6].Value.ToString();
            }
        }

        private void BillDgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional jika ada logika tambahan
        }

        private void ProductDgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional jika ada logika tambahan
        }
    }
}
