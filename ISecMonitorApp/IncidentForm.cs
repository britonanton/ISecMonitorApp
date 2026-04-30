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
    public partial class IncidentForm : Form
    {
        public IncidentForm()
        {
            InitializeComponent();
        }

        private void IncidentForm_Load(object sender, EventArgs e)
        {
            LoadIncidents();
            LoadStatuses();
        }

        private void LoadIncidents()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT incident_id FROM incidents";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cmbIncident.DataSource = table;
                cmbIncident.DisplayMember = "incident_id";
                cmbIncident.ValueMember = "incident_id";
            }
        }

        private void LoadStatuses()
        {
            cmbStatus.Items.Add("new");
            cmbStatus.Items.Add("in_progress");
            cmbStatus.Items.Add("resolved");
            cmbStatus.SelectedIndex = 0;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("sp_update_incident_status", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_incident_id", cmbIncident.SelectedValue);
                    cmd.Parameters.AddWithValue("@p_new_status", cmbStatus.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Статус обновлён");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbIncident_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}