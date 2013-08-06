// NoteDataXML_Panel.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using Layout;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;

namespace LayoutPanels
{
	public class NoteDataXML_Panel :NoteDataXML
	{
		#region constants
		public override int defaultHeight { get { return 400; } }
		public override int defaultWidth { get { return 500; } }
		#endregion
		#region gui
		protected LayoutPanelBase panelLayout;
		#endregion
		#region variable
		public override bool IsPanel {
			get { return true;}
		}
		#endregion
		#region testingstub
		public void Add10TestNotes ()
		{
			for (int i=0; i <10; i++) {
				NoteDataXML note = new NoteDataXML();
				note.Caption = "hello there " + i.ToString();
				panelLayout.AddNote (note);
			}
		}
		#endregion
		// This is where it gets tricky. Need to modify the list of valid data types to store!
		public NoteDataXML_Panel () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Panel");




		}
		public override void Dispose ()
		{
			lg.Instance.Line("NoteDataXML_Panel->Dispose", ProblemType.MESSAGE, String.Format ("Dispose called for Note Panel Caption/Guid {0}/{1}", Caption, this.GuidForNote),Loud.ACRITICAL);
			base.Dispose ();
		}

		/// <summary>
		/// Gets the panels layout. This is used when autobuilding a panel inside a panel inside DefaultLayouts.cs
		/// </summary>
		public LayoutPanelBase GetPanelsLayout ()
		{
			return panelLayout;
		}
		public NoteDataXML_Panel(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Panel");

		}
		/// <summary>
		/// Gets the child notes.
		/// </summary>
		public  override System.Collections.ArrayList  GetChildNotes()
		{
			LayoutDatabase layout = new LayoutDatabase(this.GuidForNote);
			layout.LoadFrom(null);
			return layout.GetAllNotes();

		}
		protected override string GetIcon ()
		{
			return @"%*folder.png";
		}
		// names of subnotes, if a panel or equivalent notes (used for accessiblity as NoteDataXML_Panel not alwasy available)
		/// <summary>
		/// Lists the of subnotes. JUST THE NAMES
		/// 
		/// </summary>
		/// <returns>
		/// The of subnotes.
		/// </returns>
		public override System.Collections.Generic.List<string> ListOfSubnotes ()
		{
			System.Collections.Generic.List<string> output = new System.Collections.Generic.List<string> ();
			LayoutDatabase layout = new LayoutDatabase(this.GuidForNote);
			layout.LoadFrom(null);
			foreach (NoteDataInterface note in layout.GetAllNotes()) {
				output.Add(note.Caption);
			}
			return output;
		}
		/// <summary>
		/// Adds the note. (during a move operation. Called from LayoutPanel)
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public void AddNote (NoteDataInterface note)
		{
			if (null == panelLayout) {
				throw new Exception("No layout defined for this Subpanel. Did you remember to add it to a Layout?");
			}
			else
			panelLayout.AddNote (note);
			// jan 20 2013 - added this because i was doing an Update in LayoutPanel but that was causing
			// issues with destroying/disposing the original object
			//note.CreateParent(panelLayout);
		}
		public override void Save ()
		{
			base.Save ();


			// February 2013 - Confusion arises because both LayoutPanel and Notes have ParentGUID
			//    it is the notes ParentGUID that is blank, not the layoutpanel
			//NewMessage.Show(String.Format ("My {0} parent GUid is {1} ..", this.Caption, panelLayout.ParentGUID));
			// need some kind of copy constructor to grab things like Notebook and Section from the parent to give to the child panels
//			if (panelLayout.ParentGUID == Constants.BLANK) {
//				panelLayout.ParentGUID = GetAbsoluteParent ();
//			}
			//panelLayout.ParentGUID = Layout.GetAbsoluteParent().GUID;

			if (Layout != null) {

//				if (Layout.GUID != panelLayout.ParentGUID)
//				{
//					string message = Loc.Instance.GetStringFmt ("Parent ID is: {0} but this panel parent's ID is set to: {0}", 
//					                                            this.Layout.GUID, this.panelLayout.ParentGUID);
//					NewMessage.Show (message);
//				}

				panelLayout.SetParentFields (Layout.Section, Layout.Keywords, Layout.Subtype, Layout.Notebook);
				panelLayout.SaveLayout ();
			} else {
				NewMessage.Show ("Could not save a subpanel. Import error. Layout was null on " + Caption + " from? " );
			}

		}
		
		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			ParentNotePanel.BorderStyle = BorderStyle.Fixed3D;
		CaptionLabel.Dock = DockStyle.Top;


			ToolStripButton UpdateGuid = new ToolStripButton();
			UpdateGuid.Text = Loc.Instance.GetString ("Verify Parent");
			UpdateGuid.Click+= HandleVerifyParentClick;
			this.properties.DropDownItems.Add (UpdateGuid);

			panelLayout = new LayoutPanel(Layout.GUID, false);
			panelLayout.SetSubNoteSaveRequired = Layout.SetSaveRequired;

			// must set the Parent before loading
			panelLayout.Parent = ParentNotePanel;

			// load the layout based on the note
			panelLayout.LoadLayout(this.GuidForNote, true, Layout.GetLayoutTextEditContextStrip());

		
			panelLayout.Visible = true;
			panelLayout.Dock = DockStyle.Fill;
			panelLayout.BringToFront();
			panelLayout.SetParentLayoutCurrentNote = Layout.SetCurrentTextNote;

			/* I do not know why I duplicated this!
			ToolStripButton ShowTabs = new ToolStripButton(Loc.Instance.GetString("Show Tabs?"));
			ShowTabs.CheckOnClick = true;
			ShowTabs.Checked = panelLayout.ShowTabs;
			ShowTabs.CheckedChanged+= HandleCheckedChanged;
			
			properties.DropDownItems.Add (ShowTabs);
			*/
			
		}

		void HandleVerifyParentClick (object sender, EventArgs e)
		{

			string GUIDToSetTo = this.Layout.GUID;

			// here is where things get tricky
			// on a proper Layout ALL the subpanels inherit the ROOT ParentGUID
			// but the imported data messed this up
			// So if OUR PARENT has a PARENT then we use the PARENTGUID

			if (this.Layout.GetIsChild == true && this.Layout.ParentGuidFromNotes != Constants.BLANK) {
				GUIDToSetTo = this.Layout.ParentGuidFromNotes;
			}

			string message = Loc.Instance.GetStringFmt ("Parent ID is: {0}. Panel Parent ID says {1}.  Panel Parent Notes ID says: {2}", GUIDToSetTo,
			                                            this.panelLayout.ParentGUID, this.panelLayout.ParentGuidFromNotes);

			// issue I do not thik ParentGUID is the same as notes.ParentGuid and why not?
			if (NewMessage.Show (Loc.Instance.GetStringFmt ("Set Parent ID of this Panel to {0}?", GUIDToSetTo), 
			                     Loc.Instance.GetString (message),
			                     MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				this.panelLayout.SetParentGuidForNotesFromExternal(GUIDToSetTo);
			}

		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Panel");
		}

		void HandleCheckedChanged (object sender, EventArgs e)
		{
			panelLayout.ShowTabs = !panelLayout.ShowTabs;
			panelLayout.RefreshTabs();
			SetSaveRequired(true);
		}
		public NoteDataInterface FindSubpanelNote(NoteDataInterface note)
		{
			lg.Instance.Line("NoteDataXML_Panel->FindSubpanelNote", ProblemType.MESSAGE, String.Format ("Searching {0} for subnotes", this.Caption),Loud.CTRIVIAL); 
			return panelLayout.FindSubpanelNote(note);
		}

		public void ClearDrag()
		{
			panelLayout.ClearDrag();
		}
		protected override AppearanceClass UpdateAppearance ()
		{
			AppearanceClass app = base.UpdateAppearance ();

			if (null != app) {
				ParentNotePanel.BackColor = app.captionBackground;
				panelLayout.ColorToolBars(app);
			}
			return app;
		}
	}
}

