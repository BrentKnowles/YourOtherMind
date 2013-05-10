// NoteDataXML_Timeline.cs
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
using CoreUtilities;
using CoreUtilities.Links;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Timeline;

using System.ComponentModel;
namespace Layout
{
	public class NoteDataXML_Timeline : NoteDataXML
	{

		#region xml_variables

		List<string> listoftableguids;

		/// <summary>
		/// Gets or sets the listoftableguids.
		/// 
		/// These are the tables that are associated with this timeline
		/// 
		/// NOTE: The first table is setup automatically when timeline created
		///    the others can only be hooked up via the Object Inspector. Advanced feature.
		/// </summary>
		/// <value>
		/// The listoftableguids.
		/// </value>
		[Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
		        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
		        typeof(System.Drawing.Design.UITypeEditor))]
		[TypeConverter(typeof(Layout.StringListConverter))]
		public List<string> Listoftableguids {
			get {
				return listoftableguids;
			}
			set {
				listoftableguids = value;
			}
		}



		bool hideZoomOutPanel=false;
		[Description("Hides the 'month' panel.")]
		[DisplayName("Hide Zoomed Out Panel?")]
		public bool HideZoomOutPanel {
			get {
				return hideZoomOutPanel;
			}
			set {
				hideZoomOutPanel = value;
			}
		}

		private int iconsPerColumn=3;

		/// <summary>
		/// Gets or sets the icons per column. Indicates how many icons should appear in each column, in a day-box on the timeline."
		/// </summary>
		/// <value>
		/// The icons per column.
		/// </value>
		public int IconsPerColumn {
			get {
				return iconsPerColumn;
			}
			set {
				iconsPerColumn = value;
			}
		}

		private DateTime timelineStartDate;

		public DateTime TimelineStartDate {
			get {
				return timelineStartDate;
			}
			set {
				timelineStartDate = value;
			}
		}

		bool tableCreated = false;
		// internal only. Tracks whether the table has been made yet (basically only the VERY first call to CreateParent, creates the associated table)
		// I DO NOT know why I put this in place? It is important[XmlIgnore]
		public bool TableCreated {
			get {
				return tableCreated;
			}
			set {
				tableCreated = value;
			}
		}
		// we supply a default rowfilter so users understand this feature and how it works
		private string rowfilter = "icon>=0";
		/// <summary>
		/// For Timelines, to filter the timeline
		/// </summary>
		[Description("Will filter the table(s) being used to populate the timeline.")]
		[DisplayName("Row Filter")]
		public string RowFilter
		{
			get { return rowfilter; }
			set
			{
				rowfilter = value;


				
			}

			
			
		}

		private Calendar.calendertype mCalendarType = Calendar.calendertype.Gregorian;
		/// <summary>
		/// Gets or sets the type of the Calendar.
		/// 
		/// User can sets this. The TimeLinePanel itself 'does something' with it
		/// </summary>
		/// <value>
		/// The type of the M calendar.
		/// </value>
		public Calendar.calendertype MCalendarType {
			get {
				return mCalendarType;
			}
			set {
				mCalendarType = value;


			}
		}

		#endregion
		NotePanelTimeline Timeline = null;

		public NoteDataXML_Timeline () : base()
		{
		}

		public NoteDataXML_Timeline (int _height, int _width): base(_height, _width)
		{
			// This is the CREATIOn constructor and should only be caleld when a New Note is made
			// This is because we will create the TABLE that needs to be paired with this note


			Listoftableguids = new List<string>();
			// the name for the default table
			string GuidOfTable = this.GuidForNote+"table";
			Listoftableguids.Add (GuidOfTable);


			// Cannot actually create the table here becaue Layout is null


		}


		protected override void CommonConstructorBehavior ()
		{
			base.CommonConstructorBehavior ();

			Caption = Loc.Instance.Cat.GetString("Timeline");
			
		}
		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			CaptionLabel.Dock = DockStyle.Top;


			if (TableCreated == false) {
				Width = 500;
				Height = 200;
				ParentNotePanel.Width = Width;
				ParentNotePanel.Height = Height;

				TimelineStartDate = DateTime.Today;

				// A. Create the Table
				NoteDataXML_Table myTable = new NoteDataXML_Table (100, 100, new ColumnDetails[7]{
					new ColumnDetails ("Date", 50),
					new ColumnDetails ("Type", 50),
					new ColumnDetails ("Data", 50),
					new ColumnDetails ("Data2", 50),
					new ColumnDetails ("Data3", 50),
					new ColumnDetails ("Data4", 50),
					new ColumnDetails ("icon", 50)});
				string GuidOfTable = this.GuidForNote + "table";
				myTable.GuidForNote = GuidOfTable;

				myTable.Caption = Loc.Instance.GetStringFmt ("Table for Timeline");

			


			
				Layout.AddNote (myTable);

				// we do not need to call CreateParent because AddNote does it for us
				//myTable.CreateParent (Layout);
				Save ();
				myTable.AddRow (new object[7]{DateTime.Today.ToString (), "100", "Example Description", "Example Title", "", "", 2});
				TableCreated = true;
				Save ();
				// B. Populate it with example Row
			}
			Timeline = new NotePanelTimeline (this);
			Timeline.Dock = DockStyle.Fill;
			ParentNotePanel.Controls.Add (Timeline);

			Timeline.BringToFront ();

			ToolStripMenuItem RowFilterStrip = 
				LayoutDetails.BuildMenuPropertyEdit (Loc.Instance.GetString ("Row Filter: {0}"), RowFilter, Loc.Instance.GetString ("Filter via the columns on the table associated with this timeline."), HandleRowFilterChange);



			ToolStripSeparator sep = new ToolStripSeparator();


			ToolStripComboBox dropper = new ToolStripComboBox ();




			dropper.ToolTipText = Loc.Instance.GetString ("Set the type of timeline you want by selecting an appropriate calendar.");
			dropper.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (Calendar.calendertype calendertype in Enum.GetValues (typeof(Calendar.calendertype))) {
				dropper.Items.Add (calendertype.ToString ());
			}
			dropper.Text = MCalendarType.ToString ();
			dropper.SelectedIndexChanged += HandleSelectedIndexCalendarPickerChanged;
		


			// Icons Per Column. We had the edit to a ToolStrip Host and add the Host to the toolstrip
			NumericUpDown numbers = new NumericUpDown ();


			Panel numbersPanel = new Panel ();
			numbersPanel.BackColor = dropper.BackColor;
			Label numbersLabel = new Label ();
			numbersLabel.Left = 0;
			//numbersLabel.Dock = DockStyle.Left;
			numbersLabel.Text = Loc.Instance.GetString ("Icons/Column: ");
			numbersLabel.AutoSize = false;
			numbersLabel.Width = 85;
			numbers.Left = 90;
			numbers.Width = 45;
			//numbersLabel.AutoSize = true;
			//numbers.Dock = DockStyle.Right;

			numbersPanel.Controls.Add (numbersLabel);
			numbersPanel.Controls.Add (numbers);
			numbersLabel.BringToFront ();
			//numbersPanel.Dock = DockStyle.Fill;

	
			numbers.Value = IconsPerColumn;
			numbers.ValueChanged += HandleIconsPerColumnValueChanged;
			numbers.Minimum = 1;
			numbers.Maximum = 6;
			ToolStripControlHost iconsPerColumn = new ToolStripControlHost (numbersPanel);


			DateTimePicker dates = new DateTimePicker ();
			dates.Width = 125;
			try {
				dates.Value = this.TimelineStartDate;
			} catch (Exception) {
				dates.Value = DateTime.Today;
			}
			dates.ValueChanged+= HandleValueCurrentdateChanged;
			ToolStripControlHost dateToolStrip = new ToolStripControlHost(dates);
			dateToolStrip.ToolTipText = Loc.Instance.GetString ("Select a date to center the timeline on that date.");



			properties.DropDownItems.Add (sep);
			properties.DropDownItems.Add (dropper);
			properties.DropDownItems.Add (iconsPerColumn);
			properties.DropDownItems.Add (RowFilterStrip);
			properties.DropDownItems.Add (dateToolStrip);

			//
			//
			// Hide ZOom Panel
			//
			//
			ToolStripButton HideMonths = new ToolStripButton();
			HideMonths.Text = Loc.Instance.GetString("Hide Months Panel");
			HideMonths.CheckOnClick = true;
			HideMonths.Checked = this.HideZoomOutPanel;
			HideMonths.Click+= HandleHideMonthsClick;

			properties.DropDownItems.Add (HideMonths);
			// Adjust panel as needed; also add this to the menu too
			Timeline.HideZoomPanel(this.HideZoomOutPanel);


		}

		void HandleHideMonthsClick (object sender, EventArgs e)
		{
			this.HideZoomOutPanel = (sender as ToolStripButton).Checked;
			Timeline.HideZoomPanel(this.HideZoomOutPanel);
			Timeline.Refresh();
		}

		void HandleRowFilterChange (object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				string rowfilter = RowFilter;
				LayoutDetails.HandleMenuLabelEdit (sender, e, ref rowfilter, SetSaveRequired);
				RowFilter = rowfilter;

				// update based on filter setting
				Timeline.Refresh ();
			}
		}

		void HandleValueCurrentdateChanged (object sender, EventArgs e)
		{
			this.TimelineStartDate = (sender as DateTimePicker).Value;

			Timeline.ResetTimeline(MCalendarType);


			Timeline.Refresh ();
			SetSaveRequired(true);
		}

		void HandleIconsPerColumnValueChanged (object sender, EventArgs e)
		{
			IconsPerColumn = (int)(sender as NumericUpDown).Value;
		

			Timeline.Refresh();
			SetSaveRequired(true);
		}

		void HandleSelectedIndexCalendarPickerChanged (object sender, EventArgs e)
		{
			string value =(sender as ToolStripComboBox).Text;
			MCalendarType = (Calendar.calendertype)Enum.Parse (typeof(Calendar.calendertype), (value));
			if (MCalendarType == Calendar.calendertype.Plot)
			{
				// we override the Start Date if we set this to a Plot Calendar
				TimelineStartDate = new DateTime(NotePanelTimeline.PLOT_DEFAULT_YEAR, 1,  1);
			}
			else
			{
				// we reset the timelinedate
				TimelineStartDate = DateTime.Today;
			}




			Timeline.ResetTimeline(this.MCalendarType);
			Timeline.Refresh ();
			SetSaveRequired(true);
		}



		public override void Save ()
		{
			base.Save ();
		}
		public override string RegisterType ()
		{
			return Loc.Instance.Cat.GetString("Timeline");
		}

		private bool errorthrown=false;
		/// <summary>
		/// Gets the table for this timeline.
		/// </summary>
		/// <returns>
		/// The table for this timeline.
		/// </returns>
		/// <param name='TimelineTableGuid'>
		/// We pass in the GUID to allow other tables to be found (i.e., if you have made a list of tables to associate with this timeline)
		/// </param>
		public NoteDataXML_Table GetTableForThisTimeline (string TimelineTableGuid)
		{
			NoteDataXML_Table FoundTable = (NoteDataXML_Table)Layout.FindNoteByGuid (TimelineTableGuid);

			// April 1 2013 - this may make things TOO SLOW
			// but only way to guarantee al ive connection when the TIMELINE is not in the same panel as the TABLES
			FoundTable = (NoteDataXML_Table)Layout.GoToNote(FoundTable);
			if (null == FoundTable && false==errorthrown) {
				// just throw message once.
				NewMessage.Show (Loc.Instance.GetString ("The table for this Timeline was not found. Probably the guid of it needs to be set to its caption."));
				errorthrown=true;
				//throw new Exception("The table for this Timeline was not found");
			}
			return FoundTable;
		}

		/// <summary>
		/// Determines whether this instance is loaded. Passes the value from the layout
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLoaded()
		{
			if (Layout == null) return false;

			return Layout.IsLoaded;
		}
		public NoteDataXML_Timeline(NoteDataInterface Note) : base(Note)
		{
			
		}
		public override void CopyNote (NoteDataInterface Note)
		{
			base.CopyNote (Note);
		}
		protected override AppearanceClass UpdateAppearance ()
		{
			AppearanceClass app = base.UpdateAppearance ();
			if (Timeline != null) {
				Timeline.GradientColor1 = app.captionBackground;
				Timeline.GradientColor2 = app.captionForeground;
				Timeline.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
			}
			return app;
		}
	}
}