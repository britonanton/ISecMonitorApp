using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ISecMonitorApp
{
    public partial class ResourceForm : Form
    {
        public ResourceForm()
        {
            InitializeComponent();
        }

        private void ResourceForm_Load(object sender, EventArgs e)
        {
            LoadResources();
        }

        private void LoadResources()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT resource_id, name FROM resources ORDER BY name";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cmbResource.DataSource = table;
                cmbResource.DisplayMember = "name";
                cmbResource.ValueMember = "resource_id";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbResource.SelectedValue == null)
            {
                MessageBox.Show("Выберите ресурс.");
                return;
            }

            var confirm = MessageBox.Show(
                "Удалить выбранный ресурс?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM resources WHERE resource_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cmbResource.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Ресурс удалён");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}