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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Icon = new Icon("appicon.ico");
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {
            lblUser.Text = $"Пользователь: {AppSession.CurrentFullName} ({AppSession.CurrentRole})";
            ConfigureAccess();    
        }

        private void lblUser_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void LoadEvents()
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            event_id,
                            event_type,
                            event_description,
                            severity,
                            event_time,
                            resource_name,
                            login,
                            full_name,
                            role_name
                        FROM v_events_full
                        ORDER BY event_time DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void ConfigureAccess()
        {
            if (AppSession.CurrentRole == "analyst")
            {
                btnDeleteResource.Visible = false;
            }

            if (AppSession.CurrentRole == "operator")
            {
                btnDeleteResource.Visible = false;
            }

            if (AppSession.CurrentRole == "admin")
            {
                btnDeleteResource.Visible = true;
            }
        }

        private void btnDeleteResource_Click(object sender, EventArgs e)
        {
            ResourceForm form = new ResourceForm();
            form.ShowDialog();
            LoadEvents();
        }

        private void btnAddEvent_Click_1(object sender, EventArgs e)
        {
            EventForm form = new EventForm();
            form.ShowDialog();
            LoadEvents();
        }

        private void btnUpdateIncident_Click_1(object sender, EventArgs e)
        {
            IncidentForm form = new IncidentForm();
            form.ShowDialog();
            LoadEvents();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            LoadEvents();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
