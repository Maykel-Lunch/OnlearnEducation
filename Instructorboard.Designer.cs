namespace OnlearnEducation
{
    partial class Instructorboard
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
            btnLogout = new Button();
            btnExportExcel = new Button();
            label3 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            btnAddStudent = new Button();
            instructorGridView = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)instructorGridView).BeginInit();
            SuspendLayout();
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
            // 
            // btnExportExcel
            // 
            btnExportExcel.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            btnExportExcel.Location = new Point(639, 81);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(209, 27);
            btnExportExcel.TabIndex = 6;
            btnExportExcel.Text = "Download Courses Info";
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
            // panel1
            // 
            panel1.Controls.Add(btnLogout);
            panel1.Controls.Add(btnExportExcel);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btnAddStudent);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 114);
            panel1.TabIndex = 2;
            // 
            // btnAddStudent
            // 
            btnAddStudent.BackColor = SystemColors.ActiveCaption;
            btnAddStudent.Font = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btnAddStudent.ForeColor = SystemColors.ButtonHighlight;
            btnAddStudent.Location = new Point(687, 40);
            btnAddStudent.Name = "btnAddStudent";
            btnAddStudent.Size = new Size(161, 35);
            btnAddStudent.TabIndex = 7;
            btnAddStudent.Text = "Enroll a Student";
            btnAddStudent.UseVisualStyleBackColor = false;
            // 
            // instructorGridView
            // 
            instructorGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            instructorGridView.Dock = DockStyle.Fill;
            instructorGridView.Location = new Point(0, 114);
            instructorGridView.Name = "instructorGridView";
            instructorGridView.RowHeadersWidth = 62;
            instructorGridView.Size = new Size(884, 336);
            instructorGridView.TabIndex = 3;
            // 
            // Instructorboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 450);
            Controls.Add(instructorGridView);
            Controls.Add(panel1);
            Name = "Instructorboard";
            Text = "Instructorboard";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)instructorGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnLogout;
        private Button btnExportExcel;
        private Label label3;
        private Label label2;
        private Panel panel1;
        private DataGridView instructorGridView;
        private Button btnAddStudent;
    }
}