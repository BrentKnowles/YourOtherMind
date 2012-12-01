using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

using CoreUtilities;
/*This is Just an Experiment.
 * 
 * Trying to see if I can organize the notes in a better way
 * 
 * Goals
 * 1. Easy, logical maintenance
 * 2. Able to store data elsewhere
 * 3. Faster loading of data
 * 4. Logical access to data both from the LayoutPanel itself and 
 * 5. from external places requesting data (the random table system)
 */
namespace Layout
{
	public class LayoutPanel : Panel, LayoutPanelInterface
	{

		// storing reference to Interface, allowing a data swapout later
		LayoutInterface Notes = null;

		public LayoutPanel ()
		{
			this.BackColor = Color.Pink;

			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;
			ToolStripButton addNote = new ToolStripButton ("boom");

			addNote.Click += HandleAddClick;

			bar.Items.Add (addNote);

			Notes = new LayoutDatabase("fakeguid2");

			for (int i = 0; i < 2; i++) {
				NoteDataXML xml = new NoteDataXML ();
				xml.Caption = "tree frog" + i.ToString();



				Notes.Add (xml);

				xml.CreateParent ();
			}

			Notes.SaveTo();

			/*TODO:
			Thoughts: Does the NoteData actually create the Panel (i.e., flip the ownership around)
			 The NoteData will never REFERENCE the actual GUI elements however.
			 Also play with the Unit test example that StackOverflow had, this seems interest
			 */
		}

		public void LoadLayout (string GUID)
		{
			// To Do: Not done yet, obviously
			Notes.LoadFrom (GUID);
			// Notes now contain the file contents
			// Now we have to create the Visual Representation
			foreach (NoteDataInterface data in Notes.GetNotes()) {
				data.CreateParent();
			}
		}

		void HandleAddClick (object sender, EventArgs e)
		{

			foreach (NoteDataInterface data in Notes.GetNotes()) {
				NewMessage.Show (data.Caption);
				Console.WriteLine(((NoteDataXML)data).JustXMLONLYTest);
				//data.CreateParent(); This is bad design though: use the itnerface.
			}



		}
	}
}

