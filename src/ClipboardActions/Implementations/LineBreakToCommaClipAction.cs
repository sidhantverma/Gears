using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gears
{
	public class LineBreakToCommaClipAction : ClipboardAction
	{
		public override string Name { get; protected set; }

		public LineBreakToCommaClipAction()
		{
			this.Name = "Line break to comma";
		}

		public override string Execute(string input)
		{
			return RegExReplace("(\r\n)+", input, ", ");
		}
	}
}
