using System;
using System.Windows.Forms;

namespace Gears
{
	public partial class DrawingTextInputForm : Form
	{
		MainForm gearsUIForm;

		public DrawingTextInputForm(MainForm f)
		{
			InitializeComponent();
			this.gearsUIForm = f;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			gearsUIForm.DrawingText = textBox1.Text;
			gearsUIForm.drawText = true;
			this.Close();
		}

		private void InputForm_Load(object sender, EventArgs e)
		{
			textBox1.Text = gearsUIForm.DrawingText;
			textBox1.Font = gearsUIForm.DrawingFont;
			textBox1.ForeColor = gearsUIForm.DrawingColor;
			textBox1.SelectionLength = 0;
		}
	}
}
