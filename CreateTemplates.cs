using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using ClosedXML.Excel;
using System.IO;

namespace OnlearnEducation
{
    public class TemplateCreator
    {
        public static void CreateTemplates()
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            CreateStudentTemplate();
            CreateInstructorTemplate();
            CreateAdminTemplate();
        }

        private static void CreateStudentTemplate()
        {
            using (var package = new ExcelPackage())
            {
                // Data Sheet
                var dataSheet = package.Workbook.Worksheets.Add("Data");
                // Add headers in row 7
                dataSheet.Cells["A7"].Value = "CourseName";
                dataSheet.Cells["B7"].Value = "CourseDescription";
                dataSheet.Cells["C7"].Value = "InstructorName";
                dataSheet.Cells["D7"].Value = "NextAssignment";
                dataSheet.Cells["E7"].Value = "NextAssignmentDueDate";
                dataSheet.Cells["F7"].Value = "DaysRemaining";
                dataSheet.Cells["G7"].Value = "RecentGrade";
                dataSheet.Cells["H7"].Value = "NextQuiz";
                dataSheet.Cells["I7"].Value = "QuizTotalMarks";
                dataSheet.Cells["J7"].Value = "FeedbackRating";

                // Format headers and enable filtering
                var headerRange = dataSheet.Cells["A7:J7"];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                dataSheet.Cells["A7:J7"].AutoFilter = true;

                // Add sample data for charts
                dataSheet.Cells["A8"].Value = "Sample Course 1";
                dataSheet.Cells["G8"].Value = 85;
                dataSheet.Cells["A9"].Value = "Sample Course 2";
                dataSheet.Cells["G9"].Value = 92;
                dataSheet.Cells["A10"].Value = "Sample Course 3";
                dataSheet.Cells["G10"].Value = 78;

                // Charts Sheet
                var chartsSheet = package.Workbook.Worksheets.Add("Charts");
                chartsSheet.Cells["A1"].Value = "Grade Distribution";
                chartsSheet.Cells["A1"].Style.Font.Bold = true;
                chartsSheet.Cells["A1"].Style.Font.Size = 14;

                // Create bar chart for grade distribution
                var barChart = chartsSheet.Drawings.AddChart("GradeDistribution", eChartType.ColumnClustered);
                barChart.SetPosition(1, 0, 2, 0);
                barChart.SetSize(600, 300);
                barChart.Series.Add(dataSheet.Cells["G8:G10"], dataSheet.Cells["A8:A10"]);

                // Create pie chart for pass/fail status
                chartsSheet.Cells["A20"].Value = "Pass/Fail Distribution";
                chartsSheet.Cells["A20"].Style.Font.Bold = true;
                chartsSheet.Cells["A20"].Style.Font.Size = 14;

                var pieChart = chartsSheet.Drawings.AddChart("PassFailDistribution", eChartType.Pie);
                pieChart.SetPosition(20, 0, 2, 0);
                pieChart.SetSize(600, 300);
                pieChart.Series.Add(dataSheet.Cells["G8:G10"], dataSheet.Cells["A8:A10"]);

                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");
                summarySheet.Cells["A2"].Value = "Student Dashboard Summary";
                summarySheet.Cells["A2"].Style.Font.Bold = true;
                summarySheet.Cells["A2"].Style.Font.Size = 14;

                summarySheet.Cells["A3"].Value = "Total Courses";
                summarySheet.Cells["A4"].Value = "Average Grade";
                summarySheet.Cells["A5"].Value = "Pass Rate";

                // Save the template
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "StudentDashboard_Template.xlsx");
                Directory.CreateDirectory(Path.GetDirectoryName(templatePath));
                package.SaveAs(new FileInfo(templatePath));

                // Use ClosedXML to add additional features
                using (var workbook = new XLWorkbook(templatePath))
                {
                    var ws = workbook.Worksheet("Data");
                    
                    // Add data validation for grades (0-100)
                    var gradeRange = ws.Range("G8:G1000");
                    gradeRange.SetDataValidation().WholeNumber.Between(0, 100);
                    
                    // Add conditional formatting for grades
                    gradeRange.AddConditionalFormat().WhenLessThan(60)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Red);
                    gradeRange.AddConditionalFormat().WhenBetween(60, 69)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Yellow);
                    gradeRange.AddConditionalFormat().WhenGreaterThan(69)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Green);

                    // Add data validation for feedback rating (1-5)
                    var feedbackRange = ws.Range("J8:J1000");
                    feedbackRange.SetDataValidation().WholeNumber.Between(1, 5);

                    workbook.Save();
                }
            }
        }

        private static void CreateInstructorTemplate()
        {
            using (var package = new ExcelPackage())
            {
                // Data Sheet
                var dataSheet = package.Workbook.Worksheets.Add("Data");
                // Add headers in row 7
                dataSheet.Cells["A7"].Value = "CourseID";
                dataSheet.Cells["B7"].Value = "CourseName";
                dataSheet.Cells["C7"].Value = "Enrolled_Students";
                dataSheet.Cells["D7"].Value = "Active_Assignments";
                dataSheet.Cells["E7"].Value = "Ungraded_Submissions";
                dataSheet.Cells["F7"].Value = "Avg_Feedback_Rating";
                dataSheet.Cells["G7"].Value = "Next_Assignment_Due";
                dataSheet.Cells["H7"].Value = "Quiz_Count";
                dataSheet.Cells["I7"].Value = "Course_Description";
                dataSheet.Cells["J7"].Value = "Submission_Rate_Pct";

                // Format headers and enable filtering
                var headerRange = dataSheet.Cells["A7:J7"];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                dataSheet.Cells["A7:J7"].AutoFilter = true;

                // Add sample data for charts
                dataSheet.Cells["B8"].Value = "Course 1";
                dataSheet.Cells["C8"].Value = 25;
                dataSheet.Cells["F8"].Value = 4.5;
                dataSheet.Cells["B9"].Value = "Course 2";
                dataSheet.Cells["C9"].Value = 30;
                dataSheet.Cells["F9"].Value = 4.2;
                dataSheet.Cells["B10"].Value = "Course 3";
                dataSheet.Cells["C10"].Value = 20;
                dataSheet.Cells["F10"].Value = 4.8;

                // Charts Sheet
                var chartsSheet = package.Workbook.Worksheets.Add("Charts");
                chartsSheet.Cells["A1"].Value = "Enrollment Distribution";
                chartsSheet.Cells["A1"].Style.Font.Bold = true;
                chartsSheet.Cells["A1"].Style.Font.Size = 14;

                // Create bar chart for enrollment
                var barChart = chartsSheet.Drawings.AddChart("EnrollmentDistribution", eChartType.ColumnClustered);
                barChart.SetPosition(1, 0, 2, 0);
                barChart.SetSize(600, 300);
                barChart.Series.Add(dataSheet.Cells["C8:C10"], dataSheet.Cells["B8:B10"]);

                // Create pie chart for feedback ratings
                chartsSheet.Cells["A20"].Value = "Feedback Ratings";
                chartsSheet.Cells["A20"].Style.Font.Bold = true;
                chartsSheet.Cells["A20"].Style.Font.Size = 14;

                var pieChart = chartsSheet.Drawings.AddChart("FeedbackRatings", eChartType.Pie);
                pieChart.SetPosition(20, 0, 2, 0);
                pieChart.SetSize(600, 300);
                pieChart.Series.Add(dataSheet.Cells["F8:F10"], dataSheet.Cells["B8:B10"]);

                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");
                summarySheet.Cells["A2"].Value = "Instructor Dashboard Summary";
                summarySheet.Cells["A2"].Style.Font.Bold = true;
                summarySheet.Cells["A2"].Style.Font.Size = 14;

                summarySheet.Cells["A3"].Value = "Total Courses";
                summarySheet.Cells["A4"].Value = "Total Enrolled Students";
                summarySheet.Cells["A5"].Value = "Average Feedback Rating";
                summarySheet.Cells["A6"].Value = "Average Submission Rate";

                // Save the template
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "InstructorDashboard_Template.xlsx");
                Directory.CreateDirectory(Path.GetDirectoryName(templatePath));
                package.SaveAs(new FileInfo(templatePath));

                // Use ClosedXML to add additional features
                using (var workbook = new XLWorkbook(templatePath))
                {
                    var ws = workbook.Worksheet("Data");
                    
                    // Add data validation for feedback rating (1-5)
                    var feedbackRange = ws.Range("F8:F1000");
                    feedbackRange.SetDataValidation().Decimal.Between(1, 5);
                    
                    // Add conditional formatting for submission rate
                    var submissionRange = ws.Range("J8:J1000");
                    submissionRange.AddConditionalFormat().WhenLessThan(50)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Red);
                    submissionRange.AddConditionalFormat().WhenBetween(50, 74)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Yellow);
                    submissionRange.AddConditionalFormat().WhenGreaterThan(74)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Green);

                    workbook.Save();
                }
            }
        }

        private static void CreateAdminTemplate()
        {
            using (var package = new ExcelPackage())
            {
                // System Overview Sheet
                var systemSheet = package.Workbook.Worksheets.Add("System Overview");
                systemSheet.Cells["A7"].Value = "New_Enrollments_This_Week";
                systemSheet.Cells["B7"].Value = "Total_Active_Users";
                systemSheet.Cells["C7"].Value = "Courses_Without_Assignments";
                systemSheet.Cells["D7"].Value = "Avg_Feedback_Rating";
                systemSheet.Cells["E7"].Value = "Ungraded_Submissions_Pct";
                systemSheet.Cells["F7"].Value = "Recent_Audit_Entry";
                systemSheet.Cells["G7"].Value = "Inactive_Instructors";
                systemSheet.Cells["H7"].Value = "High_Enrollment_Courses";
                systemSheet.Cells["I7"].Value = "Password_Reset_Requests";
                systemSheet.Cells["J7"].Value = "System_Storage_Usage_Estimate";

                // Enable filtering for System Overview
                var systemHeaderRange = systemSheet.Cells["A7:J7"];
                systemHeaderRange.Style.Font.Bold = true;
                systemHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                systemSheet.Cells["A7:J7"].AutoFilter = true;

                // Course Analytics Sheet
                var courseSheet = package.Workbook.Worksheets.Add("Course Analytics");
                courseSheet.Cells["A7"].Value = "CourseID";
                courseSheet.Cells["B7"].Value = "CourseName";
                courseSheet.Cells["C7"].Value = "Enrolled_Students";
                courseSheet.Cells["D7"].Value = "Active_Assignments";
                courseSheet.Cells["E7"].Value = "Ungraded_Submissions";
                courseSheet.Cells["F7"].Value = "Avg_Feedback_Rating";
                courseSheet.Cells["G7"].Value = "Recent_Feedback";
                courseSheet.Cells["H7"].Value = "Next_Assignment_Due";
                courseSheet.Cells["I7"].Value = "Quiz_Count";
                courseSheet.Cells["J7"].Value = "Course_Description";
                courseSheet.Cells["K7"].Value = "Submission_Rate_Pct";

                // Enable filtering for Course Analytics
                var courseHeaderRange = courseSheet.Cells["A7:K7"];
                courseHeaderRange.Style.Font.Bold = true;
                courseHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                courseSheet.Cells["A7:K7"].AutoFilter = true;

                // Student Performance Sheet
                var studentSheet = package.Workbook.Worksheets.Add("Student Performance");
                studentSheet.Cells["A7"].Value = "UserID";
                studentSheet.Cells["B7"].Value = "Student_Name";
                studentSheet.Cells["C7"].Value = "Courses_Taken";
                studentSheet.Cells["D7"].Value = "Avg_Grade";
                studentSheet.Cells["E7"].Value = "Assignments_Missed";
                studentSheet.Cells["F7"].Value = "Feedback_Responsiveness";
                studentSheet.Cells["G7"].Value = "Total_Quiz_Questions";
                studentSheet.Cells["H7"].Value = "Best_Performing_Course";
                studentSheet.Cells["I7"].Value = "Last_Active_Date";
                studentSheet.Cells["J7"].Value = "Alert_Status";

                // Enable filtering for Student Performance
                var studentHeaderRange = studentSheet.Cells["A7:J7"];
                studentHeaderRange.Style.Font.Bold = true;
                studentHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                studentSheet.Cells["A7:J7"].AutoFilter = true;

                // Audit Log Sheet
                var auditSheet = package.Workbook.Worksheets.Add("Audit Log");
                auditSheet.Cells["A7"].Value = "log_id";
                auditSheet.Cells["B7"].Value = "action";
                auditSheet.Cells["C7"].Value = "table_name";
                auditSheet.Cells["D7"].Value = "record_id";
                auditSheet.Cells["E7"].Value = "timestamp";

                // Enable filtering for Audit Log
                var auditHeaderRange = auditSheet.Cells["A7:E7"];
                auditHeaderRange.Style.Font.Bold = true;
                auditHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                auditSheet.Cells["A7:E7"].AutoFilter = true;

                // Charts Sheet
                var chartsSheet = package.Workbook.Worksheets.Add("Charts");
                
                // System Metrics Chart
                chartsSheet.Cells["A1"].Value = "System Metrics";
                chartsSheet.Cells["A1"].Style.Font.Bold = true;
                chartsSheet.Cells["A1"].Style.Font.Size = 14;

                var systemChart = chartsSheet.Drawings.AddChart("SystemMetrics", eChartType.ColumnClustered);
                systemChart.SetPosition(1, 0, 2, 0);
                systemChart.SetSize(600, 300);
                systemChart.Series.Add(systemSheet.Cells["B7:J7"], systemSheet.Cells["A7:I7"]);

                // Course Analytics Chart
                chartsSheet.Cells["A20"].Value = "Course Analytics";
                chartsSheet.Cells["A20"].Style.Font.Bold = true;
                chartsSheet.Cells["A20"].Style.Font.Size = 14;

                var courseChart = chartsSheet.Drawings.AddChart("CourseAnalytics", eChartType.Pie);
                courseChart.SetPosition(20, 0, 2, 0);
                courseChart.SetSize(600, 300);
                courseChart.Series.Add(courseSheet.Cells["C7:K7"], courseSheet.Cells["A7:I7"]);

                // Student Performance Chart
                chartsSheet.Cells["A40"].Value = "Student Performance";
                chartsSheet.Cells["A40"].Style.Font.Bold = true;
                chartsSheet.Cells["A40"].Style.Font.Size = 14;

                var studentChart = chartsSheet.Drawings.AddChart("StudentPerformance", eChartType.ColumnClustered);
                studentChart.SetPosition(40, 0, 2, 0);
                studentChart.SetSize(600, 300);
                studentChart.Series.Add(studentSheet.Cells["C7:J7"], studentSheet.Cells["A7:H7"]);

                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");
                summarySheet.Cells["A2"].Value = "Admin Dashboard Summary";
                summarySheet.Cells["A2"].Style.Font.Bold = true;
                summarySheet.Cells["A2"].Style.Font.Size = 14;

                // System Overview metrics
                summarySheet.Cells["A3"].Value = "Total Active Users";
                summarySheet.Cells["A4"].Value = "New Enrollments This Week";
                summarySheet.Cells["A5"].Value = "Average Feedback Rating";
                summarySheet.Cells["A6"].Value = "Ungraded Submissions";

                // Course Analytics summary
                summarySheet.Cells["D3"].Value = "Total Courses";
                summarySheet.Cells["D4"].Value = "Average Enrollment";

                // Student Performance metrics
                summarySheet.Cells["G3"].Value = "Total Students";
                summarySheet.Cells["G4"].Value = "Average Grade";

                // Audit Log statistics
                summarySheet.Cells["J3"].Value = "Total Log Entries";
                summarySheet.Cells["J4"].Value = "Most Recent Entry";

                // Save the template
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "AdminDashboard_Template.xlsx");
                Directory.CreateDirectory(Path.GetDirectoryName(templatePath));
                package.SaveAs(new FileInfo(templatePath));

                // Use ClosedXML to add additional features
                using (var workbook = new XLWorkbook(templatePath))
                {
                    // Add data validation and conditional formatting to System Overview
                    var systemWs = workbook.Worksheet("System Overview");
                    var feedbackRange = systemWs.Range("D8:D1000");
                    feedbackRange.SetDataValidation().Decimal.Between(1, 5);
                    
                    // Add conditional formatting for ungraded submissions
                    var ungradedRange = systemWs.Range("E8:E1000");
                    ungradedRange.AddConditionalFormat().WhenGreaterThan(50)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Red);
                    ungradedRange.AddConditionalFormat().WhenBetween(26, 50)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Yellow);
                    ungradedRange.AddConditionalFormat().WhenLessThan(26)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Green);

                    // Add data validation and conditional formatting to Course Analytics
                    var courseWs = workbook.Worksheet("Course Analytics");
                    var enrollmentRange = courseWs.Range("C8:C1000");
                    enrollmentRange.SetDataValidation().WholeNumber.GreaterThan(0);
                    
                    // Add conditional formatting for submission rate
                    var submissionRange = courseWs.Range("K8:K1000");
                    submissionRange.AddConditionalFormat().WhenLessThan(50)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Red);
                    submissionRange.AddConditionalFormat().WhenBetween(50, 74)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Yellow);
                    submissionRange.AddConditionalFormat().WhenGreaterThan(74)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Green);

                    // Add data validation and conditional formatting to Student Performance
                    var studentWs = workbook.Worksheet("Student Performance");
                    var gradeRange = studentWs.Range("D8:D1000");
                    gradeRange.SetDataValidation().WholeNumber.Between(0, 100);
                    
                    // Add conditional formatting for grades
                    gradeRange.AddConditionalFormat().WhenLessThan(60)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Red);
                    gradeRange.AddConditionalFormat().WhenBetween(60, 69)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Yellow);
                    gradeRange.AddConditionalFormat().WhenGreaterThan(69)
                        .Fill.SetPatternType(XLFillPatternValues.Solid)
                        .Fill.SetBackgroundColor(XLColor.Green);

                    workbook.Save();
                }
            }
        }
    }
}