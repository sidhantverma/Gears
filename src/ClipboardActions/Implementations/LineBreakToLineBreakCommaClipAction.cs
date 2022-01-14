using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gears
{
	public class LineBreakToLineBreakCommaClipAction : ClipboardAction
	{
		public override string Name { get; protected set; }

		public LineBreakToLineBreakCommaClipAction()
		{
			this.Name = "Line break to line break and comma";
		}

		public override string Execute(string input)
		{
			return RegExReplace("(\r\n)+", input, "," + Environment.NewLine);
		}
	}
}
