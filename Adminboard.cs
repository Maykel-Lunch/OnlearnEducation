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
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            // System Overview Tab
                            var systemSheet = workbook.Worksheets.Add("System Overview");
                            ExportDataTableToWorksheet(systemSheet, (DataTable)systemOverviewGridView.DataSource);
                            FormatSystemOverviewWorksheet(systemSheet);

                            // Course Analytics Tab
                            var courseSheet = workbook.Worksheets.Add("Course Analytics");
                            ExportDataTableToWorksheet(courseSheet, (DataTable)studentPerformanceGridView.DataSource);
                            FormatCourseAnalyticsWorksheet(courseSheet);

                            // Student Performance Tab
                            var studentSheet = workbook.Worksheets.Add("Student Performance");
                            ExportDataTableToWorksheet(studentSheet, (DataTable)dataGridView1.DataSource);
                            FormatStudentPerformanceWorksheet(studentSheet);

                            // Audit Log Tab
                            var auditSheet = workbook.Worksheets.Add("Audit Log");
                            ExportDataTableToWorksheet(auditSheet, (DataTable)auditLogGridView.DataSource);
                            FormatAuditLogWorksheet(auditSheet);

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

        private void ExportDataTableToWorksheet(IXLWorksheet worksheet, DataTable dt)
        {
            // Add title
            worksheet.Cell(1, 1).Value = $"Admin Dashboard for {_username}";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
            worksheet.Range(1, 1, 1, dt.Columns.Count).Merge();

            // Add data
            worksheet.Cell(3, 1).InsertTable(dt);

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();
        }

        private void FormatSystemOverviewWorksheet(IXLWorksheet worksheet)
        {
            var dataRange = worksheet.Range(3, 1, worksheet.LastRowUsed().RowNumber(), worksheet.LastColumnUsed().ColumnNumber());
            dataRange.Style.NumberFormat.Format = "@"; // Text format by default

            // Get the DataTable to find column indices
            var dt = (DataTable)systemOverviewGridView.DataSource;

            // Format specific columns using indices
            var ratingColumn = dt.Columns["Avg_Feedback_Rating"];
            if (ratingColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(ratingColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00";
            }

            var submissionsColumn = dt.Columns["Ungraded_Submissions_Pct"];
            if (submissionsColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(submissionsColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00%";
            }

            var storageColumn = dt.Columns["System_Storage_Usage_Estimate"];
            if (storageColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(storageColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00%";
            }
        }

        private void FormatCourseAnalyticsWorksheet(IXLWorksheet worksheet)
        {
            var dataRange = worksheet.Range(3, 1, worksheet.LastRowUsed().RowNumber(), worksheet.LastColumnUsed().ColumnNumber());
            dataRange.Style.NumberFormat.Format = "@"; // Text format by default

            // Get the DataTable to find column indices
            var dt = (DataTable)studentPerformanceGridView.DataSource;

            // Format specific columns using indices
            var dateColumn = dt.Columns["Next_Assignment_Due"];
            if (dateColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(dateColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "mm/dd/yyyy";
            }

            var ratingColumn = dt.Columns["Avg_Feedback_Rating"];
            if (ratingColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(ratingColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00";
            }
        }

        private void FormatStudentPerformanceWorksheet(IXLWorksheet worksheet)
        {
            var dataRange = worksheet.Range(3, 1, worksheet.LastRowUsed().RowNumber(), worksheet.LastColumnUsed().ColumnNumber());
            dataRange.Style.NumberFormat.Format = "@"; // Text format by default

            // Get the DataTable to find column indices
            var dt = (DataTable)dataGridView1.DataSource;

            // Format specific columns using indices
            var dateColumn = dt.Columns["Last_Active_Date"];
            if (dateColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(dateColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "mm/dd/yyyy";
            }

            var gradeColumn = dt.Columns["Avg_Grade"];
            if (gradeColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(gradeColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00";
            }

            var responsivenessColumn = dt.Columns["Feedback_Responsiveness"];
            if (responsivenessColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(responsivenessColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "0.00";
            }
        }

        private void FormatAuditLogWorksheet(IXLWorksheet worksheet)
        {
            var dataRange = worksheet.Range(3, 1, worksheet.LastRowUsed().RowNumber(), worksheet.LastColumnUsed().ColumnNumber());
            dataRange.Style.NumberFormat.Format = "@"; // Text format by default

            // Get the DataTable to find column indices
            var dt = (DataTable)auditLogGridView.DataSource;

            // Format timestamp column
            var timestampColumn = dt.Columns["timestamp"];
            if (timestampColumn != null)
            {
                var colIndex = dt.Columns.IndexOf(timestampColumn) + 1;
                worksheet.Column(colIndex).Style.NumberFormat.Format = "mm/dd/yyyy hh:mm:ss";
            }
        }
    }
}
