using System;
using System.Windows.Forms;

namespace OnlearnEducation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create templates if they don't exist
            try
            {
                TemplateCreator.CreateTemplates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating templates: {ex.Message}", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Application.Run(new OnLearnLoginForm());
        }
    }
}