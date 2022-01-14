using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Gears
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool mutexCreated = true;
			using (Mutex mutex = new Mutex(true, "Gears", out mutexCreated))
			{
				if (mutexCreated)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new MainForm());
				}
				else
				{
					Process current = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName(current.ProcessName))
					{
						if (process.Id != current.Id)
						{
							MessageBox.Show("Another instance of Gears is already running.", "Gears", MessageBoxButtons.OK, MessageBoxIcon.Information);
							break;
						}
					}
				}
			}
		}

	}
}
