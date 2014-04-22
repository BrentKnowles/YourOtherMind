// TimelinePanel.cs
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
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CoreUtilities;
using System.Data;
using Layout;

namespace Timeline
{
	public class NotePanelTimeline : Panel
	{
		#region variables
		private Hashtable colorHash;
		public	Color GradientColor1 = Color.Pink;
		public	Color GradientColor2 = Color.Green;
		public LinearGradientMode GradientMode= LinearGradientMode.Horizontal;
		// builds a list of tables in Paint, the first time it is run
		System.Collections.Generic.List<NoteDataXML_Table> MyTables = new System.Collections.Generic.List<NoteDataXML_Table>();
		NoteDataXML_Timeline MyTimeline = null;

		private System.ComponentModel.IContainer components = null;
		public static int PLOT_DEFAULT_YEAR = 1999; // can't be 0	
		public static string DELETE_ENTRY = ";;;Delete;;;";
	
		private bool mutex_AddButton = false; // can probalby get rid of

		// held copy of the Table so we don't load it each time we paint
		NoteDataXML_Table CurrentVersionofTableForTHisTimeline=null;
		
		
		// this list of smartpoints is regenerated every paint cycle
		private ArrayList smartPoints=new ArrayList();


		public newGenericDate startDate = new newGenericDate(DateTime.Now.Year, 1, 1);
		
		public Calendar calendar = null;
		
		
		string sMonth = "";
		int nMonth = -1;

		private Panel panel1;
		private System.Windows.Forms.Panel panelZoomIn;
		public System.Windows.Forms.Panel panelZoomOut;
		public int Year = 0;
		public int dayPanelWidth = 100;
		
		int nHalfwayDay = 0;
		int nHalfwayMonth = 0;
		int nHalfwayYear = 0;
		
		// appearance modified settings
		public Brush dayTextBrush =  new SolidBrush(Color.Black);
		public System.Windows.Forms.ImageList imageList1;
		public Font dayTextFont= new Font("Times", 8);
		public Font monthTextFont  = new Font("Georgia", 20);
		public Brush monthTextBrush = new SolidBrush(Color.Pink);
		private System.Windows.Forms.ContextMenu hack2;
		
		public NotePanel lastOpenNote = null;
		#endregion

		private void SetUpCalendar (Calendar.calendertype newCalendar)
		{
			// this is why you might reset the Timeline, if you changed the calendar used
			switch (newCalendar) {
			case Calendar.calendertype.Gregorian: this.calendar = Calendar.BuildGregorianDefault(); break;
			case Calendar.calendertype.Plot:this.calendar = Calendar.BuildPlotDefault(); break;
			}

		}

		public bool mHideZoomOutPanel = false;

		public void HideZoomPanel (bool hideZoomOutPanel)
		{
			// HIDE TIMELINE
			if (hideZoomOutPanel == true && mHideZoomOutPanel == false) {
				Height = Height / 2;
			}
					// new value false old value true
					else 
				if (hideZoomOutPanel == false && mHideZoomOutPanel == true) {
				Height = Height * 2;
			}
			panelZoomOut.Visible = !hideZoomOutPanel;

		}
	
		/// <summary>
		/// called when changing calendar types.
		/// </summary>
		public void ResetTimeline (Calendar.calendertype newCalendar)
		{
			SetUpCalendar(newCalendar);
			CurrentVersionofTableForTHisTimeline = null;

			nHalfwayDay = 0;
			nHalfwayMonth = 0;
			nHalfwayYear = 0;
			//startDate = new newGenericDate(DateTime.Now.Year, 1, 1);
			startDate = new newGenericDate(MyTimeline.TimelineStartDate);//new newGenericDate(PLOT_DEFAULT_YEAR,1,1);
			panel1.Invalidate();
			RefreshMe ();
		}


		// move this code to somewhere suitable? Convert file to a table?






		


	


		
		public NotePanelTimeline (NoteDataXML_Timeline myTimeline)
		{
			if (null == myTimeline) {
				throw new Exception("No timeline NOTETYPE defined");
			}

			SetUpCalendar(myTimeline.MCalendarType);
			//this.calendar = myTimeline.Calendar;
			InitializeComponent();
			 MyTimeline = myTimeline;
			// This call is required by the Windows Form Designer.
			this.Controls.Add (panel1);
			panel1.Dock = DockStyle.Fill;


			Year = PLOT_DEFAULT_YEAR;
			
			//menuAddEditTimeline.Text = Loc.Instance.GetString ("Edit Timeline");//Worgan2006.Properties.Resources.EditTimelineFromTimeline;
			// Mes sageBox.Show(this.Name);
			Name = "NotePanelTimeline"; /// this is how we find out if there is a timeline when trying to edit or assing notes
			startDate = new newGenericDate(MyTimeline.TimelineStartDate);
			
			//	defaultAppearance = Appearance.appearanceType.Classic;
			// Add any initialization after the InitializeComponent call
		}
		
		private Color zoomOutBackgroundColor;
		public Color ZoomOutBackgroundColor
		{
			get {return zoomOutBackgroundColor;}
			set {zoomOutBackgroundColor = value;
				panelZoomOut.BackColor = value;
			}
		}
		
		
		//repaints the panels
		public void RefreshMe()
		{
			panelZoomIn.Refresh();
			panelZoomOut.Refresh();
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{



			this.panel1 = new Panel();

			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotePanelTimeline));
			this.panelZoomIn = new System.Windows.Forms.Panel();
			this.panelZoomOut = new System.Windows.Forms.Panel();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.hack2 = new System.Windows.Forms.ContextMenu();
			this.panel1.SuspendLayout();
	//		this.panelHeader.SuspendLayout();
	//		((System.ComponentModel.ISupportInitialize)(this.pinBox)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panelZoomIn);
			this.panel1.Controls.Add(this.panelZoomOut);
			this.panel1.Cursor = System.Windows.Forms.Cursors.SizeAll;
		//	this.panel1.MouseHover+= HandleMouseHover;;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
			this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
		//	this.panel1.Controls.SetChildIndex(this.panellink, 0);
		//	this.panel1.Controls.SetChildIndex(this.panelHeader, 0);
			this.panel1.Controls.SetChildIndex(this.panelZoomOut, 0);
			this.panel1.Controls.SetChildIndex(this.panelZoomIn, 0);
			// 
			// contextMenu1
			// 
		//	this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
			// 
			// panelHeader
			// 
		//	this.panelHeader.BackColor = System.Drawing.Color.LemonChiffon;
	//		this.panelHeader.Location = new System.Drawing.Point(0, 17);
			// 
			// panellink
			// 
		//	this.panellink.Location = new System.Drawing.Point(0, 0);
			// 
			// panelZoomIn
			// 
			this.panelZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelZoomIn.Location = new System.Drawing.Point(0, 33);
			this.panelZoomIn.Name = "panelZoomIn";
			this.panelZoomIn.Size = new System.Drawing.Size(155, 24);
			this.panelZoomIn.TabIndex = 2;
			this.panelZoomIn.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			this.panelZoomIn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelZoomIn_MouseMove);
			this.panelZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
			// 
			// panelZoomOut
			// 
			this.panelZoomOut.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelZoomOut.Location = new System.Drawing.Point(0, 57);
			this.panelZoomOut.Name = "panelZoomOut";
			this.panelZoomOut.Size = new System.Drawing.Size(155, 69);
			this.panelZoomOut.TabIndex = 3;
			this.panelZoomOut.Paint += new System.Windows.Forms.PaintEventHandler(this.panelZoomOut_Paint);
			this.panelZoomOut.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelZoomOut_MouseMove);
			this.panelZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelZoomOut_MouseDown);
			this.panelZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelZoomOut_MouseUp);
			// 
			// imageList1
			// 
			//this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("stop.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("sport_golf.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("star.png"));

			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("sport_8ball.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("sport_basketball.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tag_blue.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tag_green.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tag_orange.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tag_red.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tag_yellow.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("tick.png"));
			this.imageList1.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("television.png"));
			this.imageList1.Images.SetKeyName(0, "");
			this.imageList1.Images.SetKeyName(1, "");
			this.imageList1.Images.SetKeyName(2, "");
			this.imageList1.Images.SetKeyName(3, "");
			this.imageList1.Images.SetKeyName(4, "");
			this.imageList1.Images.SetKeyName(5, "");
			this.imageList1.Images.SetKeyName(6, "");
			this.imageList1.Images.SetKeyName(7, "");
			this.imageList1.Images.SetKeyName(8, "");
			this.imageList1.Images.SetKeyName(9, "");
			this.imageList1.Images.SetKeyName(10, "");
			this.imageList1.Images.SetKeyName(11, "");
			this.panel1.ResumeLayout(false);
		//	this.panelHeader.ResumeLayout(false);
		//	((System.ComponentModel.ISupportInitialize)(this.pinBox)).EndInit();
			this.ResumeLayout(false);
			
		}

//		void HandleMouseHover (object sender, EventArgs e)
//		{
//			if (smartPoints != null)
//			{
//				foreach(SmartPoint smart in smartPoints)
//				{
//					Point point = System.Windows.Forms.Cursor.Current.HotSpot;
//					if ( (point.X >= smart.x && point.X <= smart.x+10) && (point.Y >= smart.y && point.Y <= smart.y+10))
//					{
//						//Message Box.Show(smart.s);
//						//VirtualDesktop myParent = (VirtualDesktop)this.Parent.Parent;
//						if (smart.s != null && smart.s.Length > 0 && smart.s[0] == '*')
//						{
//							// may 2012 - if clicking on a note from a DataTable we
//							// should pop open the secondary information
//							NewMessage.Show(smart.s);
//						}
//						//else
//						//	myParent.OpenNoteWindow(smart.s, this, this.Left, this.Height);
//						return;
//					}
//				}
//			}
//		}
#endregion
		
	
		
		
		/// <summary>
		/// Handles the actual drawing of a note
		/// </summary>
		/// <param name="s"></param>
		/// <param name="nNumberFound"></param>
		/// <param name="AutoIcon">if > 0 will generate an auto icon, with this number</param>
		private void AddTimelineNote (string description, int nImage, ref int nNumberFound,
		                             int i,
		                             System.Windows.Forms.PaintEventArgs e,
		                             string sCaption, int cellWidth, int AutoIcon, string ColumnData3, int chapter, int ColorOverride)
		{ // october 20 2008 - filters out entries with DELETE_ENtry to allow proper deletion
			// if (sCaption != DELETE_ENTRY)
			{
			

				int nVert = 0; // starts off stacking vertically but will do one more colum
				
				// this keeps track of how many we found so that we can
				// stak them on top of one another
				nNumberFound++;
				if (nNumberFound > MyTimeline.IconsPerColumn) {
					nVert = nVert + 20;
					nNumberFound = 1;
					
				}
				mutex_AddButton = true;
				int nLeft = (i - 1) * dayPanelWidth + nVert;
				int nTop = 10 + ((nNumberFound - 1) * 20);
				
				int nImageIndex = nImage;
				
				string originalcaption = sCaption;
				
				if (sCaption != "" && sCaption != DELETE_ENTRY/*and option to show aptions*/) {
					bool adjusted = false;

					while (e.Graphics.MeasureString(sCaption, dayTextFont).Width+1 > cellWidth) {
						sCaption = sCaption.Substring (0, sCaption.Length / 2);
						adjusted = true;
//						e.Graphics.DrawString(newCaption, dayTextFont, dayTextBrush, nLeft + 16,
//						                      nTop);
					}
					//else
					{
						if (true == adjusted) {
							sCaption = sCaption + "...";
						}
						int positionModifier = 0;
						if (AutoIcon > 0)
						{
							// adjust the size with use
							positionModifier = 6;
							if (chapter > 0) {
								positionModifier = positionModifier + 6;
								if (chapter >9)
								{
									positionModifier = positionModifier + 2;
								}
							}
						}
						e.Graphics.DrawString (sCaption, dayTextFont, dayTextBrush, nLeft + 16 + positionModifier,
					                      nTop);
						if (false == adjusted) {
							// now blank the captionb ecause we DO NOT WANT
							// a tooltip for those that don't need it
							originalcaption = "";
						}



					}
				}

				// if imageindex was not set then just default to 0
				if (-1 == nImageIndex)
				{
					nImageIndex = 0;
				}


				if (nImageIndex != -1) {
					
					if (nImageIndex > imageList1.Images.Count - 1) {
						// to avoid user putting in icons that do not exist
						nImageIndex = 0;
					}
					if (AutoIcon > 0) {
						if (colorHash == null) {
							colorHash = new Hashtable ();
						}
						const int ColorCount = 12;

						Color[] ColorsToUse = new Color[ColorCount]{Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Purple,
							Color.Black, Color.White, Color.Brown, Color.LightBlue, Color.Orange, Color.Beige, Color.MediumTurquoise};

						// if we are the first in an entry (see below) the frame color will be changed to GOLD
						//Color FrameColor = Color.White;
						FontStyle fontStyle = FontStyle.Bold;
						int ColorToUseIdx = 0;
						//we halve the count because we are adding both COLOR and COUNT PER CATEGORY
						// so we have 2x as many entries

						if (ColorOverride >-1 && ColorOverride < ColorCount)
						{
							ColorToUseIdx = ColorOverride;
						}
						else
						if (colorHash.Count / 2 < ColorsToUse.Length - 1) {
							ColorToUseIdx = colorHash.Count / 2;
						}
						Color ColorForBackGround = Color.Black;
						if (colorHash [ColumnData3] == null) {
							colorHash.Add (ColumnData3, ColorsToUse [ColorToUseIdx]);
							ColorForBackGround = ColorsToUse [ColorToUseIdx];

							// This means this is the first in an entry added
							//FrameColor = Color.Gold;
							fontStyle = fontStyle | FontStyle.Underline;

						} else {
							ColorForBackGround = (Color)colorHash [ColumnData3];
						}
						General.TextToImageAppearance app = new General.TextToImageAppearance ();
						app.BackgroundColor = ColorForBackGround;
					//	app.TopWidth = 100;
						//app.FrameColor = FrameColor;

					//	app.LeftWidth = 5;

						app.TitleColor = CoreUtilities.TextUtils.InvertColor (app.BackgroundColor);
						//app.

						app.TitleFont = new Font ("Garamond", 10.0f, fontStyle);

						// now we need to figure out the number of items (because some columns can be double upped and we want to autoincrmenet numbers
						int numbertoshow = 1;
						if (colorHash [ColumnData3 + "count"] == null) {
							colorHash.Add (ColumnData3 + "count", 1);
						} else {
							colorHash [ColumnData3 + "count"] = ((int)colorHash [ColumnData3 + "count"]) + 1;
							numbertoshow = ((int)colorHash [ColumnData3 + "count"]);
						}
						string sChapter = "";
						string spacing = " ";
						if (chapter > 0) {
							sChapter = "(" + chapter.ToString () + ")";
							// we trim this up a bit so it does not take up too much room
							spacing = "";
						}
						Image backOfAutoIcon = null;
						if (nImageIndex > 0)
						{
							backOfAutoIcon = imageList1.Images [nImageIndex];
				
						}
						Bitmap b = CoreUtilities.General.CreateBitmapImageFromText (spacing + numbertoshow + sChapter + spacing, "", General.FromTextStyles.CUSTOM, -1, app, backOfAutoIcon);

						e.Graphics.DrawImage (b, nLeft, nTop);
					} else {
						e.Graphics.DrawImage (imageList1.Images [nImageIndex], nLeft, nTop);
					}
					if (description != "" && description != null && description != "*") {
						// add description to mouseover text
						originalcaption = originalcaption + Environment.NewLine + description;
					}
					smartPoints.Add (new SmartPoint (nLeft, nTop, description, originalcaption));
				}
			}
		}

		/// <summary>
		/// painting the zoomed in panel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// clear this because we keep count of certain albels
			colorHash = null;

			if (MyTimeline.IsLoaded() == false) return;

			try
			{
				if (mutex_AddButton == true) return;
				
				if (smartPoints != null) smartPoints.Clear();
				
				Panel thisPanel = (Panel)sender;
				// delete all buttons
				/*	foreach (Control control in thisPanel.Controls)
					{
						if (control.GetType() == typeof(Button))
						{
							thisPanel.Controls.Remove(control);
						}
					}*/
				// * need to figure out initialization better
				//if (nMonth == -1)
				{
					nMonth = startDate.Month;
					Year = startDate.Year;
					
					
				}
				
				
				// boundary checking for generic calendars BUT 
				// before I removed the reliance on startDate
				if (nMonth > calendar.NumberOfZoomOutPanels)
				{
					nMonth = calendar.NumberOfZoomOutPanels;
				}
				
				
				// custom drawing
				// just experiments right now
				//e.Graphics.DrawEllipse(Pens.Green, 30, 90, 20, 10);
				//e.Graphics.DrawLine(new Pen(Color.Black), 0, 0, Width, Height);
				Pen dayPanelPen = new Pen(Color.Gray);
				

				// 50 works when cell width = 100
				int dayTextWidth = dayPanelWidth/2 + dayPanelWidth/2;//50;
				int dayTextHeight = (int) (thisPanel.Height / 1.25);
				
				int nDaysInCurrentMonth = 0; //calendar.GetDaysInMonth(Year, 10);
				
				// hold the original starting date. We reset it at the end.
				newGenericDate startingStartDate = startDate;
				
				
				int nstartDate = startDate.Day;
				
				// figure out how many days CAN appear
				int nViewableDays = (int)(thisPanel.Width/dayPanelWidth) + 1;
				
				// display viewable days and year 
				//e.Graphics.DrawString((nViewableDays.ToString() + " " + Year.ToString()), dayTextFont, dayTextBrush, 10, 30);
				
				// variables used in loop. Declare outside for fast
				int nDayNumber = 0;
				int nModifier = -1;
				
				// generate the month based on the start date
				//			int nMonth = startDate.Month;
				
				if (nMonth-1 > calendar.NumberOfZoomOutPanels)
				{
					NewMessage.Show(Loc.Instance.GetStringFmt("{0} too many months",nMonth.ToString()));
				}
				else if (nMonth-1 < 0)
				{
					NewMessage.Show(Loc.Instance.GetStringFmt("{0} too few months",nMonth.ToString()));
				}
				sMonth = calendar.ZoomOutDetails[nMonth-1] + " {0}";
				
				
				try
				{
					// draw operations
					// probably should be building an array and building that
					// to be more flexible
					
					// we override Viewable days in the case of calendars that do not have years
					if (calendar.HasYears == false)
					{
						int DaysToStartingDate = calendar.DaysToStartingDate(startingStartDate);
						if (nViewableDays >= calendar.DaysTotal())
							nViewableDays = calendar.DaysTotal() + 1 - DaysToStartingDate;
						
						// algorith: Total Days - DaysToStartingDate
						
					}
					else
					{
						string sLabel = Year.ToString();
						if (MyTimeline.RowFilter != Constants.BLANK)
						{
							sLabel = Loc.Instance.GetStringFmt("{0} (Filter is active)",sLabel);
						}
						e.Graphics.DrawString(sLabel, new Font("Times", 12, FontStyle.Italic), new SolidBrush(Color.DarkGray), 10,
						                      10);
					}
					
					int nHalfWayPoint = 2 + (nViewableDays/2); // this will be used by the ZoomOut panel
					// to know the midpoint value
					for (int i = 1; i < nViewableDays; i++)
					{
						
						
						
						e.Graphics.DrawRectangle(dayPanelPen, (i-1)*dayPanelWidth,0, dayPanelWidth, thisPanel.Height);
						
						nDayNumber = nstartDate + (i + nModifier);
						
						
						
						nDaysInCurrentMonth = calendar.GetDaysInMonth(Year, nMonth);
						
						
						
						
						// this should only happen when I pass into a new month
						if (nDayNumber > nDaysInCurrentMonth)
						{
							nMonth++;
							
							if (nMonth == calendar.NumberOfZoomOutPanels + 1)
							{
								if (calendar.HasYears == true)
								{
									Year++;
									nMonth=1;
								}
								else
								{
									// block scrolling
									nMonth--;
									//	bLastDraw = true; // this is the last draw we'll do
									// because we have hit the end of a year
									// in a calendar that does not have years did not work
									//return;
								}
								
							}
							
							
							startDate = new newGenericDate(Year, nMonth, 1);
							
							nstartDate = 1;
							nModifier = (-1 * i) ; // set counter back
							
							sMonth = calendar.ZoomOutDetails[nMonth-1] + " {0}";;
							nDayNumber = nstartDate + (i + nModifier);
							
							
						}
						if (i == nHalfWayPoint)
						{
							// store the value of the midpoint 
							// to use when drawing the zoomout panel
							nHalfwayDay = nDayNumber;
							nHalfwayMonth = nMonth;
							nHalfwayYear = Year;
						}
						// display day

						e.Graphics.DrawString(String.Format(sMonth, nDayNumber), dayTextFont, dayTextBrush, (i-1)*dayTextWidth,
						                      dayTextHeight);
						
						// draw map note
						DateTime myDate = new DateTime(Year, nMonth, nDayNumber);
						
						int nNumberFound = 0;
						// check holidays
						if (calendar.Holidays != null)
						{
							foreach (holiday mHoliday in calendar.Holidays)
							{
								//if (mHoliday != null)
								{
									DateTime compareDate = new DateTime(myDate.Year, mHoliday.date.Month,
									                                    mHoliday.date.Day);
									if (compareDate == myDate)
									{
										AddTimelineNote(mHoliday.sText, mHoliday.nIcon, ref nNumberFound
										                , i,e, mHoliday.sCaption, dayPanelWidth, 0, "",0,-1);
									}
								}
								
							}
						}
						
						// check user added events

						// THIS HAS BEEN CUT FOR YOM 2013. Table is only way to populate data
//						if (appearance.Dates != null && appearance.Files != null)
//						{
//							
//							for (int j = 0; j < this.appearance.Dates.Length; j++)
//							{
//								string s = this.appearance.Dates[j];
//								
//								DateTime compareDate = SafeDateParse(s); // DateTime.Parse(s);
//								
//								
//								if (compareDate == myDate)
//								{
//									AddTimelineNote(this.appearance.Files[j],appearance.Icons[j], 
//									                ref nNumberFound, i,e, appearance.Captions[j]);
//									
//								}
//							}
//						} // files and dates not null


						// build a list of datatables FIRST
						// so we have access to them later


						if (MyTables.Count <= 0)
						{
						foreach (string s in MyTimeline.Listoftableguids)
						{
							if ("*" != s && "" != s && " " != s)
							{
								NoteDataXML_Table AlreadyPresent=	(NoteDataXML_Table)MyTables.Find (NoteDataXML_Table=>NoteDataXML_Table.GuidForNote == s );
									if (null == AlreadyPresent)
									{
										MyTables.Add (MyTimeline.GetTableForThisTimeline(s));
									}
							}
							}
						}
					
						// in addition to notes (May 2012) -- parse an associated DataTable
						// of the same format as the EVENTTABLE, looking for dates matching
						// This exact day
						// Render them.
						//foreach (string s in MyTimeline.Listoftableguids)
						string GuidsAdded="";
							foreach (NoteDataXML_Table table in MyTables)
						{

							// There is an issue where somehow this is called twice, simulatenous
							// and regardless of the checks above (May 2013) it still adds 
							// the same thing twice to MyTables.\
							if (GuidsAdded.IndexOf(table.GuidForNote) > -1)
							{
								// don't draw this NOTE again
								break;
							}

							GuidsAdded = GuidsAdded + table.GuidForNote;



							//if ("*" != s)
							{

								// test if table exists AND is a table
								try
								{


									//had an optimization here to only grab the table when we needed a new one BUT 
									// this does not work when you have multiple. We need to grab it each time. This will
									// cause performance hits


										CurrentVersionofTableForTHisTimeline = table;//MyTimeline.GetTableForThisTimeline(s);

									if (null==CurrentVersionofTableForTHisTimeline)
									{
										return;
									}
									if (CurrentVersionofTableForTHisTimeline != null)
									{
										
										
										if (CurrentVersionofTableForTHisTimeline.dataSource != null)
										{
											DataView view = new DataView(CurrentVersionofTableForTHisTimeline.dataSource);
											if (MyTimeline.RowFilter != "")
											{
												try
												{
													view.RowFilter = MyTimeline.RowFilter;
												}
												catch (Exception)
												{
													NewMessage.Show (Loc.Instance.GetString ("There was an error with your rowfilter. It should be in the format of validColumn=validValue. Case does matter."));
													view.RowFilter = Constants.BLANK;
													MyTimeline.RowFilter = Constants.BLANK;
												}
											}
											if (view != null)
											{
												//                                                foreach (DataRow row in table.appearance.dataSource.Tables[0].Rows)
												foreach (DataRowView row in view)
												{
													DateTime compareDate;
													// if other then blank then this holds the type of plot category (i.e., rising action)
													string lookup = Constants.BLANK;
													// do string replacement for "plot" dates
													int lookupi = 0;
													// we pass extra information in to annotate the plot view
													int chapter = 0;
													if ( row != null && row[0] != null && row[0].ToString().Length > 0 && row[0].ToString()[0] == '*')
													{
														// now remove the * and check the string to generate a fake date
														lookup = row[0].ToString();
														lookup = lookup.Replace ("*", "").Trim ();
														string[] values = lookup.Split (new char[1] {';'}, StringSplitOptions.RemoveEmptyEntries);
														compareDate =newGenericDate.SafeDateParse("01/01/1999"); 
														if (values != null && values.Length > 0)
														{
															lookup = values[0];

															Int32.TryParse (lookup, out lookupi);



															int TotalDays  = this.calendar.DaysTotal();
															bool foundone = false;
															// if the numbers passed exceed total time in year
															// we lump everything onto the last day of the year
															for (int daycounter=1; daycounter <= TotalDays; daycounter++)
															{
																// we have found the day # that matches the requested passed in
																if (daycounter == lookupi)
																{
																	compareDate = calendar.GetSafeDateFromDayOfYear(daycounter);
																	foundone = true;
																	break;
																}
															}

															if (false == foundone)
															{
																compareDate = calendar.GetSafeDateFromDayOfYear(TotalDays);
															}


//															switch (lookupi)
//															{
//															case 1: compareDate =newGenericDate.SafeDateParse("01/01/1999"); break;
//															case 2: compareDate =newGenericDate.SafeDateParse("02/01/1999"); break;
//															case 3: compareDate =newGenericDate.SafeDateParse("01/02/1999"); break;
//															case 4: compareDate =newGenericDate.SafeDateParse("02/02/1999"); break;
//															case 5: compareDate =newGenericDate.SafeDateParse("03/02/1999"); break;
//															case 6: compareDate =newGenericDate.SafeDateParse("04/02/1999"); break;
//															case 7: compareDate =newGenericDate.SafeDateParse("05/02/1999"); break;
//																// climax
//															case 8: compareDate =newGenericDate.SafeDateParse("01/03/1999"); break;
//															case 9: compareDate =newGenericDate.SafeDateParse("02/03/1999"); break;
//																// conclusion
//															case 10: compareDate =newGenericDate.SafeDateParse("01/04/1999"); break;
//															}

															if (values.Length == 2)
															{
																Int32.TryParse(values[1], out chapter);
															}
														}

													}
													else
														compareDate = newGenericDate.SafeDateParse(row[0].ToString()); // DateTime.Parse(s);
													if (compareDate == myDate)
													{
														int icon = 0;
														
														int icon_idx = CurrentVersionofTableForTHisTimeline.dataSource.Columns.IndexOf("icon");
														int data3_idx = CurrentVersionofTableForTHisTimeline.dataSource.Columns.IndexOf("Data3");
														int data4_idx = CurrentVersionofTableForTHisTimeline.dataSource.Columns.IndexOf("Data4");

														if (icon_idx < 0)
														{
															// try with capital
															icon_idx = CurrentVersionofTableForTHisTimeline.dataSource.Columns.IndexOf("Icon");
														}
														if (icon_idx > -1)
														{
															try
															{
																// we found an icon column
																if (Int32.TryParse(row[icon_idx].ToString(), out icon) == false)
																{
																	icon = 0;
																}
															}
															catch (Exception)
															{
																icon = 0;
															}
														}
														
														string title = /*row[2].ToString() + "|"+*/row[3].ToString();
														string description = row[2].ToString();
														string data3 = row[data3_idx].ToString();
														string data4 = row[data4_idx].ToString();

														int data4_i = -1;
														if (data4 != Constants.BLANK && Int32.TryParse(data4, out data4_i) == false)
														{
															data4_i = -1;
														}
														// tables assumed to be pattern on EventTable
														// with *at least* first column  = the date
														// third column the STRING title
														AddTimelineNote("*" + description, icon,
														                ref nNumberFound, i, e, title, dayPanelWidth, lookupi,data3, chapter, data4_i);
													}
												}
											}
											
											
											
										}
									}
								}
								catch (Exception ex)
								{
									NewMessage.Show(ex.ToString());
								}
							}
						}
						
						/*
						if (myDate == compareDate)
						{
						
					
						/*	Button button = new Button();
							button.Left = (i-1)*dayPanelWidth;
							button.Top = 10;
							button.Parent = thisPanel;
							button.Visible = true;
							button.Height = 20;
							button.Width = 20;
							button.Cursor = Cursors.Arrow;

							button.Image = imageList1.Images[0];
						
						
						
							//mutex_AddButton = false;

						}*/
						
					}
					
				}
				catch (Exception ex)
				{
					NewMessage.Show(ex.ToString());
				}
				startDate = startingStartDate;
				mutex_AddButton = false;
				panelZoomOut.Refresh();
				
				//Seconed you can get the Graphics object by callin CreateGraphics() method
				//panel1.CreateGraphics() INSTEAD
			}//catch
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString() + " in zoomin panel panel");
			}
		}
		
		/// <summary>
		/// opens a mosue notes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// moved from mouse over, if I pressa mousebutton down I am commmited and I can set the object inspector to this one
			//((VirtualDesktop)this.Parent.Parent).SetTooltip(appearance);
			
			// clear the array everytime we paint and rebuild it?
			
			
			// here we circle through the array of Hotspots on the current view
			
			// can I test if I am over an icon somehow
			//Graphics graphics = (sender as Panel).CreateGraphics() ;
			//graphics.
			if (smartPoints != null)
			{
				foreach(SmartPoint smart in smartPoints)
				{
					if ( (e.X >= smart.x && e.X <= smart.x+10) && (e.Y >= smart.y && e.Y <= smart.y+10))
					{
						//Message Box.Show(smart.s);
						//VirtualDesktop myParent = (VirtualDesktop)this.Parent.Parent;
						if (smart.s != null && smart.s.Length > 0 && smart.s[0] == '*')
						{
							// may 2012 - if clicking on a note from a DataTable we
							// should pop open the secondary information
							NewMessage.Show(smart.s);
						}
						//else
						//	myParent.OpenNoteWindow(smart.s, this, this.Left, this.Height);
						return;
					}
				}
			}
			
			//startDate.Add(new TimeSpan(1,0,0,0));
			// make startdate on the properites panel
			//startDate.Add(new TimeSpan(24));
			if (e.Button == MouseButtons.Left)
			{
				
				startDate= calendar.AddDays(startDate,1);
				MyTimeline.TimelineStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
				//panel1.Invalidate();
				panelZoomIn.Refresh();
					panelZoomOut.Refresh();
			}
			else
			{
				startDate=calendar.AddDays(startDate,-1);
				MyTimeline.TimelineStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
				panel1.Invalidate();
				
								panelZoomIn.Refresh();
				panelZoomOut.Refresh();
			}
		}
		
		private void panel1_Resize(object sender, System.EventArgs e)
		{
			Refresh();
		}
		
		/// <summary>
		///  this is the zoomed out panel (months panel by default)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelZoomOut_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				if (mutex_AddButton == true) return;
				
				if (nHalfwayDay > 0)
				{
					// draw only enough Month boxes to fit on screen
					// each month box should be 3 x the width
					Panel thisPanel = (sender as Panel);
					
					Pen monthPanelPen = new Pen(Color.Brown);
					Pen caretPen = new Pen(ZoomOutBackgroundColor);
					
					// draw middle caret
					int nMidPoint = thisPanel.Width / 2;
					int nCaretWidth = 100;
					int nCaretX = nMidPoint - (nCaretWidth/2);
					Rectangle rect = new Rectangle(nCaretX, 0, dayPanelWidth/2, thisPanel.Height);


//					Brush caretBrush = new LinearGradientBrush(rect, appearance.TimelineGradient.Color1, PINK THEN GREEN THEN HORIZONTOAL
//					                                           appearance.TimelineGradient.Color2, appearance.TimelineGradient.GradientMode);


		

					Brush caretBrush = new LinearGradientBrush(rect, GradientColor1,
					                                           GradientColor2, LinearGradientMode.Horizontal);

					//				LinearGradientBrush lgBrush = new LinearGradientBrush(rect,Color.Black,Color.White,LinearGradientMode.Horizontal);			
					e.Graphics.DrawRectangle(caretPen, nCaretX, 0, dayPanelWidth/2, thisPanel.Height);
					e.Graphics.FillRectangle(caretBrush, nCaretX, 0, dayPanelWidth/2, thisPanel.Height);
					
					
					
					double fText = 1.0;
					
					
					
					
					fText =  (double) nHalfwayDay;
					
					double fPercent = fText/31.0;
					
					int nLeft = (int) (nCaretX - ((dayPanelWidth*2.6*fPercent)-dayPanelWidth/ (80*fPercent)));
					
					//	e.Graphics.DrawRectangle(monthPanelPen, nLeft, 0, dayPanelWidth*3, thisPanel.Height);
					
					
					// draw 2 left and right grids
					DrawZoomOutBox(e, nLeft, calendar.ZoomOutDetails[nHalfwayMonth-1], thisPanel.Height,0, nHalfwayMonth);
					
					DrawZoomOutBox(e, nLeft + (dayPanelWidth*3), GetMonth(nHalfwayMonth,1), thisPanel.Height,1, nHalfwayMonth + 1);
					DrawZoomOutBox(e, nLeft + (2*(dayPanelWidth*3)), GetMonth(nHalfwayMonth,2), thisPanel.Height,1, nHalfwayMonth+2);
					
					// don't show negative panels if not a true calendar
					//if (calendar.HasYears == true) this did not do what I thought it might
					{
						DrawZoomOutBox(e, nLeft - (dayPanelWidth*3), GetMonth(nHalfwayMonth,-1), thisPanel.Height,-1, nHalfwayMonth-1);
						DrawZoomOutBox(e, nLeft - (2*(dayPanelWidth*3)), GetMonth(nHalfwayMonth,-2), thisPanel.Height,-1, nHalfwayMonth-2);
					}
					//e.Graphics.DrawRectangle(monthPanelPen, nLeft- (dayPanelWidth*3), 0, dayPanelWidth*3, thisPanel.Height);
					//e.Graphics.DrawRectangle(monthPanelPen, nLeft + ((dayPanelWidth*3)), 0, dayPanelWidth*3, thisPanel.Height);
					/*
									string sMonth = Months[nHalfwayMonth-1];

									Font monthTextFont = new Font("Georgie", 20);
									Brush monthTextBrush = new SolidBrush(Color.Red);

									e.Graphics.DrawString(String.Format(sMonth), monthTextFont, monthTextBrush,
										nLeft,
										10);

									// right and left text
									//string sMonthBefore = GetMonth(nHalfwayMonth,-1); 
									//e.Graphics.DrawString(String.Format(sMonthBefore), monthTextFont, monthTextBrush,
									//	nLeft - (dayPanelWidth*3),
									//	10);
				
									string sMonthAfter = GetMonth(nHalfwayMonth,1); 
									e.Graphics.DrawString(String.Format(sMonthAfter), monthTextFont, monthTextBrush,
										nLeft + ((dayPanelWidth*3)),
										10);
					*/
				} // halfway day greater than 0
			}// try
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			
			
			// control: should always move days on the dayPanel and have that redraw trigger this redraw afterwards
		}
		
		/// <summary>
		/// draws a month box with the month text
		/// </summary>
		/// <param name="nLeft"></param>
		/// <param name="nRelative">-1 = before panel 0 = middle, 1 = after panel</param>
		private void DrawZoomOutBox(System.Windows.Forms.PaintEventArgs e, int nLeft, string sMonth, int nHeight, int nRelative, int nMonth)
		{
			try
			{
				Pen monthPanelPen = new Pen(Color.Brown);
				//= new Font("Georgie", 20);
				//Brush monthTextBrush = new SolidBrush(Color.Red);
				
				
				int nWidth = dayPanelWidth * calendar.GetWidthForMonth(nMonth-1); //dayPanelWidth*3
				
				e.Graphics.DrawRectangle(monthPanelPen, nLeft, 0, nWidth, nHeight);
				e.Graphics.DrawString(String.Format(sMonth), monthTextFont, monthTextBrush,
				                      nLeft,10);
				
				// if beginning of a year put in date
				if (sMonth == calendar.ZoomOutDetails[0])
				{
					int nYear = nHalfwayYear;
					if (nRelative == 1 && calendar.HasYears == true) nYear++;
					//	if (nRelative == -1) nYear--;
					
					if (calendar.HasYears == true)
					{
						e.Graphics.DrawString(nYear.ToString(), monthTextFont, monthTextBrush, nLeft, 60);
					}
				}
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
		}
		/// <summary>
		/// uses 0 index Months array
		/// tries to pick a safe string
		/// used when building the zoom out control
		/// </summary>
		/// <param name="nMonthPos"></param>
		/// <param name="?"></param>
		/// <returns></returns>
		private string GetMonth(int nMonth, int nModifier)
		{
			int nMonthPos = nMonth + nModifier;
			
			
			if (nMonthPos >= calendar.NumberOfZoomOutPanels +1)
			{
				nMonthPos = nMonthPos - calendar.NumberOfZoomOutPanels;
			}
			else
				if (nMonthPos <= 0)
			{
				nMonthPos = calendar.NumberOfZoomOutPanels + (nMonthPos);
			}
			
			
			
			
			/*if (nMonthPos == 14)
			{
				nMonthPos = 2;
			}
			else
			if (nMonthPos == 13)
			{
				nMonthPos = 1;
			}
			else
				if (nMonthPos == 0)
			{
				nMonthPos = 12;
			}
			else
				if (nMonthPos == -1)
			{
				nMonthPos = 11;
			}

			*/
			string sValue = "";
			try
			{
				sValue = calendar.ZoomOutDetails[nMonthPos-1];
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString() + "  in GetMonth");
			}
			return sValue;
			
		}
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			panelZoomOut.Refresh();
		}
		
		private int global_nZoomOutCurrentPosition = -1;
		private bool global_bZoomoutMouseDown = false;
		
		/// <summary>
		/// allow mouse drag on this form to scroll
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelZoomOut_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// october 2008 moves this from mouse over, if you press a mouse butotn then it makes sene to take ownership of this.
			//((VirtualDesktop)this.Parent.Parent).SetTooltip(appearance);
			
			
			global_nZoomOutCurrentPosition = e.X;
			global_bZoomoutMouseDown = true;
			
		}
		
		private void panelZoomOut_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			global_bZoomoutMouseDown = false;
			
			
		}
		
		/// <summary>
		/// scroll the months
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelZoomOut_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//((VirtualDesktop)this.Parent.Parent).SetTooltip(appearance); October 2008 - removing this
			/* but I'm sure there will be a bug based on it
             * but its annoying that it steals the focus
             */
			
			if (global_nZoomOutCurrentPosition == -1) return; 
			if (global_bZoomoutMouseDown == true)
			{
				// if new position less than
				if (e.X > global_nZoomOutCurrentPosition) 
				{
					startDate= calendar.AddDays(startDate,5);
					panelZoomIn.Refresh();
				}
				else if (e.X < global_nZoomOutCurrentPosition) 
				{
					startDate=calendar.AddDays(startDate,-5);
					panelZoomIn.Refresh();
				}
				global_nZoomOutCurrentPosition = e.X;
				//global_bZoomoutMouseDown = false;
			}
			
		}
		ToolTip tips = new ToolTip();
		/// <summary>
		/// Change to hand cursor when overf a note
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelZoomIn_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			/* but I'm sure there will be a bug based on it
                        * but its annoying that it steals the focus
                        */
			//((VirtualDesktop)this.Parent.Parent).SetTooltip(appearance);
			
			if (smartPoints != null)
			{
				foreach(SmartPoint smart in smartPoints)
				{
					if ( (e.X >= smart.x && e.X <= smart.x+10) && (e.Y >= smart.y && e.Y <= smart.y+10) )
					{
						//Mess ageBox.Show(smart.s);
						Cursor = Cursors.Hand;
						if (smart.label != Constants.BLANK && string.IsNullOrEmpty(tips.GetToolTip(this)))
						{
							tips.Show (smart.label, this, new Point(e.X, e.Y));
						}
						return;
					}
					else
					{
						if (!string.IsNullOrEmpty(tips.GetToolTip(this)))
						{
							// hide any visible tooltip
							tips.Hide (this);
						}
						Cursor = Cursors.SizeAll;
					}
				}
			}
			
		}
		
		/// <summary>
		/// when the notepanel is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NotePanelTimeline_Load(object sender, System.EventArgs e)
		{
			
			if (calendar == null)
			{
				NewMessage.Show("Development Message: Calendar not defined. You should close.");
				
				
			}
		}
		
		private void contextMenu1_Popup(object sender, EventArgs e)
		{
			
		}
		protected override void WndProc(ref Message m)
		{
			
			if (m.Msg == 0x7b /*WM_CONTEXTMENU*/ )
			{
				
				return;
				
			}
			base.WndProc(ref m); 
		}
	}
}

