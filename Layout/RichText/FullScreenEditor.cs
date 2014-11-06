using System;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;

namespace Layout
{
	public class FullScreenEditor : System.Windows.Forms.Form
	{
		RichTextExtended RichTextBox = null;
		NoteDataXML_RichText originalNote;
		Panel hud = null;
		Label wordLabel = null;
		int startingWords = 0;
		NoteNavigation bookMarkView;
		private void OpenNoteNav()
		{
		

		}
		private void AddBookmarkView ()
		{
		

			
			
			//bookMarkView.Location = new Point (richBox.Location.X, richBox.Location.Y);
			
			//bookMarkView.Height = richBox.Height;
			bookMarkView.Width = 200;//((int)richBox.Width / 2);
			if (bookMarkView.Width > bookMarkView.MAX_NAVIGATION_WIDTH)
				bookMarkView.Width = bookMarkView.MAX_NAVIGATION_WIDTH;
			
			bookMarkView.Dock = DockStyle.Left;
		
			if (true == bookMarkView.Visible) {
				bookMarkView.UpdateListOfBookmarks ();
			}
		}
		public FullScreenEditor (NoteDataXML_RichText theNote)
		{


			this.KeyPreview = true;
			this.StartPosition = FormStartPosition.Manual;
			var screen = Screen.FromPoint(Cursor.Position);
			this.Location = screen.Bounds.Location;
			this.WindowState = FormWindowState.Maximized;
			//this.DesktopBounds = MyWind

			if (null == theNote) throw new Exception("theNote was invalid in FullSCreenEditor");
			originalNote = theNote;

			RichTextBox = new RichTextExtended();
			RichTextBox.Dock = DockStyle.Fill;
			this.Controls.Add (RichTextBox);
//			Panel tempToHideRTFBorder = new Panel();
//			this.Controls.Add (tempToHideRTFBorder);
//			tempToHideRTFBorder.Controls.Add (RichTextBox);
//			tempToHideRTFBorder.Dock = DockStyle.Fill;
//			tempToHideRTFBorder.BorderStyle = BorderStyle.Fixed3D;

			RichTextBox.Rtf = theNote.Data1;
			RichTextBox.BorderStyle = BorderStyle.None;
			RichTextBox.BackColor = theNote.GetRichTextBox().BackColor;


			// pretty up
			this.ControlBox = false;
			this.Text = string.Empty;
			this.BackColor = RichTextBox.BackColor;
			// this works but you see richTextBorder 
			this.Padding = new System.Windows.Forms.Padding(15);
			//RichTextBox.Margin= new System.Windows.Forms.Padding(10);

			hud = new Panel();
			this.Controls.Add (hud);
			hud.Dock = DockStyle.Bottom;
			hud.Height = this.Height / 5;
			hud.BackColor = System.Drawing.Color.White;
			hud.ForeColor = System.Drawing.Color.Black;
			hud.BringToFront();

			Button closeButton = new Button();
			closeButton.Text = Loc.GetStr("Close");
		
			closeButton.Dock = DockStyle.Bottom;
			closeButton.Click+= (object sender, EventArgs e) => {
				Close ();
			};
			closeButton.ForeColor = this.ForeColor;
		
			Button outlinerButton = new Button();
			hud.Controls.Add (outlinerButton);
			outlinerButton.Text = Loc.GetStr ("Outline");
			outlinerButton.Dock = DockStyle.Bottom;
			outlinerButton.Click+= (object sender, EventArgs e) => {
				bookMarkView.Visible = !bookMarkView.Visible;
				if (bookMarkView.Visible)
				{
					AddBookmarkView();
				}
			};
			


			wordLabel = new Label();
			startingWords = LayoutDetails.Instance.WordSystemInUse.CountWords(RichTextBox.Text);
			wordLabel.Text = Loc.Instance.GetStringFmt ("{0} words", startingWords);
			//wordLabel.BackColor = this.BackColor;
			//wordLabel.ForeColor = RichTextBox.ForeColor;
			hud.Visible = false;
			hud.Controls.Add (closeButton);
			hud.Controls.Add (wordLabel);



			// outliner

			bookMarkView = new NoteNavigation (originalNote);
			this.Controls.Add (bookMarkView);
			bookMarkView.Dock = DockStyle.Left;
			bookMarkView.Visible = false;

			RichTextBox.BringToFront ();
		}


		protected override void OnMouseMove (MouseEventArgs e)
		{
			base.OnMouseMove (e);
			if (hud.Visible == false) {
				RefreshWordCount();
				hud.Visible = true;

			}
		}
		private void RefreshWordCount()
		{
			int currentWords = LayoutDetails.Instance.WordSystemInUse.CountWords(RichTextBox.Text);
			int newWords = currentWords-startingWords;
			wordLabel.Text = Loc.Instance.GetStringFmt ("{0} (+{1}) words", currentWords, newWords);
		}

		protected override void OnKeyUp (KeyEventArgs e)
		{
			base.OnKeyUp (e);
			// any key pressing hides the panel again
			hud.Visible = false;

			if (e.KeyCode == Keys.Escape) {
				this.Close ();
			}

			if (e.Control == true) {
				if (e.KeyCode == Keys.S) {
				//	NewMessage.Show ("Save");
					SaveText ();
				}
			}
		}

		/// <summary>
		/// Saves the text.
		/// </summary>
		private void SaveText ()
		{
			if (originalNote != null) {
				originalNote.GetRichTextBox ().Rtf = RichTextBox.Rtf;
				originalNote.Save ();		
			}
		}

		/// <summary>
		/// Gets the RT.
		/// </summary>
		/// <returns>
		/// The RT.
		/// </returns>
		public string GetRTF()
		{
			return RichTextBox.Rtf;
		}
		/// <summary>
		/// Raises the closing event.
		/// </summary>
		/// <param name='e'>
		/// E.
		/// </param>
		protected override void OnClosing (System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing (e);

			//NewMessage.Show ("Closign");
			SaveText ();



		}
	}
}

