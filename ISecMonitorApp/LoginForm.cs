using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISecMonitorApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Icon = new Icon("appicon.ico");
            this.Text = "ISecMonitor — Вход в систему";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT user_id, full_name, role_name, password_salt, password_hash
                FROM users
                WHERE login = @login AND is_active = 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Пользователь не найден");
                                return;
                            }

                            int userId = reader.GetInt32("user_id");
                            string fullName = reader.GetString("full_name");
                            string role = reader.GetString("role_name");
                            string salt = reader.GetString("password_salt");
                            string hash = reader.GetString("password_hash");

                            string inputHash = PasswordHelper.HashPassword(password, salt);

                            if (inputHash == hash)
                            {
                                
                                AppSession.CurrentUserId = userId;
                                AppSession.CurrentLogin = login;
                                AppSession.CurrentFullName = fullName;
                                AppSession.CurrentRole = role;
                                AppSession.IsAuthorized = true;

                                MainForm mainForm = new MainForm();
                                this.Hide();

                                mainForm.FormClosed += (s, args) =>
                                {
                                    this.Show();
                                    txtPassword.Clear();
                                };

                                mainForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Неверный пароль");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtLogin.Font = new Font("Segoe UI", 14F);
            txtPassword.Font = new Font("Segoe UI", 14F);

            txtLogin.Multiline = true;
            txtPassword.Multiline = true;

            txtLogin.Height = 32;
            txtPassword.Height = 32;

            txtLogin.BorderStyle = BorderStyle.None;
            txtPassword.BorderStyle = BorderStyle.None;

        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;

            if (txtPassword.UseSystemPasswordChar)
                pictureBoxEye.Image = Properties.Resources.eye_closed;
            else
                pictureBoxEye.Image = Properties.Resources.eye_open;

            txtPassword.Focus();
            txtPassword.SelectionStart = txtPassword.Text.Length;
        }
    }
}
