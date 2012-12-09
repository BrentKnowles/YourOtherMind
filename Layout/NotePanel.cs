using System;
using System.Windows.Forms;
using System.Drawing;

namespace Layout
{
	/// <summary>
	/// The basic Floating Note
	/// No text on it.
	/// 
	/// In version 2.0 I will test whether it is sufficient that the NotePanel can descend into more
	/// advanced objects but the underlying NOTEDATA remains the same (that is fields that are neeed downstream remain.
	/// </summary>
	public class NotePanel : Panel, NotePanelInterface
	{
		#region variables
		NoteDataInterface Child;

		#endregion variables

		public NotePanel (NoteDataInterface child)
		{
			this.BackColor = Color.Beige;
			Child = child;



		}
	}
}

