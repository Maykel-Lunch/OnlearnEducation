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
    public partial class Studentboard : Form
    {
        private int _userId;
        private string _username;
        private string _email;
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";

        public Studentboard(int userId, string username, string email)
        {
            InitializeComponent();
            _userId = userId;
            _username = username;
            _email = email;
            
            // Set window title with student name
            this.Text = $"Student Dashboard - {username}";
            
            // Set labels
            label2.Text = username;
            label3.Text = email;

            // Add event handlers
            btnLogout.Click += btnLogout_Click_1;
            // button1.Click += button1_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            // Load student data when form loads
            this.Load += (s, e) => LoadStudentData();
        }

        private void LoadStudentData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                            CourseName,
                            CourseDescription,
                            InstructorName,
                            NextAssignment,
                            NextAssignmentDueDate,
                            DATEDIFF(NextAssignmentDueDate, CURDATE()) AS DaysRemaining,
                            RecentGrade,
                            NextQuiz,
                            QuizTotalMarks,
                            FeedbackRating
                        FROM student_dashboard 
                        WHERE UserID = @userId
                        ORDER BY 
                            CASE WHEN NextAssignmentDueDate IS NULL THEN 1 ELSE 0 END,
                            NextAssignmentDueDate ASC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);
                        connection.Open();

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            studentGridView.DataSource = dt;

                            // Configure grid view
                            studentGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            studentGridView.AllowUserToAddRows = false;
                            studentGridView.AllowUserToDeleteRows = false;
                            studentGridView.ReadOnly = true;
                            studentGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            studentGridView.MultiSelect = false;
                            studentGridView.RowHeadersVisible = false;

                            // Format date columns
                            foreach (DataGridViewColumn column in studentGridView.Columns)
                            {
                                if (column.Name.Contains("Date"))
                                {
                                    column.DefaultCellStyle.Format = "MM/dd/yyyy";
                                }
                                else if (column.Name == "FeedbackRating")
                                {
                                    column.DefaultCellStyle.Format = "N2";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}", "Error",
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
                    saveFileDialog.Title = "Save Student Dashboard";
                    saveFileDialog.FileName = $"StudentDashboard_{DateTime.Now:yyyyMMdd}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the template file path
                        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "StudentDashboard_Template.xlsx");
                        
                        if (!File.Exists(templatePath))
                        {
                            MessageBox.Show($"Template file not found at: {templatePath}\nPlease ensure StudentDashboard_Template.xlsx exists in the Templates folder.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create a copy of the template
                        File.Copy(templatePath, saveFileDialog.FileName, true);

                        // Open the copied file
                        using (XLWorkbook workbook = new XLWorkbook(saveFileDialog.FileName))
                        {
                            var worksheet = workbook.Worksheet("Data");
                            var dt = (DataTable)studentGridView.DataSource;

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
                                    else if (dt.Columns[j].ColumnName == "FeedbackRating" || 
                                            dt.Columns[j].ColumnName == "RecentGrade")
                                    {
                                        cell.Style.NumberFormat.Format = "0.00";
                                    }
                                }
                            }

                            // Update summary sheet
                            var summarySheet = workbook.Worksheet("Summary");
                            summarySheet.Cell("A3").Value = dt.Rows.Count; // Total courses

                            // Calculate average grade
                            double totalGrade = 0;
                            int gradeCount = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["RecentGrade"] != DBNull.Value)
                                {
                                    totalGrade += Convert.ToDouble(row["RecentGrade"]);
                                    gradeCount++;
                                }
                            }
                            summarySheet.Cell("A4").Value = gradeCount > 0 ? totalGrade / gradeCount : 0;
                            summarySheet.Cell("A4").Style.NumberFormat.Format = "0.00";

                            // Calculate pass rate
                            int passCount = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["RecentGrade"] != DBNull.Value && Convert.ToDouble(row["RecentGrade"]) >= 60)
                                {
                                    passCount++;
                                }
                            }
                            summarySheet.Cell("A5").Value = dt.Rows.Count > 0 ? (double)passCount / dt.Rows.Count : 0;
                            summarySheet.Cell("A5").Style.NumberFormat.Format = "0.00%";

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
    }
}
