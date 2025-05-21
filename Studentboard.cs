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
                        // Create a new DataTable
                        DataTable dt = (DataTable)studentGridView.DataSource;

                        // Create Excel file
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Student Dashboard");
                            
                            // Add title
                            worksheet.Cell(1, 1).Value = $"Student Dashboard for {_username}";
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                            worksheet.Range(1, 1, 1, dt.Columns.Count).Merge();

                            // Add data
                            worksheet.Cell(3, 1).InsertTable(dt);

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();

                            // Add a chart for grades
                            if (dt.Columns.Contains("RecentGrade"))
                            {
                                var chart = worksheet.Workbook.Worksheets.Add($"{worksheet.Name} Charts");
                                chart.Cell(1, 1).Value = "Grade Analysis";
                                chart.Cell(1, 1).Style.Font.Bold = true;
                                chart.Cell(1, 1).Style.Font.FontSize = 14;

                                // Prepare data for charts
                                int dataRow = 3;
                                chart.Cell(dataRow, 1).Value = "Course";
                                chart.Cell(dataRow, 2).Value = "Grade";
                                chart.Cell(dataRow, 3).Value = "Status";
                                chart.Cell(dataRow, 4).Value = "Percentage";
                                chart.Range(dataRow, 1, dataRow, 4).Style.Font.Bold = true;

                                // Add data for charts
                                double totalGrade = 0;
                                int passCount = 0;
                                int failCount = 0;

                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["RecentGrade"] != DBNull.Value)
                                    {
                                        dataRow++;
                                        double grade = Convert.ToDouble(row["RecentGrade"]);
                                        string status = grade >= 60 ? "Passed" : "Failed";
                                        
                                        chart.Cell(dataRow, 1).Value = row["CourseName"].ToString();
                                        chart.Cell(dataRow, 2).Value = grade;
                                        chart.Cell(dataRow, 3).Value = status;
                                        
                                        totalGrade += grade;
                                        if (status == "Passed") passCount++;
                                        else failCount++;
                                    }
                                }

                                // Add summary statistics
                                dataRow += 2;
                                chart.Cell(dataRow, 1).Value = "Summary Statistics";
                                chart.Cell(dataRow, 1).Style.Font.Bold = true;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "Total Courses:";
                                chart.Cell(dataRow, 2).Value = dt.Rows.Count;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "Average Grade:";
                                chart.Cell(dataRow, 2).Value = totalGrade / dt.Rows.Count;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "Pass Rate:";
                                chart.Cell(dataRow, 2).Value = (double)passCount / dt.Rows.Count;
                                chart.Cell(dataRow, 2).Style.NumberFormat.Format = "0.00%";

                                // Add instructions for creating charts
                                dataRow += 2;
                                chart.Cell(dataRow, 1).Value = "To create charts in Excel:";
                                chart.Cell(dataRow, 1).Style.Font.Bold = true;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "1. Select the data range (A3:D" + (dataRow - 2) + ")";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "2. Go to Insert > Charts";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "3. Choose Bar Chart for Grade distribution";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "4. Choose Pie Chart for Pass/Fail distribution";

                                // Auto-fit columns
                                chart.Columns().AdjustToContents();
                            }

                            // Save the file
                            workbook.SaveAs(saveFileDialog.FileName);
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
