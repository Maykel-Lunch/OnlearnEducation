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
    public partial class Adminboard : Form
    {
        private int _userId;
        private string _username;
        private string _email;
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";
        // private System.Windows.Forms.DataGridView auditLogGridView;

        public Adminboard(int userId, string username, string email)
        {
            InitializeComponent();
            _userId = userId;
            _username = username;
            _email = email;
            
            // Set window title with admin name
            this.Text = $"Admin Dashboard - {username}";
            
            // Set labels
            label2.Text = username;
            label3.Text = email;

            // Add event handlers
            btnLogout.Click += btnLogout_Click_1;
            // button1.Click += button1_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            // Load data when form loads
            this.Load += (s, e) => LoadAllData();
        }

        private void LoadAllData()
        {
            LoadSystemOverview();
            LoadCourseAnalytics();
            LoadStudentPerformance();
            LoadAuditLogs();
        }

        private void LoadSystemOverview()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                        New_Enrollments_This_Week,
                        Total_Active_Users,
                        Courses_Without_Assignments,
                        Avg_Feedback_Rating,
                        Ungraded_Submissions_Pct,
                        Recent_Audit_Entry,
                        Inactive_Instructors,
                        High_Enrollment_Courses,
                        Password_Reset_Requests,
                        System_Storage_Usage_Estimate
                    FROM admin_system_overview";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            systemOverviewGridView.DataSource = dt;

                            ConfigureGridView(systemOverviewGridView);
                            FormatSystemOverviewColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading system overview: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCourseAnalytics()
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
                        Recent_Feedback,
                        Next_Assignment_Due,
                        Quiz_Count,
                        Course_Description,
                        Submission_Rate_Pct
                    FROM instructor_course_analytics
                    ORDER BY Enrolled_Students DESC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            studentPerformanceGridView.DataSource = dt;

                            ConfigureGridView(studentPerformanceGridView);
                            FormatCourseAnalyticsColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course analytics: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentPerformance()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                        UserID,
                        Student_Name,
                        Courses_Taken,
                        Avg_Grade,
                        Assignments_Missed,
                        Feedback_Responsiveness,
                        Total_Quiz_Questions,
                        Best_Performing_Course,
                        Last_Active_Date,
                        Alert_Status
                    FROM student_performance_summary
                    ORDER BY Avg_Grade DESC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;

                            ConfigureGridView(dataGridView1);
                            FormatStudentPerformanceColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student performance: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAuditLogs()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                        log_id,
                        action,
                        table_name,
                        record_id,
                        timestamp
                    FROM audit_log
                    ORDER BY timestamp DESC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            auditLogGridView.DataSource = dt;

                            ConfigureGridView(auditLogGridView);
                            FormatAuditLogColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audit logs: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridView(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
        }

        private void FormatSystemOverviewColumns()
        {
            foreach (DataGridViewColumn column in systemOverviewGridView.Columns)
            {
                switch (column.Name)
                {
                    case "Avg_Feedback_Rating":
                        column.DefaultCellStyle.Format = "N2";
                        break;
                    case "Ungraded_Submissions_Pct":
                    case "System_Storage_Usage_Estimate":
                        column.DefaultCellStyle.Format = "P2";
                        break;
                }
            }
        }

        private void FormatCourseAnalyticsColumns()
        {
            foreach (DataGridViewColumn column in studentPerformanceGridView.Columns)
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

        private void FormatStudentPerformanceColumns()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                switch (column.Name)
                {
                    case "Last_Active_Date":
                        column.DefaultCellStyle.Format = "MM/dd/yyyy";
                        break;
                    case "Avg_Grade":
                        column.DefaultCellStyle.Format = "N2";
                        break;
                    case "Feedback_Responsiveness":
                        column.DefaultCellStyle.Format = "P2";
                        break;
                }
            }
        }

        private void FormatAuditLogColumns()
        {
            foreach (DataGridViewColumn column in auditLogGridView.Columns)
            {
                switch (column.Name)
                {
                    case "timestamp":
                        column.DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
                        break;
                }
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
                    saveFileDialog.Title = "Save Admin Dashboard";
                    saveFileDialog.FileName = $"AdminDashboard_{DateTime.Now:yyyyMMdd}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the template file path
                        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "AdminDashboard_Template.xlsx");
                        
                        if (!File.Exists(templatePath))
                        {
                            MessageBox.Show($"Template file not found at: {templatePath}\nPlease ensure AdminDashboard_Template.xlsx exists in the Templates folder.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create a copy of the template
                        File.Copy(templatePath, saveFileDialog.FileName, true);

                        // Open the copied file
                        using (XLWorkbook workbook = new XLWorkbook(saveFileDialog.FileName))
                        {
                            // System Overview
                            var systemSheet = workbook.Worksheet("System Overview");
                            var systemDt = (DataTable)systemOverviewGridView.DataSource;
                            InsertDataIntoWorksheet(systemSheet, systemDt, new[] { "Avg_Feedback_Rating" }, new[] { "Ungraded_Submissions_Pct", "System_Storage_Usage_Estimate" });

                            // Course Analytics
                            var courseSheet = workbook.Worksheet("Course Analytics");
                            var courseDt = (DataTable)studentPerformanceGridView.DataSource;
                            InsertDataIntoWorksheet(courseSheet, courseDt, new[] { "Avg_Feedback_Rating" }, new[] { "Submission_Rate_Pct" });

                            // Student Performance
                            var studentSheet = workbook.Worksheet("Student Performance");
                            var studentDt = (DataTable)dataGridView1.DataSource;
                            InsertDataIntoWorksheet(studentSheet, studentDt, new[] { "Avg_Grade" }, new[] { "Feedback_Responsiveness" });

                            // Audit Log
                            var auditSheet = workbook.Worksheet("Audit Log");
                            var auditDt = (DataTable)auditLogGridView.DataSource;
                            InsertDataIntoWorksheet(auditSheet, auditDt, null, null);

                            // Update summary sheet
                            var summarySheet = workbook.Worksheet("Summary");
                            UpdateSummarySheet(summarySheet, systemDt, courseDt, studentDt, auditDt);

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

        private void InsertDataIntoWorksheet(IXLWorksheet worksheet, DataTable dt, string[] decimalColumns, string[] percentageColumns)
        {
            // Insert data starting at row 8 (after headers)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = worksheet.Cell(i + 8, j + 1);
                    cell.Value = dt.Rows[i][j].ToString();

                    // Apply specific formatting based on column type
                    if (dt.Columns[j].ColumnName.Contains("Date") || dt.Columns[j].ColumnName.Contains("timestamp"))
                    {
                        cell.Style.NumberFormat.Format = "mm/dd/yyyy hh:mm:ss";
                    }
                    else if (decimalColumns != null && decimalColumns.Contains(dt.Columns[j].ColumnName))
                    {
                        cell.Style.NumberFormat.Format = "0.00";
                    }
                    else if (percentageColumns != null && percentageColumns.Contains(dt.Columns[j].ColumnName))
                    {
                        cell.Style.NumberFormat.Format = "0.00%";
                    }
                }
            }
        }

        private void UpdateSummarySheet(IXLWorksheet summarySheet, DataTable systemDt, DataTable courseDt, DataTable studentDt, DataTable auditDt)
        {
            // System Overview metrics
            summarySheet.Cell("A3").Value = "Total Active Users";
            summarySheet.Cell("B3").Value = Convert.ToInt32(systemDt.Rows[0]["Total_Active_Users"]);
            
            summarySheet.Cell("A4").Value = "New Enrollments This Week";
            summarySheet.Cell("B4").Value = Convert.ToInt32(systemDt.Rows[0]["New_Enrollments_This_Week"]);
            
            summarySheet.Cell("A5").Value = "Average Feedback Rating";
            summarySheet.Cell("B5").Value = Convert.ToDouble(systemDt.Rows[0]["Avg_Feedback_Rating"]);
            summarySheet.Cell("B5").Style.NumberFormat.Format = "0.00";
            
            summarySheet.Cell("A6").Value = "Ungraded Submissions";
            summarySheet.Cell("B6").Value = Convert.ToDouble(systemDt.Rows[0]["Ungraded_Submissions_Pct"]);
            summarySheet.Cell("B6").Style.NumberFormat.Format = "0.00%";

            // Course Analytics summary
            summarySheet.Cell("D3").Value = "Total Courses";
            summarySheet.Cell("E3").Value = courseDt.Rows.Count;
            
            summarySheet.Cell("D4").Value = "Average Enrollment";
            double totalEnrollment = 0;
            foreach (DataRow row in courseDt.Rows)
            {
                if (row["Enrolled_Students"] != DBNull.Value)
                {
                    totalEnrollment += Convert.ToDouble(row["Enrolled_Students"]);
                }
            }
            summarySheet.Cell("E4").Value = courseDt.Rows.Count > 0 ? totalEnrollment / courseDt.Rows.Count : 0;
            summarySheet.Cell("E4").Style.NumberFormat.Format = "0.00";

            // Student Performance metrics
            summarySheet.Cell("G3").Value = "Total Students";
            summarySheet.Cell("H3").Value = studentDt.Rows.Count;
            
            summarySheet.Cell("G4").Value = "Average Grade";
            double totalGrade = 0;
            int gradeCount = 0;
            foreach (DataRow row in studentDt.Rows)
            {
                if (row["Avg_Grade"] != DBNull.Value)
                {
                    totalGrade += Convert.ToDouble(row["Avg_Grade"]);
                    gradeCount++;
                }
            }
            summarySheet.Cell("H4").Value = gradeCount > 0 ? totalGrade / gradeCount : 0;
            summarySheet.Cell("H4").Style.NumberFormat.Format = "0.00";

            // Audit Log statistics
            summarySheet.Cell("J3").Value = "Total Log Entries";
            summarySheet.Cell("K3").Value = auditDt.Rows.Count;
            
            summarySheet.Cell("J4").Value = "Most Recent Entry";
            if (auditDt.Rows.Count > 0)
            {
                summarySheet.Cell("K4").Value = Convert.ToDateTime(auditDt.Rows[0]["timestamp"]);
                summarySheet.Cell("K4").Style.NumberFormat.Format = "mm/dd/yyyy hh:mm:ss";
            }
        }
    }
}
