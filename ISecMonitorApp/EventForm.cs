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
    public partial class EventForm : Form
    {
        public EventForm()
        {
            InitializeComponent();
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            LoadResources();
            LoadSeverity();
        }

        private void LoadResources()
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT resource_id, name FROM resources ORDER BY name";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        cmbResource.DataSource = table;
                        cmbResource.DisplayMember = "name";
                        cmbResource.ValueMember = "resource_id";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ресурсов: " + ex.Message);
            }
        }

        private void LoadSeverity()
        {
            cmbSeverity.Items.Clear();
            cmbSeverity.Items.Add("low");
            cmbSeverity.Items.Add("medium");
            cmbSeverity.Items.Add("high");
            cmbSeverity.Items.Add("critical");
            cmbSeverity.SelectedIndex = 1;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (cmbResource.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtEventType.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                cmbSeverity.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("sp_add_event", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_resource_id", Convert.ToInt32(cmbResource.SelectedValue));
                        cmd.Parameters.AddWithValue("@p_user_id", AppSession.CurrentUserId);
                        cmd.Parameters.AddWithValue("@p_event_type", txtEventType.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_event_description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_severity", cmbSeverity.SelectedItem.ToString());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Событие успешно добавлено.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления события: " + ex.Message);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbResource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }
    }
}