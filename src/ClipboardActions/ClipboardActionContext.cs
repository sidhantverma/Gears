using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gears
{
	public class ClipboardActionContext
	{
		private ClipboardAction _clipboardAction;

		public ActionResult ActionResult { get; private set; }

		public ClipboardActionContext(ClipboardAction clipboardAction)
		{
			this._clipboardAction = clipboardAction;
		}

		public ActionResult Execute(string input)
		{
			ActionResult = new ActionResult();

			try
			{
				ActionResult.Result = this._clipboardAction.Execute(input);
				ActionResult.Status = Status.Completed;
				ActionResult.StatusMessage = $"Success: {this._clipboardAction.Name}";
			}
			catch (Exception ex)
			{
				ActionResult.Status = Status.Failed;
				ActionResult.StatusMessage = $"Error: {this._clipboardAction.Name} {Environment.NewLine}Reason: {ex.Message}";
			}

			return ActionResult;
		}
	}

	public class ActionResult
	{
		public Status Status { get; set; }

		public string StatusMessage { get; set; }

		public string Result { get; set; }
	}

	public enum Status
	{
		Completed,
		Failed
	}
}
