using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
            DisplayCustomer();
        }

        
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\muham\OneDrive\Documents\SistemManajemenTokoEmas.mdf;Integrated Security=True;Connect Timeout=30";

       
        private void DisplayCustomer()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM [Customer]";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        CustomerDgv.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (CusIdTb.Text == "" || CusNameTb.Text == "" || CusPhoneTb.Text == "")
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO [Customer] (CusId, CusName, CusPhone) VALUES (@ID, @Name, @Phone)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", CusIdTb.Text);
                        cmd.Parameters.AddWithValue("@Name", CusNameTb.Text);
                        cmd.Parameters.AddWithValue("@Phone", CusPhoneTb.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil ditambahkan.");
                DisplayCustomer();
                ResetBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menambah data: " + ex.Message);
            }
        }

       
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (CusIdTb.Text == "" || CusNameTb.Text == "" || CusPhoneTb.Text == "")
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE [Customer] SET CusName = @Name, CusPhone = @Phone WHERE CusId = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", CusIdTb.Text);
                        cmd.Parameters.AddWithValue("@Name", CusNameTb.Text);
                        cmd.Parameters.AddWithValue("@Phone", CusPhoneTb.Text);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            MessageBox.Show("Data berhasil diperbarui.");
                        else
                            MessageBox.Show("Data tidak ditemukan.");
                    }
                }

                DisplayCustomer();
                ResetBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mengupdate data: " + ex.Message);
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (CusIdTb.Text == "")
            {
                MessageBox.Show("ID pelanggan harus diisi.");
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM [Customer] WHERE CusId = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", CusIdTb.Text);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            MessageBox.Show("Data berhasil dihapus.");
                        else
                            MessageBox.Show("Data tidak ditemukan.");
                    }
                }

                DisplayCustomer();
                ResetBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghapus data: " + ex.Message);
            }
        }

        
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            CusIdTb.Text = "";
            CusNameTb.Text = "";
            CusPhoneTb.Text = "";
        }

        
        private void CustomerDgv_DoubleClick(object sender, EventArgs e)
        {
            if (CustomerDgv.CurrentRow != null && CustomerDgv.CurrentRow.Index != -1)
            {
                CusIdTb.Text = CustomerDgv.CurrentRow.Cells["CusId"].Value.ToString();
                CusNameTb.Text = CustomerDgv.CurrentRow.Cells["CusName"].Value.ToString();
                CusPhoneTb.Text = CustomerDgv.CurrentRow.Cells["CusPhone"].Value.ToString();
            }
        }

        
        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void Productlbl_Click(object sender, EventArgs e)
        {
            Product obj = new Product();
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

        private void Customer_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
