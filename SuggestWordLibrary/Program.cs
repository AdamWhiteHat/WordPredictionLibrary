using System;
using System.IO;
using System.Windows.Forms;

namespace SuggestWordLibrary
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Application.Run(new MainForm());
		}

		private static readonly string ExceptionLogFilename = "Exceptions.log.txt";

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			File.AppendAllLines(ExceptionLogFilename, new string[] { $"Unhandled exception caught @ {DateTime.Now.ToShortDateString()}:", e.ExceptionObject.ToString(), "--- End of exception ---", Environment.NewLine, Environment.NewLine });
		}
	}
}
