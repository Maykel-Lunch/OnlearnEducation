using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OnlearnEducation
{
    public partial class EnrollStudentForm : Form
    {
        private int _instructorId;
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";
        private ComboBox cmbCourses;
        private TextBox txtStudentName;
        private Button btnEnroll;
        private Button btnCancel;
        private Label lblCourse;
        private Label lblStudentName;

        public EnrollStudentForm(int instructorId)
        {
            InitializeComponent();
            _instructorId = instructorId;
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.cmbCourses = new ComboBox();
            this.txtStudentName = new TextBox();
            this.btnEnroll = new Button();
            this.btnCancel = new Button();
            this.lblCourse = new Label();
            this.lblStudentName = new Label();

            // Form settings
            this.Text = "Enroll Student";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Course label
            this.lblCourse.AutoSize = true;
            this.lblCourse.Location = new System.Drawing.Point(20, 20);
            this.lblCourse.Size = new System.Drawing.Size(100, 20);
            this.lblCourse.Text = "Select Course:";

            // Course combo box
            this.cmbCourses.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCourses.Location = new System.Drawing.Point(20, 45);
            this.cmbCourses.Size = new System.Drawing.Size(340, 25);
            this.cmbCourses.FormattingEnabled = true;

            // Student name label
            this.lblStudentName.AutoSize = true;
            this.lblStudentName.Location = new System.Drawing.Point(20, 80);
            this.lblStudentName.Size = new System.Drawing.Size(100, 20);
            this.lblStudentName.Text = "Student Name:";

            // Student name text box
            this.txtStudentName.Location = new System.Drawing.Point(20, 105);
            this.txtStudentName.Size = new System.Drawing.Size(340, 25);

            // Enroll button
            this.btnEnroll.Location = new System.Drawing.Point(20, 150);
            this.btnEnroll.Size = new System.Drawing.Size(160, 35);
            this.btnEnroll.Text = "Enroll Student";
            this.btnEnroll.Click += new EventHandler(btnEnroll_Click);

            // Cancel button
            this.btnCancel.Location = new System.Drawing.Point(200, 150);
            this.btnCancel.Size = new System.Drawing.Size(160, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                this.lblCourse,
                this.cmbCourses,
                this.lblStudentName,
                this.txtStudentName,
                this.btnEnroll,
                this.btnCancel
            });
        }

        private void LoadCourses()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT CourseID, CourseName 
                                   FROM courses 
                                   WHERE CreatedBy = @InstructorId
                                   ORDER BY CourseName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InstructorId", _instructorId);
                        connection.Open();

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            cmbCourses.DisplayMember = "CourseName";
                            cmbCourses.ValueMember = "CourseID";
                            cmbCourses.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Please enter a student name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCourses.SelectedValue == null)
            {
                MessageBox.Show("Please select a course.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // First, create the user
                    string createUserQuery = @"INSERT INTO users (Username, UserType) 
                                             VALUES (@Username, 'student')";

                    using (MySqlCommand createUserCmd = new MySqlCommand(createUserQuery, connection))
                    {
                        createUserCmd.Parameters.AddWithValue("@Username", txtStudentName.Text);
                        createUserCmd.ExecuteNonQuery();

                        // Get the new user's ID
                        long newUserId = createUserCmd.LastInsertedId;

                        // Then enroll the user in the course
                        string enrollQuery = @"CALL EnrollUser(@UserID, @CourseID)";

                        using (MySqlCommand enrollCmd = new MySqlCommand(enrollQuery, connection))
                        {
                            enrollCmd.Parameters.AddWithValue("@UserID", newUserId);
                            enrollCmd.Parameters.AddWithValue("@CourseID", cmbCourses.SelectedValue);
                            enrollCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Student enrolled successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enrolling student: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 