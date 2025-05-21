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
using System.Drawing.Drawing2D;

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

            // Set GrowStyle
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            // Load data when form loads
            this.Load += (s, e) =>
        {
            LoadEnrollmentData();
                LoadUserType();
            };
        }

        private void LoadEnrollmentData()
        {
            try
            {
            tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            tableLayoutPanel1.ColumnCount = 3;
                tableLayoutPanel1.RowCount = 1;

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));

            AddHeaderLabel("Course Name", 0, 0);
            AddHeaderLabel("Instructor", 1, 0);
            AddHeaderLabel("Enrollment Date", 2, 0);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT CourseName, InstructorName, EnrollmentDate FROM userenrollmentswithinstructor WHERE UserID = @UserId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int rowIndex = 1;
                            if (!reader.HasRows)
                            {
                                tableLayoutPanel1.RowCount++;
                                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                                AddDataLabel("No courses enrolled yet", 0, rowIndex);
                                AddDataLabel("N/A", 1, rowIndex);
                                AddDataLabel("N/A", 2, rowIndex);

                                // Style the message row
                                var messageLabel = tableLayoutPanel1.GetControlFromPosition(0, rowIndex) as Label;
                                if (messageLabel != null)
                                {
                                    messageLabel.ForeColor = Color.Gray;
                                    messageLabel.Font = new Font(messageLabel.Font, FontStyle.Italic);
                                    messageLabel.TextAlign = ContentAlignment.MiddleCenter;
                                }
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    tableLayoutPanel1.RowCount++;
                                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollment data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT UserType FROM users WHERE UserID = @UserId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);
                        connection.Open();
                        string userType = command.ExecuteScalar()?.ToString() ?? "Unknown";

                        // Set the text and style based on user type
                        UserType.Text = userType;
                        switch (userType.ToLower())
                        {
                            case "student":
                                UserType.ForeColor = Color.Green;
                                btnAdminDashboard.Visible = false;
                                btnStudentCourseLogin.Visible = true;
                                btnInstructor.Visible = false;
                                break;
                            case "instructor":
                                UserType.ForeColor = Color.Blue;
                                btnAdminDashboard.Visible = false;
                                btnStudentCourseLogin.Visible = false;
                                btnInstructor.Visible = true;
                                break;
                            case "admin":
                                UserType.ForeColor = Color.Red;
                                btnAdminDashboard.Visible = true;
                                btnStudentCourseLogin.Visible = false;
                                btnInstructor.Visible = false;
                                break;
                            default:
                                UserType.Text = "Unknown User Type";
                                UserType.ForeColor = Color.Gray;
                                btnAdminDashboard.Visible = false;
                                btnStudentCourseLogin.Visible = false;
                                btnInstructor.Visible = false;
                                break;
                        }

                        // Create rounded border
                        UserType.Paint += (s, e) =>
                        {
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                int radius = 10;
                                Rectangle rect = new Rectangle(0, 0, UserType.Width - 1, UserType.Height - 1);
                                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                                path.CloseAllFigures();

                                UserType.Region = new Region(path);
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user type: {ex.Message}", "Error",
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
            using (var form = new Form())
            {
                form.Text = "Change Password";
                form.Size = new Size(500, 350);  // Increased width and height
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                // Adjust label and textbox positions with more spacing
                var currentPasswordLabel = new Label { Text = "Current Password:", Location = new Point(20, 20), AutoSize = true };
                var currentPasswordBox = new TextBox { Location = new Point(20, 45), Size = new Size(440, 25), PasswordChar = '•' };

                var newPasswordLabel = new Label { Text = "New Password:", Location = new Point(20, 90), AutoSize = true };
                var newPasswordBox = new TextBox { Location = new Point(20, 115), Size = new Size(440, 25), PasswordChar = '•' };

                var confirmPasswordLabel = new Label { Text = "Confirm New Password:", Location = new Point(20, 160), AutoSize = true };
                var confirmPasswordBox = new TextBox { Location = new Point(20, 185), Size = new Size(440, 25), PasswordChar = '•' };

                var updateButton = new Button
                {
                    Text = "Update Password",
                    Location = new Point(20, 250),  // Moved further down
                    Size = new Size(440, 40),       // Wider and taller button
                    DialogResult = DialogResult.OK
                };

                form.Controls.AddRange(new Control[] {
                    currentPasswordLabel, currentPasswordBox,
                    newPasswordLabel, newPasswordBox,
                    confirmPasswordLabel, confirmPasswordBox,
                    updateButton
                });

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(currentPasswordBox.Text) ||
                        string.IsNullOrWhiteSpace(newPasswordBox.Text) ||
                        string.IsNullOrWhiteSpace(confirmPasswordBox.Text))
                    {
                        MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (newPasswordBox.Text != confirmPasswordBox.Text)
                    {
                        MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Verify current password
                            string verifyQuery = "SELECT Password FROM users WHERE UserID = @UserId";
                            using (MySqlCommand verifyCmd = new MySqlCommand(verifyQuery, connection))
                            {
                                verifyCmd.Parameters.AddWithValue("@UserId", _userId);
                                string? currentStoredPassword = verifyCmd.ExecuteScalar()?.ToString();

                                if (currentStoredPassword != currentPasswordBox.Text)
                                {
                                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            // Update password
                            string updateQuery = "UPDATE users SET Password = @NewPassword WHERE UserID = @UserId";
                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@NewPassword", newPasswordBox.Text);
                                updateCmd.Parameters.AddWithValue("@UserId", _userId);
                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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

                            // Add search and filter instructions
                            worksheet.Cell(2, 1).Value = "Search and Filter Instructions:";
                            worksheet.Cell(2, 1).Style.Font.Bold = true;
                            worksheet.Cell(3, 1).Value = "1. Use the filter arrows in column headers to filter data";
                            worksheet.Cell(4, 1).Value = "2. Use Ctrl+F to search within the worksheet";
                            worksheet.Cell(5, 1).Value = "3. Sort by clicking column headers";

                            // Add headers
                            worksheet.Cell(7, 1).Value = "Course Name";
                            worksheet.Cell(7, 2).Value = "Instructor";
                            worksheet.Cell(7, 3).Value = "Enrollment Date";
                            worksheet.Range(7, 1, 7, 3).Style.Font.Bold = true;

                            // Add data
                            worksheet.Cell(8, 1).InsertTable(dt);

                            // Calculate last row and column
                            int lastRow = dt.Rows.Count + 7;
                            int lastCol = dt.Columns.Count;

                            // Add a summary section
                            worksheet.Cell(lastRow + 2, 1).Value = "Enrollment Summary";
                            worksheet.Cell(lastRow + 2, 1).Style.Font.Bold = true;
                            worksheet.Cell(lastRow + 2, 1).Style.Font.FontSize = 12;

                            // Add row count
                            worksheet.Cell(lastRow + 3, 1).Value = "Total Courses:";
                            worksheet.Cell(lastRow + 3, 2).Value = dt.Rows.Count;
                            worksheet.Cell(lastRow + 3, 2).Style.Font.Bold = true;

                            // Add instructor count
                            var uniqueInstructors = dt.AsEnumerable()
                                .Select(r => r.Field<string>("Instructor"))
                                .Distinct()
                                .Count();
                            worksheet.Cell(lastRow + 4, 1).Value = "Unique Instructors:";
                            worksheet.Cell(lastRow + 4, 2).Value = uniqueInstructors;

                            // Add a chart for enrollment timeline
                            if (dt.Rows.Count > 0)
                            {
                                var chart = worksheet.Workbook.Worksheets.Add("Enrollment Timeline");
                                chart.Cell(1, 1).Value = "Course Enrollment Timeline";
                                chart.Cell(1, 1).Style.Font.Bold = true;
                                chart.Cell(1, 1).Style.Font.FontSize = 14;

                                // Create a summary table instead of pivot table
                                int summaryRow = 3;
                                chart.Cell(summaryRow, 1).Value = "Enrollment Timeline Summary";
                                chart.Cell(summaryRow, 1).Style.Font.Bold = true;

                                // Group enrollments by date
                                var enrollmentsByDate = dt.AsEnumerable()
                                    .GroupBy(r => r.Field<string>("Enrollment Date"))
                                    .OrderBy(g => g.Key);

                                int currentRow = summaryRow + 1;
                                foreach (var group in enrollmentsByDate)
                                {
                                    chart.Cell(currentRow, 1).Value = group.Key;
                                    chart.Cell(currentRow, 2).Value = group.Count();
                                    currentRow++;
                                }

                                // Add total
                                chart.Cell(currentRow, 1).Value = "Total Enrollments:";
                                chart.Cell(currentRow, 2).Value = dt.Rows.Count;
                                chart.Cell(currentRow, 1).Style.Font.Bold = true;
                                chart.Cell(currentRow, 2).Style.Font.Bold = true;

                                // Auto-fit columns
                                chart.Columns().AdjustToContents();
                            }

                            // Enable filtering
                            var dataRange = worksheet.Range(7, 1, lastRow, lastCol);
                            dataRange.SetAutoFilter();

                            // Add conditional formatting for enrollment dates
                            var dateColumn = dt.Columns["Enrollment Date"];
                            var colIndex = dt.Columns.IndexOf(dateColumn) + 1;
                            
                            // Apply conditional formatting to the date column
                            for (int row = 8; row <= lastRow; row++)
                            {
                                var cell = worksheet.Cell(row, colIndex);
                                if (DateTime.TryParse(cell.Value.ToString(), out DateTime dateValue))
                                {
                                    if (dateValue > DateTime.Now.AddDays(-7))
                                    {
                                        cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                                    }
                                    else if (dateValue < DateTime.Now.AddDays(-30))
                                    {
                                        cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                                    }
                                }
                            }

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

        private void btnAdminDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                Adminboard adminBoard = new Adminboard(_userId, Username.Text, UserEmail.Text);
                adminBoard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening admin dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStudentCourseLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Studentboard studentBoard = new Studentboard(_userId, Username.Text, UserEmail.Text);
                studentBoard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening student dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInstructor_Click(object sender, EventArgs e)
        {
            try
            {
                Instructorboard instructorBoard = new Instructorboard(_userId, Username.Text, UserEmail.Text);
                instructorBoard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening instructor dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserType_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
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
    }
}
