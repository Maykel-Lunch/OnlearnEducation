namespace OnlearnEducation
{
    partial class Adminboard
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
            panel1 = new Panel();
            btnLogout = new Button();
            button1 = new Button();
            btnExportExcel = new Button();
            label3 = new Label();
            label2 = new Label();
            AdminControl = new TabControl();
            tabPage1 = new TabPage();
            systemOverviewGridView = new DataGridView();
            tabPage2 = new TabPage();
            studentPerformanceGridView = new DataGridView();
            tabPage3 = new TabPage();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            AdminControl.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)systemOverviewGridView).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)studentPerformanceGridView).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLogout);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnExportExcel);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(885, 114);
            panel1.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Red;
            btnLogout.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = SystemColors.ControlLightLight;
            btnLogout.ImageKey = "(none)";
            btnLogout.Location = new Point(736, 3);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(112, 34);
            btnLogout.TabIndex = 10;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click_1;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(639, 43);
            button1.Name = "button1";
            button1.Size = new Size(209, 31);
            button1.TabIndex = 7;
            button1.Text = "Change my password? ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            btnExportExcel.Location = new Point(639, 79);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(209, 27);
            btnExportExcel.TabIndex = 6;
            btnExportExcel.Text = "Download Reports";
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 57);
            label3.Name = "label3";
            label3.Size = new Size(53, 21);
            label3.TabIndex = 3;
            label3.Text = "label2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 32);
            label2.Name = "label2";
            label2.Size = new Size(59, 25);
            label2.TabIndex = 0;
            label2.Text = "label1";
            // 
            // AdminControl
            // 
            AdminControl.Controls.Add(tabPage1);
            AdminControl.Controls.Add(tabPage2);
            AdminControl.Controls.Add(tabPage3);
            AdminControl.Dock = DockStyle.Fill;
            AdminControl.Location = new Point(0, 114);
            AdminControl.Name = "AdminControl";
            AdminControl.SelectedIndex = 0;
            AdminControl.Size = new Size(885, 336);
            AdminControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(systemOverviewGridView);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(877, 298);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "System Overview";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // systemOverviewGridView
            // 
            systemOverviewGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            systemOverviewGridView.Dock = DockStyle.Fill;
            systemOverviewGridView.Location = new Point(3, 3);
            systemOverviewGridView.Name = "systemOverviewGridView";
            systemOverviewGridView.RowHeadersWidth = 62;
            systemOverviewGridView.Size = new Size(871, 292);
            systemOverviewGridView.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(studentPerformanceGridView);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(877, 280);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Course Analytics";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // studentPerformanceGridView
            // 
            studentPerformanceGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            studentPerformanceGridView.Dock = DockStyle.Fill;
            studentPerformanceGridView.Location = new Point(3, 3);
            studentPerformanceGridView.Name = "studentPerformanceGridView";
            studentPerformanceGridView.RowHeadersWidth = 62;
            studentPerformanceGridView.Size = new Size(871, 274);
            studentPerformanceGridView.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dataGridView1);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(877, 280);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Student Performance";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(871, 274);
            dataGridView1.TabIndex = 0;
            // 
            // Adminboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(885, 450);
            Controls.Add(AdminControl);
            Controls.Add(panel1);
            Name = "Adminboard";
            Text = "Adminboard";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            AdminControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)systemOverviewGridView).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)studentPerformanceGridView).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button btnExportExcel;
        private Button button1;
        private Label label3;
        private Label label2;
        private Button btnLogout;
        private TabControl AdminControl;
        private TabPage tabPage1;
        private DataGridView systemOverviewGridView;
        private TabPage tabPage2;
        private DataGridView studentPerformanceGridView;
        private TabPage tabPage3;
        private DataGridView dataGridView1;
    }
}