using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OnlearnEducation
{
    public partial class ProfileDashboard : Form
    {
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";
        private int _userId;

        public ProfileDashboard(int userId, string username, string email)
        {
            InitializeComponent();
            _userId = userId;
            Username.Text = username;
            UserEmail.Text = email;

            // Load data when form loads
            this.Load += (s, e) => LoadEnrollmentData();
        }

        private void LoadEnrollmentData()
        {
            try
            {
                // Clear existing controls
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                // Set up table structure
                tableLayoutPanel1.ColumnCount = 3;
                tableLayoutPanel1.RowCount = 1; // Start with header row

                // Configure column widths
                tableLayoutPanel1.ColumnStyles.Clear(); // Clear existing styles
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));

                // Add headers
                AddHeaderLabel("Course Name", 0, 0);
                AddHeaderLabel("Instructor", 1, 0);
                AddHeaderLabel("Enrollment Date", 2, 0);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                            CourseName,
                            InstructorName,
                            EnrollmentDate
                        FROM 
                            userenrollmentswithinstructor
                        WHERE
                            UserID = @UserId";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int rowIndex = 1; // Start after header

                            while (reader.Read())
                            {
                                tableLayoutPanel1.RowCount++;
                                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                                // Add null checks and default values
                                string courseName = reader["CourseName"]?.ToString() ?? "N/A";
                                string instructorName = reader["InstructorName"]?.ToString() ?? "N/A";
                                string enrollmentDate = reader["EnrollmentDate"] != DBNull.Value 
                                    ? Convert.ToDateTime(reader["EnrollmentDate"]).ToString("d") 
                                    : "N/A";

                                AddDataLabel(courseName, 0, rowIndex);
                                AddDataLabel(instructorName, 1, rowIndex);
                                AddDataLabel(enrollmentDate, 2, rowIndex);

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollment data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddHeaderLabel(string text, int column, int row)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            tableLayoutPanel1.Controls.Add(label, column, row);
        }

        private void AddDataLabel(string text, int column, int row)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(5, 3, 5, 3)
            };
            tableLayoutPanel1.Controls.Add(label, column, row);
        }
    }
}
