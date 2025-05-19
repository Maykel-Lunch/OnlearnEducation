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
            // Create a top panel for user details and button
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;
            topPanel.Padding = new Padding(10);

            // button1
            button1.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(720, 20);
            button1.Name = "button1";
            button1.Size = new Size(207, 34);
            button1.TabIndex = 0;
            button1.Text = "Change my password? ";
            button1.UseVisualStyleBackColor = true;
            topPanel.Controls.Add(button1);

            // Username
            Username.AutoSize = true;
            Username.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Username.Location = new Point(20, 20);
            Username.Name = "Username";
            Username.Size = new Size(78, 32);
            Username.TabIndex = 1;
            Username.Text = "label1";
            topPanel.Controls.Add(Username);

            // UserEmail
            UserEmail.AutoSize = true;
            UserEmail.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            UserEmail.Location = new Point(20, 52);
            UserEmail.Name = "UserEmail";
            UserEmail.Size = new Size(53, 21);
            UserEmail.TabIndex = 2;
            UserEmail.Text = "label2";
            topPanel.Controls.Add(UserEmail);

            // panel1 (for table)
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;
            panel1.Padding = new Padding(10);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Name = "panel1";
            panel1.TabIndex = 3;

            // tableLayoutPanel1
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.Padding = new Padding(5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.TabIndex = 0;

            // ProfileDashboard
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(956, 450);
            Controls.Add(panel1);
            Controls.Add(topPanel);
            Name = "ProfileDashboard";
            Text = "Dashboard";
            MinimumSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label Username;
        private Label UserEmail;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
    }
}