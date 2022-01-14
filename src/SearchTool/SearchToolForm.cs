using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Gears
{
	public partial class SearchToolForm : Form
	{
		List<string> fileList;

		public SearchToolForm()
		{
			InitializeComponent();
			ControlExtensions.DoubleBuffered(treeView1, true);

			fileList = new List<string>();
		}

		public string SearchPaths
		{
			get
			{
				return Properties.Settings.Default.ST_SearchPaths;
			}
			set
			{
				Properties.Settings.Default.ST_SearchPaths = UniqueList(this.SearchPaths, value);
				Properties.Settings.Default.Save();
				UpdateComboBoxes();
			}
		}

		public string SearchTerms
		{
			get
			{
				return Properties.Settings.Default.ST_SearchTerms;
			}
			set
			{
				Properties.Settings.Default.ST_SearchTerms = UniqueList(this.SearchTerms, value);
				Properties.Settings.Default.Save();
				UpdateComboBoxes();
			}
		}

		private string UniqueList(string oldList, string newValue)
		{
			if (newValue == "")
			{
				return "";
			}

			string[] items = oldList.Split(';');
			IEnumerable<string> exist = items.Where(x => x.ToLower() == newValue.ToLower().Trim());
			if (exist.Count() > 0)
			{
				return newValue.Trim() + ";" + oldList.Remove(oldList.IndexOf(newValue), newValue.Length);
			}
			else
			{
				return newValue.Trim() + ";" + oldList;
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(comboSearchFor.Text.Trim()) || String.IsNullOrEmpty(comboPath.Text.Trim()) || !Directory.Exists(comboPath.Text.Trim()))
			{
				MessageBox.Show("Invalid arguments supplied!", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			SearchStart();

		}

		private void SearchStart()
		{
			btnSearch.Enabled = !btnSearch.Enabled;
			btnStop.Enabled = !btnStop.Enabled;

			this.AcceptButton = btnStop;

			fileList.Clear();
			treeView1.Nodes.Clear();

			this.SearchPaths = comboPath.Text;
			this.SearchTerms = comboSearchFor.Text;

			backgroundWorker1.RunWorkerAsync(new SearchArgs() { SearchPath = comboPath.Text, IncludeSubDir = chkSubDirectories.Checked });
		}

		private void SearchEnd()
		{
			btnSearch.Enabled = !btnSearch.Enabled;
			btnStop.Enabled = !btnStop.Enabled;
			this.AcceptButton = btnSearch;

			//lblCurrentFile.Text = "Total file(s) processed " + fileList.Count.ToString() + " File(s) found " + treeView1.Nodes.Count;
			lblCurrentFile.Text = String.Format("Total {0} file(s) processed and found matche(s) in {1} file(s).", fileList.Count, treeView1.Nodes.Count);
		}

		private void PopulateFileList(string path, bool alldir)
		{
			try
			{
				if (true)
				{
					string searchText = "";
					if (comboSearchFor.InvokeRequired)
					{
						comboSearchFor.Invoke((MethodInvoker)delegate {
							searchText = comboSearchFor.Text;
						});
					}
					else
					{
						searchText = comboSearchFor.Text;
					}

					string[] files = Directory.GetFiles(path, txtFileMask.Text);
					string[] dirs = Directory.GetDirectories(path);
					foreach (string f in files)
					{
						fileList.Add(f);
						if (lblCurrentFile.InvokeRequired)
						{
							lblCurrentFile.Invoke((MethodInvoker)delegate
							{
								lblCurrentFile.Text = "Searching In: " + f;
							});
						}

						TreeNode fileRootNode = new TreeNode(f);
						fileRootNode.ExpandAll();
						bool matchFound = false;

						FileInfo fi = new FileInfo(f);

						foreach (var match in File.ReadLines(f).Select((text, index) => new { text, lineNumber = index + 1 }).Where(x => Compare(x.text, searchText)))
						{
							matchFound = true;
							fileRootNode.Nodes.Add("Line No: " + match.lineNumber + ", " + match.text);
						}

						if (matchFound)
						{
							treeView1.Invoke((MethodInvoker)delegate {
								treeView1.Nodes.Add(fileRootNode);
							});
						}
					}
					if (alldir)
					{
						foreach (string d in dirs)
						{
							// before starting new search check if operation is requested to be cancelled
							if (!backgroundWorker1.CancellationPending)
							{
								PopulateFileList(d, true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			backgroundWorker1.CancelAsync();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			PopulateFileList((e.Argument as SearchArgs).SearchPath, (e.Argument as SearchArgs).IncludeSubDir);
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			SearchEnd();
		}

		private bool Compare(string x, string y)
		{
			if (chkCaseInsensitive.Checked)
			{
				return x.ToLower().Contains(y.ToLower());
			}
			else
			{
				return x.Contains(y);
			}
		}

		private void treeView1_DoubleClick(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode.Parent != null)
			{
				//MessageBox.Show(treeView1.SelectedNode.Parent.Text);
				System.Diagnostics.Process.Start(treeView1.SelectedNode.Parent.Text);
			}
		}

		private void SearchToolUI_Load(object sender, EventArgs e)
		{
			lblCurrentFile.Text = "";
			UpdateComboBoxes();

			comboSearchFor.Text = SearchTerms.Split(';')[0];
			comboPath.Text = SearchPaths.Split(';')[0];
		}

		private void UpdateComboBoxes(bool isExplicit = false)
		{
			string[] paths = SearchPaths.Split(';');
			comboPath.Items.Clear();
			foreach (string s in paths)
			{
				if (!string.IsNullOrEmpty(s.Trim()))
				{
					comboPath.Items.Add(s);
				}
			}

			string[] terms = SearchTerms.Split(';');
			comboSearchFor.Items.Clear();
			foreach (string s in terms)
			{
				if (!string.IsNullOrEmpty(s.Trim()))
				{
					comboSearchFor.Items.Add(s); 
				}
			}

			if (isExplicit)
			{
				comboPath.Text = comboSearchFor.Text = "";
			}
		}

		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			treeView1.ExpandAll();
		}

		private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			treeView1.CollapseAll();
		}

		private void clearSearchHistoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.ST_SearchTerms = "";
			Properties.Settings.Default.Save();
			Properties.Settings.Default.ST_SearchPaths = "";
			Properties.Settings.Default.Save();
			UpdateComboBoxes(true);
		}

		private void treeView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.C))
			{
				Clipboard.Clear();
				Clipboard.SetText(treeView1.SelectedNode.Text.Remove(0, treeView1.SelectedNode.Text.IndexOf(',')+1).Trim());
			}
		}
	}

	internal class SearchArgs
	{
		public string SearchPath { get; set; }
		public bool IncludeSubDir { get; set; }
	}

	internal static class ControlExtensions
	{
		public static void DoubleBuffered(this Control control, bool enable)
		{
			var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			doubleBufferPropertyInfo.SetValue(control, enable, null);
		}
	}
}
