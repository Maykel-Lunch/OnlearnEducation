using ClosedXML.Excel;

namespace OnlearnEducation
{
    partial class ProfileDashboard
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
            button1 = new Button();
            Username = new Label();
            UserEmail = new Label();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            topPanel = new Panel();
            btnExportExcel = new Button();
            panel1.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(742, 12);
            button1.Name = "button1";
            button1.Size = new Size(201, 34);
            button1.TabIndex = 0;
            button1.Text = "Change my password? ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Username
            // 
            Username.AutoSize = true;
            Username.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Username.Location = new Point(20, 20);
            Username.Name = "Username";
            Username.Size = new Size(78, 32);
            Username.TabIndex = 1;
            Username.Text = "label1";
            // 
            // UserEmail
            // 
            UserEmail.AutoSize = true;
            UserEmail.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            UserEmail.Location = new Point(20, 52);
            UserEmail.Name = "UserEmail";
            UserEmail.Size = new Size(53, 21);
            UserEmail.TabIndex = 2;
            UserEmail.Text = "label2";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 80);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10);
            panel1.Size = new Size(956, 464);
            panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.Location = new Point(10, 10);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(0);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // topPanel
            // 
            topPanel.Controls.Add(button1);
            topPanel.Controls.Add(Username);
            topPanel.Controls.Add(UserEmail);
            topPanel.Controls.Add(btnExportExcel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(10);
            topPanel.Size = new Size(956, 80);
            topPanel.TabIndex = 4;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            btnExportExcel.Location = new Point(690, 46);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(253, 27);
            btnExportExcel.TabIndex = 5;
            btnExportExcel.Text = "Download Enrolled Course Info";
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // ProfileDashboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(956, 544);
            Controls.Add(panel1);
            Controls.Add(topPanel);
            MinimumSize = new Size(800, 600);
            Name = "ProfileDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label Username;
        private Label UserEmail;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel topPanel;
        private Button btnExportExcel;
    }
}