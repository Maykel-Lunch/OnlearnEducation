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
using System.IO;
using ClosedXML.Excel;

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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Enrolled Courses";
                    saveFileDialog.FileName = $"EnrolledCourses_{DateTime.Now:yyyyMMdd}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create a new DataTable
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Course Name", typeof(string));
                        dt.Columns.Add("Instructor", typeof(string));
                        dt.Columns.Add("Enrollment Date", typeof(string));

                        // Get data from database
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
                                    while (reader.Read())
                                    {
                                        string courseName = reader["CourseName"]?.ToString() ?? "N/A";
                                        string instructorName = reader["InstructorName"]?.ToString() ?? "N/A";
                                        string enrollmentDate = reader["EnrollmentDate"] != DBNull.Value 
                                            ? Convert.ToDateTime(reader["EnrollmentDate"]).ToString("d") 
                                            : "N/A";

                                        dt.Rows.Add(courseName, instructorName, enrollmentDate);
                                    }
                                }
                            }
                        }

                        // Create Excel file
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Enrolled Courses");
                            
                            // Add title
                            worksheet.Cell(1, 1).Value = $"Enrolled Courses for {Username.Text}";
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                            worksheet.Range(1, 1, 1, 3).Merge();

                            // Add headers
                            worksheet.Cell(3, 1).Value = "Course Name";
                            worksheet.Cell(3, 2).Value = "Instructor";
                            worksheet.Cell(3, 3).Value = "Enrollment Date";
                            worksheet.Range(3, 1, 3, 3).Style.Font.Bold = true;

                            // Add data
                            worksheet.Cell(4, 1).InsertTable(dt);

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();

                            // Save the file
                            workbook.SaveAs(saveFileDialog.FileName);
                        }

                        MessageBox.Show("Courses exported successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
