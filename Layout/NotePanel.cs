using System;
using System.Windows.Forms;
using System.Drawing;
/// <summary>
/// Note panel.
/// 
/// Core resizing: combination of my old code from earlier version of YOM + http://social.msdn.microsoft.com/Forums/en-US/Vsexpressvcs/thread/a184f136-0e54-4b56-86be-9a1c57212344/
/// </summary>
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

		private const int cGripSize = 20;
		private bool mDragging;
		private Point mDragPos;

		#endregion variables

		public NoteDataInterface GetChild ()
		{
			return Child;
		}

		public void BringToFrontChild ()
		{
			if (Child != null) {
				Child.BringToFrontAndShow();
			}
		}


		public NotePanel (NoteDataInterface child)
		{
			this.BackColor = Color.Beige;
			Child = child;
			// removing DoubleBuffered had no impact on load time performance
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.Padding = new Padding(4,0,4,5);

		}
		protected override void OnPaint (PaintEventArgs e)
		{
			if (Child.Dock == DockStyle.None) {
				ControlPaint.DrawSizeGrip (e.Graphics, this.BackColor,
			                          new Rectangle (this.ClientSize.Width - cGripSize, this.ClientSize.Height - cGripSize, cGripSize, cGripSize));
			}
			base.OnPaint(e);
		}
		private bool IsOnGrip(Point pos) {
			return pos.X >= this.ClientSize.Width - cGripSize &&
				pos.Y >= this.ClientSize.Height - cGripSize;
		}
		protected override void OnMouseLeave (EventArgs e)
		{
			if (Child.Dock == DockStyle.None) {
				mDragging = false;
				this.Cursor = Cursors.Default;
			}
			base.OnMouseLeave (e);
		}
	
		protected override void OnMouseDown (MouseEventArgs e)
		{
			if (Child.Dock == DockStyle.None) {
				mDragging = IsOnGrip (e.Location);
				mDragPos = e.Location;
			
			}
			base.OnMouseDown(e);
		}
		
		protected override void OnMouseUp (MouseEventArgs e)
		{
			Child.RespondToNoteSelection();
			if (Child.Dock == DockStyle.None) {
				mDragging = false;
				this.Cursor = Cursors.Default;
			}
			base.OnMouseUp(e);
		}
		
		protected override void OnMouseMove (MouseEventArgs e)
		{
			if (Child.Dock == DockStyle.None) {
				if (mDragging) {
					CoreUtilities.lg.Instance.Line ("NotePanel->MouseMove", CoreUtilities.ProblemType.TEMPORARY, "we are dragging.");

					Child.Height = this.Height + e.Y - mDragPos.Y;
					Child.Width = this.Width + e.X - mDragPos.X;
					Child.SetSaveRequired (true);
					this.Size = new Size (this.Width + e.X - mDragPos.X,
				                     this.Height + e.Y - mDragPos.Y);
					mDragPos = e.Location;
				} else if (IsOnGrip (e.Location))
					this.Cursor = Cursors.SizeNWSE;
				else {
					this.Cursor = Cursors.Default;
					mDragging = false;
				}
			}
			base.OnMouseMove(e);
		}
	}
}

