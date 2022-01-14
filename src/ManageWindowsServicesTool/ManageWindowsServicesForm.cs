using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gears
{
	public partial class ManageWindowsServicesForm : Form
	{
		private FileInfo _file = null;
		private string _cmdArg;

		public ManageWindowsServicesForm()
		{
			InitializeComponent();
		}

		private void ServiceInstallerForm_Load(object sender, EventArgs e)
		{

		}

		private void btn_DragEnter(object sender, DragEventArgs e)
		{
			IDataObject _data = e.Data;
			string _buttonName = (sender as Button).Text;
			_cmdArg = _buttonName.ToLower().Contains("uninstall") ? @" /u " : " ";

			if (_data.GetDataPresent(DataFormats.FileDrop))
			{
				_file = new FileInfo(((string[])_data.GetData("FileName"))[0]);

				if (_file.Extension.ToLower() == ".exe")
				{
					e.Effect = DragDropEffects.All;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
		}

		private void btn_DragDrop(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			RunCommandAsync();
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				_file = new FileInfo(openFileDialog.FileName);
				_cmdArg = " ";
				RunCommandAsync();
			}
		}

		private void btnUninstall_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				_file = new FileInfo(openFileDialog.FileName);
				_cmdArg = @" /u ";
				RunCommandAsync();
			}
		}

		private async void RunCommandAsync()
		{
			string fileName = _file.FullName;
			string cmdFormat = "\"{0}\"{1}\"{2}\"";
			string installUtilPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe";

			string cmdText = string.Format(cmdFormat, installUtilPath, _cmdArg, fileName);

			Process process = new Process();
			process.StartInfo.FileName = cmdText;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();

			StreamReader rdr = process.StandardOutput;
			textBox1.Text = "Please wait...";
			textBox1.Text = await rdr.ReadToEndAsync();
		}
	}
}
