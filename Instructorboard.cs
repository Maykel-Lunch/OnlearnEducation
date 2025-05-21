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
                        // Create a new DataTable
                        DataTable dt = (DataTable)instructorGridView.DataSource;

                        // Create Excel file
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Instructor Dashboard");

                            // Add title
                            worksheet.Cell(1, 1).Value = $"Instructor Dashboard for {_username}";
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                            worksheet.Range(1, 1, 1, dt.Columns.Count).Merge();

                            // Add data table starting at column A
                            worksheet.Cell(3, 1).InsertTable(dt);

                            // Add visualization starting at column M (13th column)
                            int vizStartCol = 13; // Column M
                            int lastRow = dt.Rows.Count + 3;

                            // Add summary section
                            worksheet.Cell(3, vizStartCol).Value = "Course Analytics Summary";
                            worksheet.Cell(3, vizStartCol).Style.Font.Bold = true;
                            worksheet.Cell(3, vizStartCol).Style.Font.FontSize = 12;

                            // Add row count
                            worksheet.Cell(4, vizStartCol).Value = "Total Courses:";
                            worksheet.Cell(4, vizStartCol + 1).Value = dt.Rows.Count;
                            worksheet.Cell(4, vizStartCol + 1).Style.Font.Bold = true;

                            // Add statistics for numeric columns
                            if (dt.Columns.Contains("Enrolled_Students"))
                            {
                                double totalStudents = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["Enrolled_Students"] != DBNull.Value)
                                    {
                                        totalStudents += Convert.ToDouble(row["Enrolled_Students"]);
                                    }
                                }
                                worksheet.Cell(5, vizStartCol).Value = "Total Enrolled Students:";
                                worksheet.Cell(5, vizStartCol + 1).Value = totalStudents;
                            }

                            if (dt.Columns.Contains("Avg_Feedback_Rating"))
                            {
                                double sum = 0;
                                int count = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["Avg_Feedback_Rating"] != DBNull.Value)
                                    {
                                        sum += Convert.ToDouble(row["Avg_Feedback_Rating"]);
                                        count++;
                                    }
                                }
                                if (count > 0)
                                {
                                    worksheet.Cell(6, vizStartCol).Value = "Average Feedback Rating:";
                                    worksheet.Cell(6, vizStartCol + 1).Value = sum / count;
                                    worksheet.Cell(6, vizStartCol + 1).Style.NumberFormat.Format = "0.00";
                                }
                            }

                            // Add a chart for enrollment distribution
                            if (dt.Columns.Contains("Enrolled_Students"))
                            {
                                var chart = worksheet.Workbook.Worksheets.Add($"{worksheet.Name} Charts");
                                chart.Cell(1, 1).Value = "Course Analytics";
                                chart.Cell(1, 1).Style.Font.Bold = true;
                                chart.Cell(1, 1).Style.Font.FontSize = 14;

                                // Prepare data for charts
                                int dataRow = 3;
                                chart.Cell(dataRow, 1).Value = "Course";
                                chart.Cell(dataRow, 2).Value = "Enrolled Students";
                                chart.Cell(dataRow, 3).Value = "Feedback Rating";
                                chart.Cell(dataRow, 4).Value = "Enrollment %";
                                chart.Range(dataRow, 1, dataRow, 4).Style.Font.Bold = true;

                                // Add data for charts
                                double totalStudents = 0;
                                double totalRating = 0;
                                int ratingCount = 0;

                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["Enrolled_Students"] != DBNull.Value)
                                    {
                                        dataRow++;
                                        double students = Convert.ToDouble(row["Enrolled_Students"]);
                                        totalStudents += students;
                                        
                                        chart.Cell(dataRow, 1).Value = row["CourseName"].ToString();
                                        chart.Cell(dataRow, 2).Value = students;
                                        
                                        if (row["Avg_Feedback_Rating"] != DBNull.Value)
                                        {
                                            double rating = Convert.ToDouble(row["Avg_Feedback_Rating"]);
                                            chart.Cell(dataRow, 3).Value = rating;
                                            totalRating += rating;
                                            ratingCount++;
                                        }
                                    }
                                }

                                // Calculate and add percentages
                                dataRow = 3;
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["Enrolled_Students"] != DBNull.Value)
                                    {
                                        dataRow++;
                                        double students = Convert.ToDouble(row["Enrolled_Students"]);
                                        double percentage = totalStudents > 0 ? (students / totalStudents) * 100 : 0;
                                        chart.Cell(dataRow, 4).Value = percentage;
                                        chart.Cell(dataRow, 4).Style.NumberFormat.Format = "0.00%";
                                    }
                                }

                                // Add summary statistics
                                dataRow += 2;
                                chart.Cell(dataRow, 1).Value = "Summary Statistics";
                                chart.Cell(dataRow, 1).Style.Font.Bold = true;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "Total Students:";
                                chart.Cell(dataRow, 2).Value = totalStudents;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "Average Students per Course:";
                                chart.Cell(dataRow, 2).Value = totalStudents / dt.Rows.Count;
                                dataRow++;
                                if (ratingCount > 0)
                                {
                                    chart.Cell(dataRow, 1).Value = "Average Feedback Rating:";
                                    chart.Cell(dataRow, 2).Value = totalRating / ratingCount;
                                    chart.Cell(dataRow, 2).Style.NumberFormat.Format = "0.00";
                                }

                                // Add instructions for creating charts
                                dataRow += 2;
                                chart.Cell(dataRow, 1).Value = "To create charts in Excel:";
                                chart.Cell(dataRow, 1).Style.Font.Bold = true;
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "1. Select the data range (A3:D" + (dataRow - 2) + ")";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "2. Go to Insert > Charts";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "3. Choose Bar Chart for Enrollment distribution";
                                dataRow++;
                                chart.Cell(dataRow, 1).Value = "4. Choose Pie Chart for Course distribution";

                                // Auto-fit columns
                                chart.Columns().AdjustToContents();
                            }

                            // Format the data table
                            var dataRange = worksheet.Range(3, 1, lastRow, dt.Columns.Count);
                            dataRange.Style.NumberFormat.Format = "@"; // Text format by default

                            // Format specific columns
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

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();

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
