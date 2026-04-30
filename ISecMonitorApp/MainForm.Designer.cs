namespace ISecMonitorApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblUser = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.btnUpdateIncident = new System.Windows.Forms.Button();
            this.btnDeleteResource = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUser.Location = new System.Drawing.Point(12, 417);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(144, 24);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Пользователь:";
            this.lblUser.Click += new System.EventHandler(this.lblUser_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(799, 336);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnLoad
            // 
            this.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(12, 343);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(130, 62);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Загрузить данные";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click_1);
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddEvent.Location = new System.Drawing.Point(148, 343);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(130, 62);
            this.btnAddEvent.TabIndex = 4;
            this.btnAddEvent.Text = "Добавить событие";
            this.btnAddEvent.UseVisualStyleBackColor = true;
            this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click_1);
            // 
            // btnUpdateIncident
            // 
            this.btnUpdateIncident.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUpdateIncident.Location = new System.Drawing.Point(284, 343);
            this.btnUpdateIncident.Name = "btnUpdateIncident";
            this.btnUpdateIncident.Size = new System.Drawing.Size(130, 62);
            this.btnUpdateIncident.TabIndex = 5;
            this.btnUpdateIncident.Text = "Изменить статус";
            this.btnUpdateIncident.UseVisualStyleBackColor = true;
            this.btnUpdateIncident.Click += new System.EventHandler(this.btnUpdateIncident_Click_1);
            // 
            // btnDeleteResource
            // 
            this.btnDeleteResource.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeleteResource.Location = new System.Drawing.Point(420, 343);
            this.btnDeleteResource.Name = "btnDeleteResource";
            this.btnDeleteResource.Size = new System.Drawing.Size(130, 62);
            this.btnDeleteResource.TabIndex = 6;
            this.btnDeleteResource.Text = "Удалить ресурс";
            this.btnDeleteResource.UseVisualStyleBackColor = true;
            this.btnDeleteResource.Click += new System.EventHandler(this.btnDeleteResource_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLogout.Location = new System.Drawing.Point(688, 391);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 50);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Выход";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnDeleteResource);
            this.Controls.Add(this.btnUpdateIncident);
            this.Controls.Add(this.btnAddEvent);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ISecMonitor — Мониторинг событий безопасности";
            this.Load += new System.EventHandler(this.MainForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAddEvent;
        private System.Windows.Forms.Button btnUpdateIncident;
        private System.Windows.Forms.Button btnDeleteResource;
        private System.Windows.Forms.Button btnLogout;
    }
}