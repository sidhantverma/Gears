using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Gears
{
	public partial class MainForm : Form
	{
		Point startPoint;
		Point endPoint;
		Pen pen;
		SolidBrush brush;
		Bitmap bitmap;
		Bitmap lastBitmap;
		BitmapStack stackBitmap;
		bool undo = false, init = true;
		public bool drawText = false;
		int screenWidth, screenHeight;
		SearchToolForm searchToolForm = null;
		ManageWindowsServicesForm serviceInstallerForm = null;

		protected override void SetVisibleCore(bool value)
		{
			base.SetVisibleCore(false);
		}

		public MainForm()
		{
			InitializeComponent();
			ConfigureRunAtStartup();
		}

		void openCntxtMenuHotKey_HotkeyPressed(object sender, EventArgs e)
		{
			openContextMenu();
		}

		public Color DrawingColor
		{
			get
			{
				return Properties.Settings.Default.DrawingColor;
			}
			set
			{
				pen.Color = value;
				brush.Color = value;

				drawingColorToolStripMenuItem.BackColor = value;
				drawingColorToolStripMenuItem.ForeColor = (PerceivedBrightness(value) > 130 ? Color.Black : Color.White);

				Properties.Settings.Default.DrawingColor = value;
				Properties.Settings.Default.Save();
			}
		}

		private int PerceivedBrightness(Color c)
		{
			return (int)Math.Sqrt(
			c.R * c.R * .241 +
			c.G * c.G * .691 +
			c.B * c.B * .068);
		}

		public int DrawingWidth
		{
			get
			{
				return Properties.Settings.Default.DrawingWidth;
			}
			set
			{
				pen.Width = value;
				Properties.Settings.Default.DrawingWidth = value;
				Properties.Settings.Default.Save();
			}
		}

		public string DrawingShape
		{
			get
			{
				return Properties.Settings.Default.DrawingShape;
			}
			set
			{
				Properties.Settings.Default.DrawingShape = value;
				Properties.Settings.Default.Save();
			}
		}

		public Font DrawingFont
		{
			get
			{
				return Properties.Settings.Default.DrawingFont;
			}
			set
			{
				Properties.Settings.Default.DrawingFont = value;
				Properties.Settings.Default.Save();
			}
		}

		public string DrawingText
		{
			get
			{
				return Properties.Settings.Default.DrawingText;
			}
			set
			{
				Properties.Settings.Default.DrawingText = value;
				Properties.Settings.Default.Save();
			}
		}

		private void UpdateWidth()
		{
			UncheckToolStripMenuItems(drawingWidthToolStripMenuItem.DropDownItems);
			if (this.DrawingWidth < 10)
			{
				(drawingWidthToolStripMenuItem.DropDownItems.Find("toolStripMenuItemWidth0" + this.DrawingWidth, false)[0]
					as ToolStripMenuItem).Checked = true;
			}
			else
			{
				(drawingWidthToolStripMenuItem.DropDownItems.Find("toolStripMenuItemWidth" + this.DrawingWidth, false)[0]
					as ToolStripMenuItem).Checked = true;
			}
		}

		private void UncheckToolStripMenuItems(ToolStripItemCollection menuItems)
		{
			try
			{
				foreach (ToolStripMenuItem item in menuItems)
				{
					item.Checked = false;
				}
			}
			catch
			{
				// ignore separator items
			}
		}

		private void UpdateShape()
		{
			UncheckToolStripMenuItems(contextMenuDrawing.Items);

			switch (this.DrawingShape)
			{
				case "Line":
					drawLineToolStripMenuItem.Checked = true;
					pen = new Pen(this.DrawingColor, this.DrawingWidth);
					break;
				case "Arrow":
					drawArrwoToolStripMenuItem.Checked = true;
					pen = new Pen(this.DrawingColor, this.DrawingWidth);
					AdjustableArrowCap bigArrowCap = new AdjustableArrowCap(this.DrawingWidth * 1.2F, this.DrawingWidth * 1.2F);
					pen.CustomEndCap = bigArrowCap;
					break;
				case "DArrow":
					drawDoubleArrowToolStripMenuItem.Checked = true;
					pen = new Pen(this.DrawingColor, this.DrawingWidth);
					AdjustableArrowCap dcap = new AdjustableArrowCap(this.DrawingWidth * 1.2F, this.DrawingWidth * 1.2F);
					pen.CustomStartCap = dcap;
					pen.CustomEndCap = dcap;
					break;
				case "Rectangle":
					drawRectangleToolStripMenuItem.Checked = true;
					break;
				case "Ellipse":
					drawEllipseToolStripMenuItem.Checked = true;
					break;
				default:
					// do nothing
					break;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void startOnScreenDrawingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			screenWidth = Screen.PrimaryScreen.Bounds.Width;
			screenHeight = Screen.PrimaryScreen.Bounds.Height;

			pen = new Pen(this.DrawingColor, this.DrawingWidth);
			brush = new SolidBrush(this.DrawingColor);

			drawingColorToolStripMenuItem.BackColor = DrawingColor;
			drawingColorToolStripMenuItem.ForeColor = (PerceivedBrightness(DrawingColor) > 130 ? Color.Black : Color.White);

			stackBitmap = new BitmapStack();

			UpdateShape();
			UpdateWidth();

			StartOnScreenDrawing();
		}

		private void StartOnScreenDrawing()
		{
			if (bitmap != null)
			{
				bitmap.Dispose();
			}

			if (lastBitmap != null)
			{
				lastBitmap.Dispose();
			}

			bitmap = new Bitmap(screenWidth, screenHeight);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
			}

			lastBitmap = new Bitmap(bitmap);

			init = true;
			this.Invalidate();

			stackBitmap.PushFirst(new Bitmap(bitmap));

			base.SetVisibleCore(true);

			this.Focus();
			this.Activate();

		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				openContextMenu();
			}
		}

		private void openContextMenu()
		{
			MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
			mi.Invoke(notifyIconGears, null);
		}

		private void GearsUI_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				bitmap.Dispose();
				lastBitmap.Dispose();
				base.SetVisibleCore(false);
			}
		}

		private void drawLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "Line");
		}

		private void drawArrwoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "Arrow");
		}

		private void drawDoubleArrowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "DArrow");
		}

		private void drawRectangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "Rectangle");
		}

		private void drawEllipseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "Ellipse");
		}

		private void drawTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckUncheckToolStripMenuItems(sender, "Text");
			DrawingTextInputForm inpForm = new DrawingTextInputForm(this);
			inpForm.ShowDialog();
		}

		private void CheckUncheckToolStripMenuItems(object toolStripMenuItem, string shape)
		{
			this.DrawingShape = shape;
			UpdateShape();
			UpdateWidth();
		}

		private void drawingColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = this.DrawingColor;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				this.DrawingColor = colorDialog1.Color;
			}
		}

		private void toolStripMenuItemWidth_Click(object sender, EventArgs e)
		{
			this.DrawingWidth = int.Parse((sender as ToolStripMenuItem).Text);
			UpdateWidth();
		}

		private void drawingFontToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fontDialog1.Font = this.DrawingFont;
			if (fontDialog1.ShowDialog() == DialogResult.OK)
			{
				this.DrawingFont = fontDialog1.Font;
			}
		}

		private void DrawAdvancedRectangle(Graphics g)
		{
			//label1.Text = String.Format("StartX: {0} StartY: {1}\nEndX: {2} EndY: {3}", startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
			if (endPoint.X > startPoint.X && endPoint.Y > startPoint.Y)
				g.DrawRectangle(pen,
						startPoint.X,
						startPoint.Y,
						endPoint.X - startPoint.X,
						endPoint.Y - startPoint.Y);
			else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
				g.DrawRectangle(pen,
						startPoint.X,
						endPoint.Y,
						endPoint.X - startPoint.X,
						startPoint.Y - endPoint.Y);
			else if (endPoint.X < startPoint.X && endPoint.Y < startPoint.Y)
				g.DrawRectangle(pen,
						endPoint.X,
						endPoint.Y,
						startPoint.X - endPoint.X,
						startPoint.Y - endPoint.Y);
			else if (endPoint.X < startPoint.X && endPoint.Y > startPoint.Y)
				g.DrawRectangle(pen,
						endPoint.X,
						startPoint.Y,
						startPoint.X - endPoint.X,
						endPoint.Y - startPoint.Y);
		}

		private void GearsUI_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if (!undo)
			{
				DoPainting(g);
			}
			if (init)
			{
				g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
			}
		}

		private void GearsUI_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				startPoint = new Point(e.X, e.Y);
			}
		}

		private void GearsUI_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || (DrawingShape == "Text" && drawText))
			{
				endPoint = new Point(e.X, e.Y);

				undo = false;
				init = false;

				this.Invalidate();

				using (Graphics g = Graphics.FromImage(bitmap))
				{
					DoPainting(g);
				}
			}
		}

		private void GearsUI_MouseClick(object sender, MouseEventArgs e)
		{
			if (drawText && DrawingShape == "Text")
			{
				drawText = false;
				DrawingShape = "";
			}
		}

		private void GearsUI_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					lastBitmap.Dispose();
					lastBitmap = new Bitmap(bitmap);

					Graphics fg = this.CreateGraphics();
					fg.DrawImage(lastBitmap, 0, 0);
					fg.Dispose();

					stackBitmap.Push(new Bitmap(lastBitmap));
				}
			}
		}

		private void GearsUI_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.Z))
			{
				undo = true;

				Graphics fg = this.CreateGraphics();

				lastBitmap.Dispose();
				lastBitmap = new Bitmap(stackBitmap.Pop());
				bitmap.Dispose();
				bitmap = new Bitmap(lastBitmap);

				fg.DrawImage(lastBitmap, 0, 0);
				fg.Dispose();
			}
		}

		private void DoPainting(Graphics g)
		{
			g.DrawImage(lastBitmap, 0, 0);
			switch (this.DrawingShape)
			{
				case "Line":
				case "Arrow":
				case "DArrow":
					g.DrawLine(pen, startPoint, endPoint);
					break;
				case "Rectangle":
					DrawAdvancedRectangle(g);
					break;
				case "Text":
					g.DrawString(DrawingText, DrawingFont, brush, endPoint);
					break;
				case "Ellipse":
					g.DrawEllipse(pen, new Rectangle(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
					break;
			}
		}

		private void ShowGearsBaloonTip(string message, ToolTipIcon icon)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			this.notifyIconGears.BalloonTipText = message;
			this.notifyIconGears.BalloonTipIcon = icon;
			this.notifyIconGears.ShowBalloonTip(500);
		}

		private string RegExReplace(string pattern, string input, string replacement)
		{
			Regex re = new Regex(pattern);
			return re.Replace(input, replacement);
		}

		private string RegExMatch(string pattern, string input)
		{
			Regex re = new Regex(pattern);
			return re.Match(input).Value;
		}

		private void SetClipboardText(ClipboardActionContext ctx)
		{
			if (ctx.ActionResult.Result != String.Empty)
			{
				Clipboard.SetText(ctx.ActionResult.Result);
			}

			ShowGearsBaloonTip(
				ctx.ActionResult.StatusMessage,
				ctx.ActionResult.Status == Status.Completed ? ToolTipIcon.Info : ToolTipIcon.Error);
		}

		private void lineBreakToCommaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var ctx = new ClipboardActionContext(new LineBreakToCommaClipAction());
			ctx.Execute(Clipboard.GetText());
			SetClipboardText(ctx);
		}

		private void lineBreakToLineBreakCommaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var ctx = new ClipboardActionContext(new LineBreakToLineBreakCommaClipAction());
			ctx.Execute(Clipboard.GetText());
			SetClipboardText(ctx);
		}

		private void encloseColumnNamesInSqaureBracketsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				string text = Clipboard.GetText();
				string[] lineSep = { Environment.NewLine };
				string[] commaSep = { "," };

				string[] colByLines = text.Split(lineSep, StringSplitOptions.None);
				string[] colByComma = text.Split(commaSep, StringSplitOptions.None);

				string columnsList = "";

				if (colByLines.Length > 1)
				{
					foreach (string s in colByLines)
					{
						columnsList += "[" + RegExReplace(",", s.Trim(), "") + "]," + Environment.NewLine;
					}
				}
				else
				{
					foreach (string s in colByComma)
					{
						columnsList += "[" + s.Trim() + "], ";
					}
				}

				columnsList = columnsList.Remove(columnsList.LastIndexOf(','));
				Clipboard.SetText(columnsList);

				ShowGearsBaloonTip("Success: Enclose column names in square brackets", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void convertDoubleBackSlashToSingleBackSlashToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//(?:\\)(\\)
			if (Clipboard.ContainsText())
			{
				Clipboard.SetText(RegExReplace(@"(?:\\)(\\)", Clipboard.GetText(), @"\"));

				ShowGearsBaloonTip("Success: Converte double backslash to single backslash", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void convertSingleBackSlashToDoubleBackSlashToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// (?<!\\)\\(?!\\)
			if (Clipboard.ContainsText())
			{
				Clipboard.SetText(RegExReplace(@"(?<!\\)\\(?!\\)", Clipboard.GetText(), @"\\"));

				ShowGearsBaloonTip("Success: Converte single backslash to double backslash", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void extractFileNameFromURLOrPathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				try
				{
					string text = Clipboard.GetText().Replace("\"", "");
					Uri urlOrFilePath = new Uri(text);
					string fileName;
					if (urlOrFilePath.IsFile)
					{
						fileName = new FileInfo(text).Name;
						Clipboard.SetText(fileName);
					}
					else
					{
						fileName = RegExMatch(@"(\w+\.\w+)$", urlOrFilePath.AbsolutePath);
						Clipboard.SetText(fileName);
					}

					ShowGearsBaloonTip("Success: Extract file name from URL or Path" + Environment.NewLine + fileName, ToolTipIcon.Info);
				}
				catch
				{
					ShowGearsBaloonTip("Error: Invalid characters in URL or Path", ToolTipIcon.Error);
				}
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void removeEmptyLinesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				Clipboard.SetText(RegExReplace("(\r\n)+", Clipboard.GetText(), Environment.NewLine));
				ShowGearsBaloonTip("Success: Remove empty lines", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void saveCurrentScreenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				lastBitmap.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
			}
		}

		private void saveWorkingAreaScreenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap b = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
				using (Graphics g = Graphics.FromImage(b))
				{
					g.DrawImage(lastBitmap, 0, 0);
				}
				b.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
				b.Dispose();
			}
		}

		private void squareBracketsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EncloseText("[", "]");
		}

		private void roundBracesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EncloseText("(", ")");
		}

		private void curlyBracesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EncloseText("{", "}");
		}

		private void doubleQuotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EncloseText("\"", "\"");
		}

		private void singleQuotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EncloseText("\'", "\'");
		}

		private void EncloseText(string p1, string p2)
		{
			if (Clipboard.ContainsText())
			{
				char[] startWrappers = { '[', '(', '{', '"', '\'' };
				char[] endWrappers = { ']', ')', '}', '"', '\'' };

				string text = Clipboard.GetText();

				Clipboard.SetText(p1 + text + p2);
				ShowGearsBaloonTip("Success: Enclose text " + p1 + " " + p2, ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void sqlStringifyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				Clipboard.SetText(String.Format("'{0}'", RegExReplace("(')+", Clipboard.GetText(), "''")));
				ShowGearsBaloonTip("Success: SQL Stringify", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void clearTextFormattingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				string text = Clipboard.GetText();
				Clipboard.SetText(text);
				ShowGearsBaloonTip("Success: Cleare text formatting", ToolTipIcon.Info);
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void ConfigureRunAtStartup()
		{
			if (Properties.Settings.Default.RunAtStartup)
			{
				runAtStartupToolStripMenuItem.Checked = true;
				UpdateRunAtStartUpReg(true);
			}
			else
			{
				runAtStartupToolStripMenuItem.Checked = false;
				UpdateRunAtStartUpReg(false);
			}
		}

		private void UpdateRunAtStartUpReg(bool isRunAtStartup)
		{
			try
			{
				RegistryKey run = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
				if (isRunAtStartup)
				{
					run.SetValue("Gears", Environment.CommandLine);
				}
				else
				{
					run.DeleteValue("Gears", false);
				}
			}
			catch
			{
				ShowGearsBaloonTip("Error: Could not set value in registry", ToolTipIcon.Error);
			}

		}

		private void runAtStartupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			runAtStartupToolStripMenuItem.Checked = !runAtStartupToolStripMenuItem.Checked;
			UpdateRunAtStartUpReg(runAtStartupToolStripMenuItem.Checked);

			Properties.Settings.Default.RunAtStartup = runAtStartupToolStripMenuItem.Checked;
			Properties.Settings.Default.Save();
		}

		private void prettifyXMLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				try
				{
					string formatted = XDocument.Parse(Clipboard.GetText()).ToString();

					Clipboard.SetText(formatted);
					ShowGearsBaloonTip("Success: Prettify XML", ToolTipIcon.Info);
				}
				catch
				{
					Clipboard.Clear();
					ShowGearsBaloonTip("Error: Invalid XML content", ToolTipIcon.Error);
				}
			}
			else
			{
				ShowGearsBaloonTip("Error: No text available in clipboard", ToolTipIcon.Error);
			}
		}

		private void getWiFiIPv4AddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GetNetworkIP("Wi-Fi");
		}

		private void getEthernetIPv4AddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GetNetworkIP("Ethernet");
		}

		private void GetNetworkIP(string adapter)
		{
			try
			{
				if (!NetworkInterface.GetIsNetworkAvailable())
				{
					throw new Exception("Not connected to any network.");
				}

				string ipAddress = string.Empty;
				string netInfo = GetNetworkInformation(adapter, out ipAddress);

				Clipboard.SetText(ipAddress);

				ShowGearsBaloonTip($"Success: Get {adapter} IPv4 Address" + Environment.NewLine + netInfo, ToolTipIcon.Info);
			}
			catch(Exception ex)
			{
				Clipboard.Clear();
				ShowGearsBaloonTip("Error: " + ex.Message, ToolTipIcon.Error);
			}
		}

		private string GetNetworkInformation(string adapter, out string ipAddress)
		{
			StringBuilder netInfo = new StringBuilder(String.Empty);
			ipAddress = string.Empty;

			foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (item.OperationalStatus == OperationalStatus.Up && item.Name == adapter)
				{
					netInfo.AppendLine(item.Description);

					foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
					{
						if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
						{
							ipAddress = ip.Address.ToString();
							netInfo.AppendLine(ipAddress);
						}
					}
				}

			}

			if (netInfo.ToString() == string.Empty)
			{
				throw new Exception("Not connected to any network.");
			}

			return netInfo.ToString();
		}

		private void getHostnameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Clipboard.SetText(Environment.MachineName);
				ShowGearsBaloonTip("Success: Get Hostname" + Environment.NewLine + Environment.MachineName, ToolTipIcon.Info);
			}
			catch(Exception ex)
			{
				Clipboard.Clear();
				ShowGearsBaloonTip("Error: " + ex.Message, ToolTipIcon.Error);
			}
		}

		private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			ShowGearsBaloonTip(e.Data, ToolTipIcon.Info);
		}

		private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			ShowGearsBaloonTip(e.Data, ToolTipIcon.Error);
		}

		private void keepSystemAwakeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;
			bool enabled = (sender as ToolStripMenuItem).Checked;
			KeepSystemAwake(enabled);
		}

		private void KeepSystemAwake(bool enabled)
		{
			if (enabled)
			{
				PowerHelper.ForceSystemAwake();
			}
			else
			{
				PowerHelper.ResetSystemDefault();
			}
		}

		private void resetIIStoolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Process p = new Process();
				p.StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					FileName = "iisreset",
					UseShellExecute = false,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				};

				ShowGearsBaloonTip("Reset IIS initiated", ToolTipIcon.Info);

				p.Start();

				p.BeginErrorReadLine();
				p.ErrorDataReceived += P_ErrorDataReceived;

				p.BeginOutputReadLine();
				p.OutputDataReceived += P_OutputDataReceived;
			}
			catch (Exception)
			{
				ShowGearsBaloonTip("Error: Reset IIS", ToolTipIcon.Error);
			}
		}

		private void manageToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (serviceInstallerForm == null)
			{
				serviceInstallerForm = new ManageWindowsServicesForm();
				serviceInstallerForm.Show();
			}
			else if (serviceInstallerForm.IsDisposed)
			{
				serviceInstallerForm = new ManageWindowsServicesForm();
				serviceInstallerForm.Show();
			}
			else if (serviceInstallerForm.WindowState == FormWindowState.Minimized)
			{
				serviceInstallerForm.WindowState = FormWindowState.Normal;
				serviceInstallerForm.Focus();
			}
			else
			{
				serviceInstallerForm.Focus();
			}
		}

		private void searchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (searchToolForm == null)
			{
				searchToolForm = new SearchToolForm();
				searchToolForm.Show();
			}
			else if (searchToolForm.IsDisposed)
			{
				searchToolForm = new SearchToolForm();
				searchToolForm.Show();
			}
			else if (searchToolForm.WindowState == FormWindowState.Minimized)
			{
				searchToolForm.WindowState = FormWindowState.Normal;
				searchToolForm.Focus();
			}
			else
			{
				searchToolForm.Focus();
			}
		}

	}
}