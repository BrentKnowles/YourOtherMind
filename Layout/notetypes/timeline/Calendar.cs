// Calendar.cs
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
using System.Xml;
using System.IO;
using Layout;
using System.Collections.Generic;

namespace Timeline
{
	/// <summary>
	/// This class is used to store information about the data used to build the calendar
	/// 
	/// It will allow
	/// * A traditional calendar
	/// * A fantasy calendar
	/// * A relative calendar (i.e., plot flow)
	/// </summary>
	public class Calendar
	{
		#region constant
		public enum calendertype  
		{
			Gregorian, Plot
		};

		#endregion 
		private int nNumberOfZoomOutPanels;


		
		private List<holiday> holidays;
		public List<holiday> Holidays
		{
			get {return holidays;}
			set {holidays = value;}
		}
		
		
		/// <summary>
		/// Also known as "Number of Months"
		/// </summary>
		public int NumberOfZoomOutPanels
		{
			get {return nNumberOfZoomOutPanels;}
			set {nNumberOfZoomOutPanels = value;}
		}
		
		private string mMonthLabel;
		/// <summary>
		/// Used on the add date pick screen to give a label to custom calendars
		/// </summary>
		public string MonthLabel
		{
			get {return mMonthLabel;}
			set {mMonthLabel = value;}
		}
		private string mDayLabel;
		/// <summary>
		/// Used on the add date pick screen to give a label to custom calendars
		/// </summary>
		public string DayLabel
		{
			get {return mDayLabel;}
			set {mDayLabel = value;}
		}
		private string mYearLabel;
		/// <summary>
		/// Used on the add date pick screen to give a label to custom calendars
		/// </summary>
		public string YearLabel
		{
			get {return mYearLabel;}
			set {mYearLabel = value;}
		}
		
		
		private bool mHasYears;
		public bool HasYears
		{
			get {return mHasYears;}
			set {mHasYears = value;}
		}
		private List<string> mZoomOutDetails;
		/// <summary>
		/// "Months"
		/// </summary>
		public List<string> ZoomOutDetails
		{
			get {return mZoomOutDetails;}
			set {mZoomOutDetails = value;}
		}
		
		private List<int> mZoomInPanels;
		/// <summary>
		///  How many days per month
		/// </summary>
		public List<int> ZoomInPanels
		{
			get {return mZoomInPanels;}
			set {mZoomInPanels = value;}
			
		}
		
		private List<int> mZoomInPanelRules;
		/// <summary>
		/// This array stores special codes relating to whether the
		/// "months" have any special rules (like leap days)
		/// 
		/// -1 = No Rule
		/// Greater than or equal to zero is how many days in a Leap year
		/// </summary>
		public List<int> ZoomInPanelRules
		{
			get {return mZoomInPanelRules;}
			set {mZoomInPanelRules = value;}
		}
		
		private List<int> mZoomOutWidths;
		/// <summary>
		/// NOTE: WIdths don't actually do anything, I couldn't get this to work and it was not a super important idea
		/// </summary>
		public List<int> ZoomOutWidths
		{
			get {return mZoomOutWidths;}
			set {mZoomOutWidths = value;}
		}
		
		private calendertype mcalendarType;
		public calendertype CalendarType
		{
			get {return mcalendarType;}
			set 
			{
				mcalendarType = value;
				// xml load happens in the appearancelcass
				
			}
		}
		
		/// <summary>
		/// iterates through ZoomInPanel array and looks at number of days per month
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="nMonth"></param>
		/// <returns></returns>
		public int GetDaysInMonth(int nYear, int nMonth)
		{
			int nValue = mZoomInPanels[nMonth-1];
			
			bool bLeap = false;
			// check for leap year
			int nMonthCode = mZoomInPanelRules[nMonth-1];
			if (nMonthCode != -1)
			{
				if (nYear % 400 == 0) bLeap = true;
				else if (nYear % 100 == 0) bLeap = false;
				else if (nYear %4 == 0) bLeap = true;
				else bLeap = false;
				
				if (bLeap == true)
				{
					nValue = nMonthCode;
				}
			}
			return nValue;
		}
		public Calendar()
		{
			ZoomInPanels = new List<int>();
			ZoomInPanelRules = new List<int>();
			ZoomOutWidths = new List<int>();
			ZoomOutDetails = new List<string>();

			
			
		}
		/// <summary>
		/// reutrns the total number of days in the year
		/// </summary>
		/// <returns></returns>
		public int DaysTotal()
		{
			int nCount=0;
			foreach (int nDays in mZoomInPanels)
			{
				nCount += nDays;
			}
			return nCount;
		}
		/// <summary>
		/// returns the width of the box for the month
		/// 
		/// NOTE: WIdths don't actually do anything, I couldn't get this to work and it was not a super important idea
		/// </summary>
		/// <param name="nMonth"></param>
		/// <returns></returns>
		public int GetWidthForMonth(int nMonth)
		{
			if (nMonth < 0) nMonth = mZoomOutWidths.Count-1;
			if (nMonth >= mZoomOutWidths.Count) nMonth = 0;
			int nWidth = mZoomOutWidths[nMonth];
			
			
			return nWidth;
		}
		/*	/// <summary>
		/// called from the appearance class if this is a plot type instead
		/// 
		/// ?: How will dates be stored?
		/// </summary>
		public void SetToPlotOutline()
		{
			CalendarType = Appearance.calendertype.Plot;
/*
			mZoomOutDetails = new string[]{"Introduction", "Rising Action", "Climax", "Conclusion"};
			mZoomInPanels = new int[]{2, 5, 2, 1};
			mZoomInPanelRules = new int[]{-1,-1,-1,-1};
			mZoomOutWidths = new int[]{3,3,3,3};
			nNumberOfZoomOutPanels = 4;
			MonthLabel = "Segment";
			DayLabel = "Moment";
			YearLabel = "None";
			HasYears = false;*/
		
		//}
		
		/// <summary>
		/// adds days to the timeline
		/// </summary>
		/// <param name="date"></param>
		/// <param name="nDays"></param>
		/// <returns></returns>
		public newGenericDate AddDays(newGenericDate date, int nDays)
		{
			
			// logic behind adding days
			// figure out if end of month 
			// : Day > MaxDaysForMonth
			bool bIsMonthEnd = false;
			bool bIsYearEnd = false;
			
			
			
			// set new day
			date.Day = date.Day + nDays;
			
			if (date.Day < 1)
			{
				date.Month--;
				// going back a year
				if (date.Month < 1)
				{
					if (HasYears == true)
					{
						date.Month = ZoomOutDetails.Count;
						date.Year--;
						
						
					}
					
					
				}
				// error -handling
				if (date.Month <= 0)
				{
					date.Month =1 ;
					date.Day  = 1;
				}
				else
					date.Day = ZoomInPanels[date.Month - 1];
			}
			
			if (date.Day > ZoomInPanels[date.Month -1])
			{
				bIsMonthEnd  = true;
			}
			
			if (bIsMonthEnd  == true)
			{
				// if month++ would exceed number of months
				// increment months
				if (date.Month + 1 > ZoomOutDetails.Count)
				{
					bIsYearEnd = true;
				}
			}
			
			if (bIsMonthEnd == true)
			{
				date.Day = 1;
				date.Month++; 
				
			}
			if (bIsYearEnd == true) 
			{
				if (HasYears == true)
				{
					date.Month = 1;
					date.Year++;
				}
				else
				{
					date.Month = date.Month -2; // set month back to current month if this type of calendar does not have years
					date.Day = ZoomInPanels[date.Month - 1];
				}
			}
			
			return date;
		}
		/// <summary>
		/// returns the number of days to this specific dates
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public int DaysToStartingDate(newGenericDate date)
		{
			int nCount = 0;
			for(int i = 0 ; i < date.Month-1; i++)
			{
				
				nCount += mZoomInPanels[i];
				
			}
			nCount+=date.Day;
			return nCount-1;
		}
		/// <summary>
		/// saves the file out.
		/// assumes global path has already been added to this.
		/// </summary>
		/// <param name="sFile"></param>
		public void SaveCalendar(string sFile)
		{
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
			
			TextWriter tw = new StreamWriter(sFile);
			x.Serialize(tw, this);
			tw.Close();
			
		}

		public static Calendar BuildGregorianDefault()
		{
			Calendar returnCalendar = new Calendar();
			returnCalendar.Holidays = new List<holiday>();
			returnCalendar.Holidays.Add (new holiday(1, "Remembrance Day", new newGenericDate(1, 11, 11), "Remembrance Day"));
			returnCalendar.Holidays.Add (new holiday(1, "Christmas Day", new newGenericDate(1, 12, 25), "Christmas Day"));
			returnCalendar.NumberOfZoomOutPanels = 12;
			returnCalendar.HasYears = true;

			returnCalendar.ZoomOutDetails.Add ("Jan");
			returnCalendar.ZoomOutDetails.Add ("Feb");
			returnCalendar.ZoomOutDetails.Add ("Mar");
			returnCalendar.ZoomOutDetails.Add ("Apr");
			returnCalendar.ZoomOutDetails.Add ("May");
			returnCalendar.ZoomOutDetails.Add ("June");
			returnCalendar.ZoomOutDetails.Add ("July");
			returnCalendar.ZoomOutDetails.Add ("Aug");
			returnCalendar.ZoomOutDetails.Add ("Sep");
			returnCalendar.ZoomOutDetails.Add ("Oct");
			returnCalendar.ZoomOutDetails.Add ("Nov");
			returnCalendar.ZoomOutDetails.Add ("Dec");

			returnCalendar.ZoomInPanels.AddRange(new int[12]{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31});
			returnCalendar.ZoomInPanelRules.AddRange(new int[12]{-1, 29, -1,-1,-1,-1,-1,-1,-1,-1,-1,-1});
			returnCalendar.ZoomOutWidths.AddRange(new int[12]{3,3,3,3,3,3,3,3,3,3,3,3});
			returnCalendar.CalendarType = calendertype.Gregorian;	

				
			return returnCalendar;
				

		}

		public static Calendar BuildPlotDefault ()
		{
			Calendar returnCalendar = new Calendar ();

			returnCalendar.NumberOfZoomOutPanels = 4;
			returnCalendar.MonthLabel = "Segment";
			returnCalendar.DayLabel = "Moment";
			returnCalendar.YearLabel = "None";
			returnCalendar.HasYears = false;
			returnCalendar.ZoomOutDetails.AddRange (new string[4] {"Introduction", "Rising Action", "Climax", "Conclusion"});
					
			returnCalendar.ZoomInPanels.AddRange (new int[4]{2,5,2,1});
			returnCalendar.ZoomInPanelRules.AddRange (new int[4]{-1,-1,-1,-1});
			returnCalendar.ZoomOutWidths.AddRange (new int[4]{3,3,3,3});
					
			returnCalendar.CalendarType = calendertype.Plot;

			return returnCalendar;
		}
		
	} // class
	
	
	
}
