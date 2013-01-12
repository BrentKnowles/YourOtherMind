using System;
using CoreUtilities;

namespace Layout
{
	/// <summary>
	/// Note action.
	/// *1* A holder for various actions to appear on the TextNote Context Menu 
	/// *2* List is built in Main application form (supplied via AddIns and any default NoteActions, like "running as batch file"
	/// </summary>
	public class NoteTextAction
	{
		// A delegate holding the method defined when NoteTExtAction instantiated (i.e., performs the actual run as batch file action)
		Action<string> PerformAction;
		string MenuString=Constants.BLANK;
		string ToolTip=Constants.BLANK;
		public NoteTextAction (Action<string> performAction, string menuString, string tooltip)
		{
			PerformAction = performAction;
			MenuString = menuString;
			ToolTip = tooltip;
		}

		public string GetMenuTitle()
		{
			return MenuString;
		}
		public string GetMenuTooltip()
		{
			return ToolTip;
		}
		public void RunAction (string TextFromNote)
		{
			if (null == PerformAction) {
				NewMessage.Show (Loc.Instance.GetStringFmt ("The NoteTextAction '{0}' did not have a method assigned to it")); 
			} else {
				PerformAction(TextFromNote);
			}
		}
	}
}

