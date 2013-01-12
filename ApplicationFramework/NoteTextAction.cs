using System;
using CoreUtilities;

namespace appframe
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
		// function used to create the filename the NoteTextAction expects to find the data it wants to work with
		// we allow this to be defined by the AddIns and whatnot because some devs will want to use Temp files, others with files of particular exensions
		Func<string> BuildFileName;
		public NoteTextAction (Action<object> performAction, Func<string> buildFileName, string menuString, string tooltip)
		{
			PerformAction = performAction;
			MenuString = menuString;
			ToolTip = tooltip;
			BuildFileName = buildFileName;

		}

		public string GetMenuTitle()
		{
			return MenuString;
		}
		public string GetMenuTooltip()
		{
			return ToolTip;
		}
		public string BuildTheFileName ()
		{
			if (null == BuildFileName) {
				NewMessage.Show (Loc.Instance.GetStringFmt ("The NoteTextAction '{0}' did not have a BuildFileName method assigned to it"));
			} else {
				return BuildFileName();
			}
			return Constants.BLANK;
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

