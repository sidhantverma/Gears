using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gears
{
	public abstract class ClipboardAction : IClipboardAction
	{
		public abstract string Name {get; protected set; }

		public abstract string Execute(string input);

		protected string RegExReplace(string pattern, string input, string replacement)
		{
			Regex re = new Regex(pattern);
			return re.Replace(input, replacement);
		}
	}
}
