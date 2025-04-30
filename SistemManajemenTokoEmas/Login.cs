using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            // Sembunyikan karakter password saat mengetik
            PasswordTb.UseSystemPasswordChar = true;
        }

        // Tombol tutup
        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Tombol reset
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            UNameTb.Text = "";
            PasswordTb.Text = "";
        }

        // Tombol login
        private void LogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasi input kosong
                if (UNameTb.Text.Trim() == "" || PasswordTb.Text.Trim() == "")
                {
                    MessageBox.Show("Nama pengguna atau kata sandi tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // Login berhasil
                else if (UNameTb.Text == "Admin" && PasswordTb.Text == "Password")
                {
                    MessageBox.Show("Berhasil Masuk!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Customer obj = new Customer(); 
                    obj.Show();
                    this.Hide();
                }
                // Login gagal
                else
                {
                    MessageBox.Show("Tolong masukkan nama dan password yang benar.", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Checkbox untuk melihat password
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTb.UseSystemPasswordChar = !checkBox1.Checked;
        }

        // Klik pada gambar (opsional)
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Bisa tambahkan logika lain jika dibutuhkan
        }
    }
}
