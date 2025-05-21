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
using ClosedXML.Excel;
using System.IO;

namespace OnlearnEducation
{
    public partial class Instructorboard : Form
    {
        private int _userId;
        private string _username;
        private string _email;
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";

        public Instructorboard(int userId, string username, string email)
        {
            InitializeComponent();
            _userId = userId;
            _username = username;
            _email = email;

            // Set window title with instructor name
            this.Text = $"Instructor Dashboard - {username}";

            // Set labels
            label2.Text = username;
            label3.Text = email;

            // Add event handlers
            btnLogout.Click += btnLogout_Click_1;
            // button1.Click += button1_Click;
            btnExportExcel.Click += btnExportExcel_Click;
            btnAddStudent.Click += btnAddStudent_Click;

            // Load instructor data when form loads
            this.Load += (s, e) => LoadInstructorData();
        }

        private void LoadInstructorData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                        CourseID,
                        CourseName,
                        Enrolled_Students,
                        Active_Assignments,
                        Ungraded_Submissions,
                        Avg_Feedback_Rating,
                        Next_Assignment_Due,
                        Quiz_Count,
                        Course_Description,
                        Submission_Rate_Pct
                    FROM 
                        instructor_course_overview
                    WHERE 
                        CourseID IN (SELECT CourseID FROM courses WHERE CreatedBy = @UserId)
                    ORDER BY 
                        CASE WHEN Next_Assignment_Due IS NULL THEN 1 ELSE 0 END,
                        Next_Assignment_Due ASC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);
                        connection.Open();

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            instructorGridView.DataSource = dt;

                            // Configure grid view
                            instructorGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            instructorGridView.AllowUserToAddRows = false;
                            instructorGridView.AllowUserToDeleteRows = false;
                            instructorGridView.ReadOnly = true;
                            instructorGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            instructorGridView.MultiSelect = false;
                            instructorGridView.RowHeadersVisible = false;

                            // Format columns
                            foreach (DataGridViewColumn column in instructorGridView.Columns)
                            {
                                switch (column.Name)
                                {
                                    case "Next_Assignment_Due":
                                        column.DefaultCellStyle.Format = "MM/dd/yyyy";
                                        break;
                                    case "Avg_Feedback_Rating":
                                        column.DefaultCellStyle.Format = "N2";
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading instructor data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Show the login form
                OnLearnLoginForm loginForm = new OnLearnLoginForm();
                loginForm.Show();

                // Close the current form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Password change functionality will be implemented here
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Instructor Dashboard";
                    saveFileDialog.FileName = $"InstructorDashboard_{DateTime.Now:yyyyMMdd}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the template file path
                        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "InstructorDashboard_Template.xlsx");
                        
                        if (!File.Exists(templatePath))
                        {
                            MessageBox.Show($"Template file not found at: {templatePath}\nPlease ensure InstructorDashboard_Template.xlsx exists in the Templates folder.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create a copy of the template
                        File.Copy(templatePath, saveFileDialog.FileName, true);

                        // Open the copied file
                        using (XLWorkbook workbook = new XLWorkbook(saveFileDialog.FileName))
                        {
                            var worksheet = workbook.Worksheet("Data");
                            var dt = (DataTable)instructorGridView.DataSource;

                            // Insert data starting at row 8 (after headers)
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    var cell = worksheet.Cell(i + 8, j + 1);
                                    cell.Value = dt.Rows[i][j].ToString();

                                    // Apply specific formatting based on column type
                                    if (dt.Columns[j].ColumnName.Contains("Date"))
                                    {
                                        cell.Style.NumberFormat.Format = "mm/dd/yyyy";
                                    }
                                    else if (dt.Columns[j].ColumnName == "Avg_Feedback_Rating")
                                    {
                                        cell.Style.NumberFormat.Format = "0.00";
                                    }
                                    else if (dt.Columns[j].ColumnName == "Submission_Rate_Pct")
                                    {
                                        cell.Style.NumberFormat.Format = "0.00%";
                                    }
                                }
                            }

                            // Update summary sheet
                            var summarySheet = workbook.Worksheet("Summary");
                            summarySheet.Cell("A3").Value = dt.Rows.Count; // Total courses

                            // Calculate total enrolled students
                            double totalStudents = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["Enrolled_Students"] != DBNull.Value)
                                {
                                    totalStudents += Convert.ToDouble(row["Enrolled_Students"]);
                                }
                            }
                            summarySheet.Cell("A4").Value = totalStudents;

                            // Calculate average feedback rating
                            double totalRating = 0;
                            int ratingCount = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["Avg_Feedback_Rating"] != DBNull.Value)
                                {
                                    totalRating += Convert.ToDouble(row["Avg_Feedback_Rating"]);
                                    ratingCount++;
                                }
                            }
                            summarySheet.Cell("A5").Value = ratingCount > 0 ? totalRating / ratingCount : 0;
                            summarySheet.Cell("A5").Style.NumberFormat.Format = "0.00";

                            // Calculate average submission rate
                            double totalSubmissionRate = 0;
                            int submissionCount = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["Submission_Rate_Pct"] != DBNull.Value)
                                {
                                    totalSubmissionRate += Convert.ToDouble(row["Submission_Rate_Pct"]);
                                    submissionCount++;
                                }
                            }
                            summarySheet.Cell("A6").Value = submissionCount > 0 ? totalSubmissionRate / submissionCount : 0;
                            summarySheet.Cell("A6").Style.NumberFormat.Format = "0.00%";

                            // Save the file
                            workbook.Save();
                        }

                        MessageBox.Show("Dashboard exported successfully!", "Success",
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

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            try
            {
                using (EnrollStudentForm enrollForm = new EnrollStudentForm(_userId))
                {
                    if (enrollForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the instructor data to show updated enrollment
                        LoadInstructorData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening enrollment form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
